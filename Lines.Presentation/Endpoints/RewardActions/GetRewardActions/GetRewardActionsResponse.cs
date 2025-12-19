using Lines.Application.Features.RewardActions.Shared.DTOs;

namespace Lines.Presentation.Endpoints.RewardActions.GetRewardActions
{
    public record GetRewardActionsResponse(string Name, int Points);

    public class GetRewardActionsResponseMappingConfig : Mapster.IRegister
    {
        public void Register(Mapster.TypeAdapterConfig config)
        {
            config.NewConfig<GetRewardActionsDTO, GetRewardActionsResponse>()
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Points, src => src.Points);
        }
    }
}
