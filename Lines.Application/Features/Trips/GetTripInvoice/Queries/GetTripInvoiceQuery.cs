using Lines.Application.Common;
using Lines.Application.Features.Trips.GetTripInvoice.DTOs;
using Lines.Application.Interfaces;
using Lines.Domain.Models.Trips;
using Microsoft.EntityFrameworkCore;

namespace Lines.Application.Features.Trips.GetTripInvoice.Queries;

public record GetTripInvoiceQuery(Guid TripId) : IRequest<TripInvoiceDto?>;

public class GetTripInvoiceQueryHandler(RequestHandlerBaseParameters parameters, IRepository<Trip> repository)
    : RequestHandlerBase<GetTripInvoiceQuery, TripInvoiceDto?>(parameters)
{
    public async override Task<TripInvoiceDto?> Handle(GetTripInvoiceQuery request, CancellationToken cancellationToken)
    {
        var trip = await repository.Get()
            .Include(t => t.Driver)
            .Include(t => t.Passenger)
            .Include(t => t.Payment)
            .Include(t => t.StartLocation)
            .Include(t => t.EndLocations)
            .FirstOrDefaultAsync(t => t.Id == request.TripId, cancellationToken);

        if (trip == null)
        {
            return null;
        }

        // Generate invoice number
        var invoiceNumber = $"INV-{DateTime.UtcNow:yyyy}-{trip.Id.ToString().Substring(0, 8).ToUpper()}";

        // Get start and end location addresses (simplified for now)
        var startLocationAddress = trip.StartLocation != null 
            ? $"{trip.StartLocation.Latitude:F6}, {trip.StartLocation.Longitude:F6}" 
            : "Unknown";
        
        var endLocationAddress = trip.EndLocations?.FirstOrDefault() != null 
            ? $"{trip.EndLocations.First().Location.Latitude:F6}, {trip.EndLocations.First().Location.Longitude:F6}" 
            : "Unknown";

        return new TripInvoiceDto
        {
            TripId = trip.Id,
            InvoiceNumber = invoiceNumber,
            InvoiceDate = DateTime.UtcNow,
            DriverName = $"{trip.Driver.FirstName} {trip.Driver.LastName}",
            DriverPhoneNumber = trip.Driver.PhoneNumber.Value,
            PassengerName = $"{trip.Passenger.FirstName} {trip.Passenger.LastName}",
            PassengerPhoneNumber = trip.Passenger.PhoneNumber.Value,
            PickUpTime = trip.StartedAt,
            DropOffTime = trip.EndedAt,
            StartLocationAddress = startLocationAddress,
            EndLocationAddress = endLocationAddress,
            Distance = trip.Distance,
            Fare = trip.Fare,
            Currency = trip.Currency,
            PaymentMethod = trip.PaymentMethodType?.ToString() ?? "Unknown",
            PaymentStatus = trip.Payment?.Status.ToString() ?? "NotPaidYet"
        };
    }
}
