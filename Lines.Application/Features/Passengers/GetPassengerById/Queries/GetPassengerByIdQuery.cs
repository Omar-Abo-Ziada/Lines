using Lines.Domain.Models.Passengers;
using Microsoft.EntityFrameworkCore;

namespace Lines.Application.Features.Passengers.GetPassengerById.Queries
{
    public record GetPassengerByIdQuery(Guid Id) : IRequest<Passenger>;

    public class GetPassengerByIdQueryHandler(RequestHandlerBaseParameters parameters, IRepository<Passenger> repository)
       : RequestHandlerBase<GetPassengerByIdQuery, Passenger>(parameters)
    {
        public override async Task<Passenger> Handle(GetPassengerByIdQuery request, CancellationToken cancellationToken)
        {
            var passenger = await repository
                .Get(x => x.Id == request.Id)
               // .ProjectToType<GetPassengersDto>()
                .FirstOrDefaultAsync(cancellationToken);

            if (passenger is null)
                throw new KeyNotFoundException($"Passenger with ID '{request.Id}' was not found.");

            return passenger!;
        }
    }
}
