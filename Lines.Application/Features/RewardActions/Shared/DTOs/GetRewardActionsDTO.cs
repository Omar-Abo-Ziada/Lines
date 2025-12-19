namespace Lines.Application.Features.RewardActions.Shared.DTOs
{
    public class GetRewardActionsDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Points { get; set; }
        public RewardActionType Type { get; set; }
    }


    public class GetRewardActionsDTOMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Domain.Models.User.RewardAction, GetRewardActionsDTO>()
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Points, src => src.Points)
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Type, src => src.Type);
        }
    }
}
