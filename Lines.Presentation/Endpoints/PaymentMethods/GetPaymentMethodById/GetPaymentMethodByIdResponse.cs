using Mapster;

namespace Lines.Presentation.Endpoints.PaymentMethods.GetPaymentMethodById;

public record GetPaymentMethodByIdResponse(Guid Id, int Type, bool IsDefault, string PaymentGatewayPaymentMethodId, string CustomerId, Guid UserId);

public class GetPaymentMethodByIdResponseMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Domain.Models.PaymentMethods.PaymentMethod, GetPaymentMethodByIdResponse>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Type, src => src.Type)
            .Map(dest => dest.IsDefault, src => src.IsDefault)
            .Map(dest => dest.PaymentGatewayPaymentMethodId, src => src.PaymentGatewayPaymentMethodId)
            .Map(dest => dest.CustomerId, src => src.CustomerId)
            .Map(dest => dest.UserId, src => src.UserId);
    }
}