using Lines.Domain.Models.Notifications;

namespace Lines.Application.Features.FCMNotifications.Queries
{
    public record GetFcmTokensByUserIdsQuery(List<Guid> UserIds)
    : IRequest<List<string>>;

    public class GetFcmTokensByUserIdsQueryHandler
     : RequestHandlerBase<GetFcmTokensByUserIdsQuery, List<string>>
    {
        private readonly IRepository<FCMUserToken> _repo;

        public GetFcmTokensByUserIdsQueryHandler(RequestHandlerBaseParameters p, IRepository<FCMUserToken> repo):base(p)
        {
            _repo = repo;
        }

        public override async Task<List<string>> Handle(GetFcmTokensByUserIdsQuery request, CancellationToken cancellationToken)
        {
            if (request.UserIds == null || !request.UserIds.Any())
                return new List<string>();

            var tokens = _repo.Get(t =>
                    request.UserIds.Contains(t.UserId) &&
                    t.IsActive)
                .Select(t => t.Token)
                .ToList();

            return tokens;
        }
    }
}
