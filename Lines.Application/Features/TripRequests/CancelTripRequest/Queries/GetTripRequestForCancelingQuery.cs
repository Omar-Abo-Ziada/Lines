using Lines.Application.Features.TripRequests.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Lines.Application.Features.TripRequests.Queries;

public record GetTripRequestForCancelingQuery(Guid TripRequestId) : IRequest<TripRequestForCancelingDto>;
public class GetTripRequestForCancelingQueryHandler : RequestHandlerBase<GetTripRequestForCancelingQuery, TripRequestForCancelingDto>
{
    private readonly IRepository<TripRequest> _repository;
    public GetTripRequestForCancelingQueryHandler(RequestHandlerBaseParameters parameters, IRepository<TripRequest> repository) : base(parameters)
    {
        _repository = repository;
    }

    public override async Task<TripRequestForCancelingDto> Handle(GetTripRequestForCancelingQuery request, CancellationToken cancellationToken)
    {
        return await _repository
                    .Get(x => x.Id == request.TripRequestId)
                    .AsNoTracking()
                    .ProjectToType<TripRequestForCancelingDto>()
                    .FirstOrDefaultAsync(cancellationToken)
                    .ConfigureAwait(false);
    }
}