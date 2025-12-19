using Lines.Application.Interfaces;
using Lines.Domain.Models.Passengers;
using MediatR;

namespace Lines.Application.Features.Passengers.DeletePassenger.Queries
{
    public record CheckIfPassengerExistQuery(Guid Id) : IRequest<bool>;

    public class CheckIfPassengerExistQueryHandler : IRequestHandler<CheckIfPassengerExistQuery, bool>
    {
        private readonly IRepository<Passenger> _repository;
        public CheckIfPassengerExistQueryHandler(IRepository<Passenger> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(CheckIfPassengerExistQuery request, CancellationToken cancellationToken)
        {
            return await _repository.AnyAsync(p => p.Id == request.Id, cancellationToken);
        }
    }
} 