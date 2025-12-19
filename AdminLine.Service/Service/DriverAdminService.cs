using AdminLine.Common.DTOs;
using AdminLine.Common.Helper;
using AdminLine.Framework.UoW;
using AdminLine.Service.Helpers;
using AdminLine.Service.IService;
using Lines.Application.Interfaces;
using Lines.Domain.Enums;
using Lines.Domain.Models.Drivers;
using Lines.Domain.Models.Trips;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Text;

namespace AdminLine.Service.Service;

public class DriverAdminService : IDriverAdminService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DriverAdminService> _logger;
    private readonly IDriverConnectionService _driverConnectionService;
    private readonly IApplicationUserService _applicationUserService;
    private readonly IMemoryCache _cache;

    public DriverAdminService(
        IUnitOfWork unitOfWork,
        ILogger<DriverAdminService> logger,
        IDriverConnectionService driverConnectionService,
        IApplicationUserService applicationUserService,
        IMemoryCache cache)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _driverConnectionService = driverConnectionService;
        _applicationUserService = applicationUserService;
        _cache = cache;
    }

    public async Task<PagingDto<DriverListDto>> GetDriversAsync(
        DriverFilterDto filter,
        DriverSortDto sort,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var driverRepository = _unitOfWork.GetRepository<Driver>();
            var tripRepository = _unitOfWork.GetRepository<Trip>();
            var earningRepository = _unitOfWork.GetRepository<Earning>();

            // Build base query
            var query = driverRepository.GetAll(d => !d.IsDeleted);

            // Apply search filter
            if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
            {
                var searchTerm = filter.SearchTerm.ToLower();
                query = query.Where(d =>
                    d.FirstName.ToLower().Contains(searchTerm) ||
                    d.LastName.ToLower().Contains(searchTerm) ||
                    d.Email.Value.ToLower().Contains(searchTerm) ||
                    d.PhoneNumber.Value.ToLower().Contains(searchTerm));
            }

            // Apply status filter
            if (filter.Status.HasValue)
            {
                query = filter.Status.Value switch
                {
                    DriverStatus.Active => query.Where(d =>
                        d.RegistrationStatus == RegistrationStatus.Verified &&
                        d.IsAvailable &&
                        d.IsRegistrationComplete()),
                    DriverStatus.Suspended => query.Where(d =>
                        d.RegistrationStatus == RegistrationStatus.Verified &&
                        !d.IsAvailable),
                    DriverStatus.PendingReview => query.Where(d =>
                        d.RegistrationStatus == RegistrationStatus.PendingReview),
                    DriverStatus.ProfileUpdate => query.Where(d =>
                        d.RegistrationStatus == RegistrationStatus.Verified &&
                        !d.IsRegistrationComplete()),
                    _ => query
                };
            }

            // Get all drivers matching filters (before aggregation)
            var drivers = await query
                .Include(d => d.Trips)
                .Include(d => d.Earnings)
                .ToListAsync(cancellationToken);

            // Calculate aggregations and map to DTOs
            var driverDtos = new List<DriverListDto>();

            foreach (var driver in drivers)
            {
                // Filter trips and earnings in memory
                var completedTrips = driver.Trips?.Where(t => t.Status == TripStatus.Completed).ToList() ?? new List<Trip>();
                var paidEarnings = driver.Earnings?.Where(e => e.IsPaid).ToList() ?? new List<Earning>();
                
                var totalTrips = completedTrips.Count;
                var totalEarnings = paidEarnings.Sum(e => e.Amount);
                var lastActivity = GetLastActivity(driver, completedTrips);

                // Apply range filters
                if (filter.MinTrips.HasValue && totalTrips < filter.MinTrips.Value)
                    continue;
                if (filter.MaxTrips.HasValue && totalTrips > filter.MaxTrips.Value)
                    continue;
                if (filter.MinEarnings.HasValue && totalEarnings < filter.MinEarnings.Value)
                    continue;
                if (filter.MaxEarnings.HasValue && totalEarnings > filter.MaxEarnings.Value)
                    continue;
                if (filter.MinRating.HasValue && driver.Rating < filter.MinRating.Value)
                    continue;
                if (filter.MaxRating.HasValue && driver.Rating > filter.MaxRating.Value)
                    continue;

                var dto = new DriverListDto
                {
                    DriverId = driver.Id,
                    DriverCode = DriverStatusHelper.GenerateDriverCode(driver.Id),
                    FullName = $"{driver.FirstName} {driver.LastName}",
                    Email = driver.Email.Value,
                    PhoneNumber = driver.PhoneNumber.Value,
                    Status = DriverStatusHelper.DetermineDriverStatus(driver),
                    TotalTrips = totalTrips > 0 ? totalTrips : null,
                    TotalEarnings = totalEarnings > 0 ? totalEarnings : null,
                    Rating = driver.Rating > 0 ? driver.Rating : null,
                    LastActivity = DriverStatusHelper.FormatLastActivity(lastActivity)
                };

                driverDtos.Add(dto);
            }

            // Apply sorting
            driverDtos = ApplySorting(driverDtos, sort);

            // Apply pagination
            var totalCount = driverDtos.Count;
            var pagedItems = driverDtos
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PagingDto<DriverListDto>(pagedItems, totalCount, pageNumber, pageSize);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting drivers list");
            throw;
        }
    }

    public async Task<DriverDetailDto?> GetDriverByIdAsync(Guid driverId, CancellationToken cancellationToken = default)
    {
        try
        {
            var driverRepository = _unitOfWork.GetRepository<Driver>();

            var driver = await driverRepository.GetAll(d => d.Id == driverId && !d.IsDeleted)
                .Include(d => d.Trips)
                .Include(d => d.Earnings)
                .Include(d => d.Vehicles)
                .Include(d => d.Licenses)
                .Include(d => d.BankAccounts)
                .Include(d => d.Addresses)
                .FirstOrDefaultAsync(cancellationToken);

            if (driver == null)
                return null;

            // Filter trips and earnings in memory
            var completedTrips = driver.Trips?.Where(t => t.Status == TripStatus.Completed).ToList() ?? new List<Trip>();
            var paidEarnings = driver.Earnings?.Where(e => e.IsPaid).ToList() ?? new List<Earning>();
            
            var totalTrips = completedTrips.Count;
            var totalEarnings = paidEarnings.Sum(e => e.Amount);
            var lastActivity = GetLastActivity(driver, completedTrips);
            var address = driver.Addresses?.FirstOrDefault()?.Address ?? string.Empty;

            var dto = new DriverDetailDto
            {
                DriverId = driver.Id,
                DriverCode = DriverStatusHelper.GenerateDriverCode(driver.Id),
                FullName = $"{driver.FirstName} {driver.LastName}",
                Email = driver.Email.Value,
                PhoneNumber = driver.PhoneNumber.Value,
                Status = DriverStatusHelper.DetermineDriverStatus(driver),
                TotalTrips = totalTrips > 0 ? totalTrips : null,
                TotalEarnings = totalEarnings > 0 ? totalEarnings : null,
                Rating = driver.Rating > 0 ? driver.Rating : null,
                LastActivity = DriverStatusHelper.FormatLastActivity(lastActivity),
                CompanyName = driver.CompanyName,
                DateOfBirth = driver.DateOfBirth,
                Address = address,
                PersonalPictureUrl = driver.PersonalPictureUrl,
                IsAvailable = driver.IsAvailable,
                CreatedDate = driver.CreatedDate,
                UpdatedDate = driver.UpdatedDate,
                Vehicles = driver.Vehicles?.Select(v => new VehicleInfoDto
                {
                    Id = v.Id,
                    Model = v.Model,
                    LicensePlate = v.LicensePlate,
                    IsActive = v.IsActive
                }).ToList() ?? new List<VehicleInfoDto>(),
                Licenses = driver.Licenses?.Select(l => new LicenseInfoDto
                {
                    Id = l.Id,
                    LicenseNumber = l.LicenseNumber,
                    ExpiryDate = l.ExpiryDate,
                    IsCurrent = l.IsCurrent
                }).ToList() ?? new List<LicenseInfoDto>(),
                BankAccounts = driver.BankAccounts?.Select(b => new BankAccountInfoDto
                {
                    Id = b.Id,
                    BankName = b.BankName,
                    Iban = b.IBAN,
                    IsPrimary = b.IsPrimary
                }).ToList() ?? new List<BankAccountInfoDto>()
            };

            return dto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting driver by ID: {DriverId}", driverId);
            throw;
        }
    }

    public async Task<bool> UpdateDriverStatusAsync(UpdateDriverStatusDto request, CancellationToken cancellationToken = default)
    {
        try
        {
            var driverRepository = _unitOfWork.GetRepository<Driver>();

            var driver = await driverRepository.GetAll(d => d.Id == request.DriverId && !d.IsDeleted)
                .FirstOrDefaultAsync(cancellationToken);

            if (driver == null)
            {
                _logger.LogWarning("Driver not found: {DriverId}", request.DriverId);
                return false;
            }

            // Update driver based on status
            switch (request.Status)
            {
                case DriverStatus.Active:
                    if (driver.RegistrationStatus != RegistrationStatus.Verified)
                    {
                        driver.RegistrationStatus = RegistrationStatus.Verified;
                    }
                    driver.IsAvailable = true;
                    break;

                case DriverStatus.Suspended:
                    if (driver.RegistrationStatus != RegistrationStatus.Verified)
                    {
                        driver.RegistrationStatus = RegistrationStatus.Verified;
                    }
                    driver.IsAvailable = false;
                    break;

                case DriverStatus.PendingReview:
                    driver.RegistrationStatus = RegistrationStatus.PendingReview;
                    break;

                case DriverStatus.ProfileUpdate:
                    // ProfileUpdate is a display status, not a stored status
                    // It's determined by Verified + incomplete registration
                    // No action needed, just ensure it's verified
                    if (driver.RegistrationStatus != RegistrationStatus.Verified)
                    {
                        driver.RegistrationStatus = RegistrationStatus.Verified;
                    }
                    break;
            }

            driver.UpdatedDate = DateTime.UtcNow;
            driverRepository.Update(driver);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Driver status updated: {DriverId} to {Status}", request.DriverId, request.Status);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating driver status: {DriverId}", request.DriverId);
            throw;
        }
    }

    public async Task<byte[]> ExportDriversAsync(
        DriverFilterDto filter,
        DriverSortDto sort,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // Get all drivers (no pagination for export)
            var drivers = await GetDriversAsync(filter, sort, 1, int.MaxValue, cancellationToken);

            // Generate CSV
            var csv = new StringBuilder();
            csv.AppendLine("Driver Code,Full Name,Email,Phone Number,Status,Total Trips,Total Earnings,Rating,Last Activity");

            foreach (var driver in drivers.Items)
            {
                csv.AppendLine($"{driver.DriverCode}," +
                    $"\"{driver.FullName}\"," +
                    $"\"{driver.Email}\"," +
                    $"\"{driver.PhoneNumber}\"," +
                    $"{driver.Status}," +
                    $"{driver.TotalTrips?.ToString() ?? "0"}," +
                    $"{driver.TotalEarnings?.ToString("F2") ?? "0.00"}," +
                    $"{driver.Rating?.ToString("F2") ?? "0.00"}," +
                    $"\"{driver.LastActivity ?? "Never"}\"");
            }

            return Encoding.UTF8.GetBytes(csv.ToString());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting drivers");
            throw;
        }
    }

    private DateTime? GetLastActivity(Driver driver, List<Trip>? completedTrips = null)
    {
        var activities = new List<DateTime?> { driver.UpdatedDate };

        var trips = completedTrips ?? driver.Trips?.Where(t => t.Status == TripStatus.Completed && t.EndedAt.HasValue).ToList();
        if (trips != null && trips.Any())
        {
            activities.AddRange(trips.Select(t => t.EndedAt));
        }

        return activities.Where(d => d.HasValue).Max();
    }

    private List<DriverListDto> ApplySorting(List<DriverListDto> drivers, DriverSortDto sort)
    {
        return sort.SortDirection == SortDirection.Ascending
            ? sort.SortBy switch
            {
                DriverSortField.Name => drivers.OrderBy(d => d.FullName).ToList(),
                DriverSortField.Trips => drivers.OrderBy(d => d.TotalTrips ?? 0).ToList(),
                DriverSortField.Earnings => drivers.OrderBy(d => d.TotalEarnings ?? 0).ToList(),
                DriverSortField.Rating => drivers.OrderBy(d => d.Rating ?? 0).ToList(),
                DriverSortField.LastActivity => drivers.OrderBy(d => d.LastActivity ?? "Never").ToList(),
                _ => drivers
            }
            : sort.SortBy switch
            {
                DriverSortField.Name => drivers.OrderByDescending(d => d.FullName).ToList(),
                DriverSortField.Trips => drivers.OrderByDescending(d => d.TotalTrips ?? 0).ToList(),
                DriverSortField.Earnings => drivers.OrderByDescending(d => d.TotalEarnings ?? 0).ToList(),
                DriverSortField.Rating => drivers.OrderByDescending(d => d.Rating ?? 0).ToList(),
                DriverSortField.LastActivity => drivers.OrderByDescending(d => d.LastActivity ?? "Never").ToList(),
                _ => drivers
            };
    }

    public async Task<DriverProfileHeaderDto?> GetDriverProfileHeaderAsync(Guid driverId, CancellationToken cancellationToken = default)
    {
        try
        {
            var driverRepository = _unitOfWork.GetRepository<Driver>();
            var emergencyContactRepository = _unitOfWork.GetRepository<DriverEmergencyContact>();

            var driver = await driverRepository.GetAll(d => d.Id == driverId && !d.IsDeleted)
                .FirstOrDefaultAsync(cancellationToken);

            if (driver == null)
                return null;

            // Check online status
            var connections = await _driverConnectionService.GetDriverConnectionsAsync(driverId);
            var isOnline = connections != null && connections.Any();

            // Count emergency contacts
            var emergencyCallsCount = await emergencyContactRepository.GetAll(
                ec => ec.DriverId == driverId && !ec.IsDeleted)
                .CountAsync(cancellationToken);

            // Determine verification status
            var isVerified = driver.RegistrationStatus == RegistrationStatus.Verified;

            var dto = new DriverProfileHeaderDto
            {
                DriverId = driver.Id,
                FullName = $"{driver.FirstName} {driver.LastName}",
                AvatarUrl = driver.PersonalPictureUrl,
                Rating = driver.Rating > 0 ? driver.Rating : null,
                IsOnline = isOnline,
                PhoneNumber = driver.PhoneNumber.Value,
                Email = driver.Email.Value,
                EmergencyCallsCount = emergencyCallsCount,
                IsVerified = isVerified
            };

            return dto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting driver profile header: {DriverId}", driverId);
            throw;
        }
    }

    public async Task<DriverPersonalInformationDto?> GetDriverPersonalInformationAsync(Guid driverId, CancellationToken cancellationToken = default)
    {
        try
        {
            var driverRepository = _unitOfWork.GetRepository<Driver>();

            var driver = await driverRepository.GetAll(d => d.Id == driverId && !d.IsDeleted)
                .Include(d => d.Addresses)
                    .ThenInclude(a => a.City)
                .Include(d => d.Addresses)
                    .ThenInclude(a => a.Sector)
                .Include(d => d.BankAccounts)
                .FirstOrDefaultAsync(cancellationToken);

            if (driver == null)
                return null;

            // Get primary address or first address
            var address = driver.Addresses?
                .FirstOrDefault(a => a.IsPrimary) ?? driver.Addresses?.FirstOrDefault();

            // Get primary bank account or first bank account
            var bankAccount = driver.BankAccounts?
                .FirstOrDefault(b => b.IsPrimary) ?? driver.BankAccounts?.FirstOrDefault();

            // Map IdentityType enum to string
            string? identityTypeString = null;
            if (driver.IdentityType.HasValue)
            {
                identityTypeString = driver.IdentityType.Value switch
                {
                    IdentityType.Swiss => "Swiss",
                    IdentityType.C => "C",
                    IdentityType.B => "B",
                    IdentityType.F => "F",
                    _ => null
                };
            }

            var dto = new DriverPersonalInformationDto
            {
                CompanyName = driver.CompanyName,
                CommercialRegistration = driver.CommercialRegistration,
                IdentityType = identityTypeString,
                Email = driver.Email.Value,
                PhoneNumber = driver.PhoneNumber.Value,
                Address = address?.Address,
                City = address?.City?.Name,
                Province = address?.Sector?.Name,
                ZipCode = address?.PostalCode,
                BankAccountIban = bankAccount?.IBAN
            };

            return dto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting driver personal information: {DriverId}", driverId);
            throw;
        }
    }

    public async Task<DriverDashboardDto?> GetDriverDashboardAsync(Guid driverId, CancellationToken cancellationToken = default)
    {
        try
        {
            // Check cache first
            var cacheKey = $"driver_dashboard:{driverId}";
            if (_cache.TryGetValue(cacheKey, out DriverDashboardDto? cachedDto))
            {
                _logger.LogDebug("Cache hit for driver dashboard: {DriverId}", driverId);
                return cachedDto;
            }

            // Verify driver exists
            var driverRepository = _unitOfWork.GetRepository<Driver>();
            var driver = await driverRepository.GetAll(d => d.Id == driverId && !d.IsDeleted)
                .FirstOrDefaultAsync(cancellationToken);

            if (driver == null)
            {
                _logger.LogWarning("Driver not found: {DriverId}", driverId);
                return null;
            }

            // Load trips with payment and earning includes
            var tripRepository = _unitOfWork.GetRepository<Trip>();
            var trips = await tripRepository.GetAll(t => t.DriverId == driverId)
                .Include(t => t.Payment)
                    .ThenInclude(p => p!.Earning)
                .ToListAsync(cancellationToken);

            // Load earnings with trip includes for efficient filtering
            var earningRepository = _unitOfWork.GetRepository<Earning>();
            var earnings = await earningRepository.GetAll(e => e.DriverId == driverId)
                .Include(e => e.Trip)
                .ToListAsync(cancellationToken);

            // Calculate trip counts
            var completedTripsCount = trips.Count(t => t.Status == TripStatus.Completed);
            var canceledTripsCount = trips.Count(t => t.Status == TripStatus.Cancelled);
            var scheduledTripsCount = trips.Count(t => t.Status == TripStatus.Scheduled);
            var activeTripsCount = trips.Count(t => t.Status == TripStatus.InProgress);

            // Calculate Driver Profit: Sum of Earning.Amount for completed trips
            var driverProfit = earnings
                .Where(e => e.Trip != null && e.Trip.Status == TripStatus.Completed)
                .Sum(e => e.Amount);

            // Calculate App Profit: Sum of (Payment.Amount - Earning.Amount) for completed trips
            var appProfit = trips
                .Where(t => t.Status == TripStatus.Completed 
                    && t.Payment != null 
                    && t.Payment.Earning != null)
                .Sum(t => t.Payment!.Amount - t.Payment.Earning!.Amount);

            // Calculate Driver Dues: Sum of Earning.Amount where IsPaid == false
            var driverDues = earnings
                .Where(e => !e.IsPaid)
                .Sum(e => e.Amount);

            // Calculate App Dues: Sum of (Payment.Amount - Earning.Amount) for completed trips 
            // where Payment.Status == Completed AND Earning.IsPaid == false
            var appDues = trips
                .Where(t => t.Status == TripStatus.Completed
                    && t.Payment != null
                    && t.Payment.Status == PaymentStatus.Completed
                    && t.Payment.Earning != null
                    && !t.Payment.Earning.IsPaid)
                .Sum(t => t.Payment!.Amount - t.Payment.Earning!.Amount);

            var dto = new DriverDashboardDto
            {
                CompletedTripsCount = completedTripsCount,
                CanceledTripsCount = canceledTripsCount,
                ScheduledTripsCount = scheduledTripsCount,
                ActiveTripsCount = activeTripsCount,
                DriverProfit = driverProfit,
                AppProfit = appProfit,
                DriverDues = driverDues,
                AppDues = appDues
            };

            // Cache the result
            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                SlidingExpiration = TimeSpan.FromMinutes(2)
            };
            _cache.Set(cacheKey, dto, cacheOptions);
            _logger.LogDebug("Cached driver dashboard for DriverId: {DriverId}", driverId);

            return dto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting driver dashboard: {DriverId}", driverId);
            throw;
        }
    }
}

