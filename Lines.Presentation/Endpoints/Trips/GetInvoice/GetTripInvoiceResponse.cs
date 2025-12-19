using Lines.Application.Features.Trips.GetTripInvoice.DTOs;
using Mapster;

namespace Lines.Presentation.Endpoints.Trips.GetInvoice;

public record GetTripInvoiceResponse(
    Guid TripId,
    string InvoiceNumber,
    DateTime InvoiceDate,
    string DriverName,
    string DriverPhoneNumber,
    string PassengerName,
    string PassengerPhoneNumber,
    DateTime? PickUpTime,
    DateTime? DropOffTime,
    string StartLocationAddress,
    string EndLocationAddress,
    double? Distance,
    decimal Fare,
    string Currency,
    string PaymentMethod,
    string PaymentStatus
);

public class GetTripInvoiceResponseMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<TripInvoiceDto, GetTripInvoiceResponse>()
            .Map(dest => dest.TripId, src => src.TripId)
            .Map(dest => dest.InvoiceNumber, src => src.InvoiceNumber)
            .Map(dest => dest.InvoiceDate, src => src.InvoiceDate)
            .Map(dest => dest.DriverName, src => src.DriverName)
            .Map(dest => dest.DriverPhoneNumber, src => src.DriverPhoneNumber)
            .Map(dest => dest.PassengerName, src => src.PassengerName)
            .Map(dest => dest.PassengerPhoneNumber, src => src.PassengerPhoneNumber)
            .Map(dest => dest.PickUpTime, src => src.PickUpTime)
            .Map(dest => dest.DropOffTime, src => src.DropOffTime)
            .Map(dest => dest.StartLocationAddress, src => src.StartLocationAddress)
            .Map(dest => dest.EndLocationAddress, src => src.EndLocationAddress)
            .Map(dest => dest.Distance, src => src.Distance)
            .Map(dest => dest.Fare, src => src.Fare)
            .Map(dest => dest.Currency, src => src.Currency)
            .Map(dest => dest.PaymentMethod, src => src.PaymentMethod)
            .Map(dest => dest.PaymentStatus, src => src.PaymentStatus);
    }
}
