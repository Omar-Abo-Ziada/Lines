namespace Lines.Application.Features.TermsAndConditions.GetTermsAndConditions.DTOs
{
    public class GetTermsAndConditionsDTO
    {
        public  TermsType TermsType { get; set; }
        public List<TermsAndConditionsDataDTO> Details { get; set; }
    }
}
