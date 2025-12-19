using Lines.Application.Features.Notifications.GetNotifications.DTOs;
using Lines.Application.Features.Notifications.ReadNotifications.DTOs;
using Lines.Application.Features.Offers.GetOfferByRequestId.DTOs;
using Lines.Application.Features.TermsAndConditions.GetTermsAndConditions.DTOs;

namespace Lines.Presentation.Endpoints.Offers.GetOfferByRequestId;

public class GetOfferByRequestIdResponse
{
    public GetOfferByRequestIdResult Offers { get; set; } = new();
}
