using Lines.Application.Features.Drivers.UpdateDriverAddress.DTOs;
using Lines.Application.Interfaces;
using Lines.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lines.Application.Features.Drivers.UpdateDriverAddress.Commands;

public record UpdateDriverAddressCommand(Guid driverId, string address, Guid cityId, Guid? sectorId, string postalCode) : IRequest<Result<bool>>;

public class UpdateDriverAddressCommandHandler(RequestHandlerBaseParameters parameters, IRepository<Domain.Models.Drivers.Driver> repository)
    : RequestHandlerBase<UpdateDriverAddressCommand, Result<bool>>(parameters)
{
    public async override Task<Result<bool>> Handle(UpdateDriverAddressCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var driver = await repository.Get()
                .Include(d => d.Addresses)
                .FirstOrDefaultAsync(d => d.Id == request.driverId, cancellationToken);

            if (driver == null)
            {
                return Result<bool>.Failure(new Application.Shared.Error("400", "Driver not found.", Application.Shared.ErrorType.NotFound));
            }

            // Get primary address or create new if doesn't exist
            var primaryAddress = driver.Addresses.FirstOrDefault(a => a.IsPrimary);
            
            if (primaryAddress == null)
            {
                // Create new primary address
                primaryAddress = new Domain.Models.Drivers.DriverAddress(
                    request.driverId,
                    request.cityId,
                    request.address,
                    request.postalCode,
                    request.sectorId,
                    null,
                    true
                );
                driver.Addresses.Add(primaryAddress);
            }
            else
            {
                // Update existing address using domain method
                primaryAddress.UpdateAddress(request.address, request.postalCode, request.sectorId);
                primaryAddress.CityId = request.cityId;
            }

            await repository.UpdateAsync(driver);
            repository.SaveChanges();

            return Result<bool>.Success(true);
        }
        catch (ArgumentException ex)
        {
            return Result<bool>.Failure(new Application.Shared.Error("400", ex.Message, Application.Shared.ErrorType.Validation));
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure(new Application.Shared.Error("500", $"An error occurred while updating driver address: {ex.Message}", Application.Shared.ErrorType.Failure));
        }
    }
}
