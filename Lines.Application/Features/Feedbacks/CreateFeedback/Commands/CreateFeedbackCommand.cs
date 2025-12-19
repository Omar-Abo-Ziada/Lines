
namespace Lines.Application.Features.Feedbacks.CreateFeedback.Commands
{
    public record CreateFeedbackCommand(
        Guid TripId,
        Guid FromUserId,
        Guid ToUserId,
        int Rating,
        string? Comment
    ) : IRequest<Guid>; 

    
    public class CreateFeedbackCommandHandler(
        RequestHandlerBaseParameters parameters,
        IRepository<Domain.Models.Trips.Feedback> repository
    ) : RequestHandlerBase<CreateFeedbackCommand, Guid>(parameters)
    {
        private readonly IRepository<Domain.Models.Trips.Feedback> _repository = repository;

        public override async Task<Guid> Handle(CreateFeedbackCommand request, CancellationToken cancellationToken)
        {
            var feedback = new Domain.Models.Trips.Feedback(
                request.TripId,
                request.FromUserId,
                request.ToUserId,
                request.Rating,
                request.Comment 
            );
                await _repository.AddAsync(feedback, cancellationToken);
                _repository.SaveChanges();

                return feedback.Id;
        }
    }
}
