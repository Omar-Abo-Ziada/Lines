using Lines.Application.Features.Drivers.GetDriverProfile.DTOs;
using Lines.Application.Features.Drivers.GetDriverProfile.Queries;

namespace Lines.Application.Features.Drivers.GetDriverProfile.Orchestrators;

public record GetDriverProfileOrchestrator(Guid userId) : IRequest<Result<DriverProfileDto>>;

public class GetDriverProfileOrchestratorHandler(RequestHandlerBaseParameters parameters)
    : RequestHandlerBase<GetDriverProfileOrchestrator, Result<DriverProfileDto>>(parameters)
{
    public async override Task<Result<DriverProfileDto>> Handle(
        GetDriverProfileOrchestrator request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetDriverProfileQuery(request.userId), cancellationToken)
            .ConfigureAwait(false);

        if (result is null)
        {
            return Error.NullValue;
        }

        return result;
    }
}
