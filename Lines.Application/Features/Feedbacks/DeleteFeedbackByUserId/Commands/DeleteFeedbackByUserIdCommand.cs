namespace Lines.Application.Features.Feedbacks.DeleteFeedbackByUserId.Commands
{
    public record DeleteFeedbackByUserIdCommand(Guid UserId) : IRequest<bool>;

    public class DeleteFeedbackByUserIdCommandHandler(
         RequestHandlerBaseParameters parameters,
         IRepository<Lines.Domain.Models.Trips.Feedback> repository)
         : RequestHandlerBase<DeleteFeedbackByUserIdCommand, bool>(parameters)
    {
        private readonly IRepository<Lines.Domain.Models.Trips.Feedback> _repository = repository;

        public override async Task<bool> Handle(DeleteFeedbackByUserIdCommand request, CancellationToken cancellationToken)
        {
            var feedbacks = _repository.Get(x => x.FromUserId == request.UserId || x.ToUserId == request.UserId).ToList();

            foreach (var feedback in feedbacks)
            {
                feedback.IsDeleted = true; 
                await _repository.UpdateAsync(feedback, cancellationToken);
            }

            return true;
        }
    }
} 