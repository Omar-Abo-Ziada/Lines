using Lines.Application.Features.DriverStatistics.DTOs;
using Mapster;

namespace Lines.Presentation.Endpoints.DriverStatistics.GetDriverOffers;

public record GetDriverOffersResponse(decimal ServiceFee, DateTime? ValidFrom, DateTime ValidUntil);

public class GetDriverOffersResponseMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<DriverOfferDto, GetDriverOffersResponse>()
            .Map(dest => dest.ServiceFee, src => src.ServiceFee)
            .Map(dest => dest.ValidFrom, src => src.ValidFrom)
            .Map(dest => dest.ValidUntil, src => src.ValidUntil);
    }
}


