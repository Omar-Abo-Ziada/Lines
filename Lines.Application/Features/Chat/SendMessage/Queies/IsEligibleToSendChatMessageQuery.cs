using LinqKit;

namespace Lines.Application.Features.Chat.SendMessage.Queies
{
    public record IsEligibleToSendChatMessageQuery(Guid SenderID, Guid TripID, string Content) : IRequest<bool>;

    public class IsEligibleToSendChatMessageQueryHandler : RequestHandlerBase<IsEligibleToSendChatMessageQuery, bool>
    {
        private readonly IRepository<Trip> _repository;
        public IsEligibleToSendChatMessageQueryHandler(RequestHandlerBaseParameters parameters, IRepository<Trip> repository) : base(parameters)
        {
            _repository = repository;
        }

        public override async Task<bool> Handle(IsEligibleToSendChatMessageQuery request, CancellationToken cancellationToken)
        {
            var predicate = PredicateBuilder.New<Trip>(true);

            predicate = predicate.And(t => t.Id == request.TripID)
                .And(t => t.DriverId == request.SenderID || t.PassengerId == request.SenderID)
                .And(t => t.PassengerId != t.DriverId)
                .And(t => t.Status == Domain.Enums.TripStatus.InProgress);

                return _repository.Get(predicate)
                                  .Any();
        }
    }
}
