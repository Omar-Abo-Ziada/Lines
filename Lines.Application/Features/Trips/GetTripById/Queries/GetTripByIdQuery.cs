namespace Lines.Application.Features.Trips.GetTripById.Queries
{
    public record GetTripByIdQuery(Guid TripId) : IRequest<Trip?>;

    public class GetTripByIdQueryHandler(RequestHandlerBaseParameters parameters, IRepository<Trip> repository)
        : RequestHandlerBase<GetTripByIdQuery, Trip?>(parameters)
    {
        public async override Task<Trip?> Handle(GetTripByIdQuery request, CancellationToken cancellationToken)
        {
           return await repository.GetByIdAsync(request.TripId, cancellationToken);
        }
    }
}
