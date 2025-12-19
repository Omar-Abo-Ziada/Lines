namespace Lines.Application.Features.Rewards.Shared.DTOs
{
    public class GetRewardDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int PointsRequired { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal MaxValue { get; set; }
    }


    public class GetRewardDTOMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Domain.Models.User.Reward, GetRewardDTO>()
                .Map(dest => dest.Title, src => src.Title)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.PointsRequired, src => src.PointsRequired)
                .Map(dest => dest.DiscountPercentage, src => src.DiscountPercentage)
                .Map(dest => dest.MaxValue, src => src.MaxValue)
                .Map(dest => dest.Id, src => src.Id);
        }
    }
}
