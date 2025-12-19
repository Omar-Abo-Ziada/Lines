using Lines.Application.Features.TermsAndConditions.GetTermsAndConditions.DTOs;

namespace Lines.Presentation.Endpoints.TermsAndConditions.GetTermsandConditions;

public class GetTermsAndConditionsResponse
{
    public List<GetTermsAndConditionsDTO> TermsAndConditions { get; set; } = new();
}
