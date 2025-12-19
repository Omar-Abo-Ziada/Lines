using Lines.Application.Features.Drivers.GetDriverById.Queries;
using Lines.Domain.Models.Drivers;

namespace Lines.Application.Features.Drivers.GetDriverById.Orchestrators
{
    public record GetDriverByIdOrchestrator(Guid driverId) : IRequest<Result<Driver>>;

    public class GetDriverByIdOrchestratorHandler(RequestHandlerBaseParameters parameters)
        : RequestHandlerBase<GetDriverByIdOrchestrator, Result<Driver>>(parameters)
    {
        public async override Task<Result<Driver>> Handle(
                       GetDriverByIdOrchestrator request,
                                  CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetDriverByIdQuery(request.driverId),cancellationToken)
                                                      .ConfigureAwait(false);

            if (result is null)
            {
                return Error.NullValue;
            }

            return result;
        }
    }
}
