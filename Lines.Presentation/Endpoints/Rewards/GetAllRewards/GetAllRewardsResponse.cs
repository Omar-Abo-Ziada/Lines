using Mapster;

namespace Lines.Presentation.Endpoints.Rewards.GetAllRewards
{
    public record GetAllRewardsResponse(Guid Id, string Title, string? Description, int PointsRequired, decimal DiscountPercentage, decimal MaxValue);

    public class GetAllRewardsResponseMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Domain.Models.User.Reward, GetAllRewardsResponse>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Title, src => src.Title)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.PointsRequired, src => src.PointsRequired)
                .Map(dest => dest.DiscountPercentage, src => src.DiscountPercentage)
                .Map(dest => dest.MaxValue, src => src.MaxValue);
        }
    }   



}
