using Lines.Application.Features.PaymentMethods.GetAllCreditCardsByUserId.DTOs;

namespace Lines.Presentation.Endpoints.PaymentMethods.GetAllCreditCardsByUserId
{
    public record GetAllCreditCardsByUserIdResponse(string Id , string Brand, string Last4, long ExpMonth, long ExpYear, bool IsDefault);


    public class GetAllCreditCardsByUserIdResponseMappingConfig : Mapster.IRegister
    {
        public void Register(Mapster.TypeAdapterConfig config)
        {
            config.NewConfig<PaymentGatewayCreditCardDto, GetAllCreditCardsByUserIdResponse>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Brand, src => src.Brand)
                .Map(dest => dest.Last4, src => src.Last4)
                .Map(dest => dest.ExpMonth, src => src.ExpMonth)
                .Map(dest => dest.ExpYear, src => src.ExpYear)
                .Map(dest => dest.IsDefault, src => src.IsDefault);
        }
    }
}
