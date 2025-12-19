using Lines.Application.Features.TripRequests.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Lines.Application.Features.TripRequests.Queries;

public record GetTripRequestForAcceptQuery(Guid TripRequestId) :  IRequest<GetTripRequestForAcceptDto>;

public class GetTripRequestForAcceptQueryHandler(
    RequestHandlerBaseParameters parameters,
    IRepository<TripRequest> repository)
    : RequestHandlerBase<GetTripRequestForAcceptQuery, GetTripRequestForAcceptDto>(parameters)
{
    private readonly IRepository<TripRequest> _repository = repository;

    public override async Task<GetTripRequestForAcceptDto> Handle(GetTripRequestForAcceptQuery request, CancellationToken cancellationToken)
    {
        return await _repository
                .Get(x => x.Id == request.TripRequestId)
                .AsNoTracking()
                .ProjectToType<GetTripRequestForAcceptDto>()
                .FirstOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);
        
    }
}