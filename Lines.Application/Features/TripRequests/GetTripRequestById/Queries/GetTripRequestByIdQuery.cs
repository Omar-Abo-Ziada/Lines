using Microsoft.EntityFrameworkCore;

namespace Lines.Application.Features.TripRequests.GetTripRequestById.Queries
{
    public record GetTripRequestByIdQuery(Guid Id) : IRequest<TripRequest?>;

    public class GetTripRequestByIdQueryHandler(RequestHandlerBaseParameters parameters, IRepository<TripRequest> repository) :
        RequestHandlerBase<GetTripRequestByIdQuery, TripRequest?>(parameters)
    {
        private readonly IRepository<TripRequest> _repository = repository;

        public override async Task<TripRequest?> Handle(GetTripRequestByIdQuery request, CancellationToken cancellationToken)
        {
            var tripRequest = await _repository.Get(tr => tr.Id == request.Id)
                                               .SingleOrDefaultAsync(cancellationToken);

            return tripRequest;
        }
    }
}
