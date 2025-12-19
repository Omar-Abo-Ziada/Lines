namespace Lines.Application.Features.Trips.CompleteTrip.Commands
{
    public record CompleteTripCommand(Trip trip) : IRequest<bool>;

    public class CompleteTripCommandHandler(RequestHandlerBaseParameters parameters, 
                                            IRepository<Trip> repository) 
                                            : RequestHandlerBase<CompleteTripCommand, bool>(parameters)
    {
        private readonly IRepository<Trip> _repository = repository;

        public override async Task<bool> Handle(CompleteTripCommand request, CancellationToken cancellationToken)
        {
            request.trip.CompleteTrip();
            await _repository.UpdateAsync(request.trip, cancellationToken);
            _repository.SaveChanges();

            return true;
        }
    }


}
