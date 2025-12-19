using Lines.Application.Features.Drivers.GetDriverContact.DTOs;
using Lines.Application.Features.Drivers.GetDriverContact.Queries;
using Lines.Domain.Shared;

namespace Lines.Application.Features.Drivers.GetDriverContact.Orchestrators;

public record GetDriverContactOrchestrator(Guid userId) : IRequest<Result<DriverContactDto>>;

public class GetDriverContactOrchestratorHandler(RequestHandlerBaseParameters parameters)
    : RequestHandlerBase<GetDriverContactOrchestrator, Result<DriverContactDto>>(parameters)
{
    public async override Task<Result<DriverContactDto>> Handle(
        GetDriverContactOrchestrator request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetDriverContactQuery(request.userId), cancellationToken)
            .ConfigureAwait(false);

        if (result is null)
        {
            return Error.NullValue;
        }

        return result;
    }
}
