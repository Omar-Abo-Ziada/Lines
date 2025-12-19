using Lines.Application.Common;
using Lines.Application.Interfaces;
using Lines.Domain.Models.Passengers;
using MediatR;

namespace Lines.Application.Features.Passengers.DeletePassenger.Commands
{
    public record DeletePassengerCommand(Guid Id) : IRequest<Unit>;

    public class DeletePassengerCommandHandler(
        RequestHandlerBaseParameters parameters,
        IRepository<Passenger> repository
    ) : RequestHandlerBase<DeletePassengerCommand, Unit>(parameters)
    {
        private readonly IRepository<Passenger> _repository = repository;

        public override async Task<Unit> Handle(DeletePassengerCommand request, CancellationToken cancellationToken)
        {
            await _repository
                .DeleteAsync(request.Id, cancellationToken)
                .ConfigureAwait(false);
            return Unit.Value;
        }
    }
}
