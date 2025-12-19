using Mapster;

namespace Lines.Presentation.Endpoints.Feedbacks.CreateFeedback
{
    public record CreateFeedbackResponse(Guid ID);

    public class CreateFeedbackResponseMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Guid, CreateFeedbackResponse>()
                .Map(dest => dest.ID, src => src);
        }
    }



}
