using Lines.Application.Features.Drivers.GetDriverAddress.DTOs;
using Lines.Application.Features.Drivers.GetDriverAddress.Queries;
using Lines.Domain.Shared;
using MediatR;

namespace Lines.Application.Features.Drivers.GetDriverAddress.Orchestrators;

public record GetDriverAddressOrchestrator(Guid userId) : IRequest<Result<DriverAddressDto>>;

public class GetDriverAddressOrchestratorHandler(RequestHandlerBaseParameters parameters)
    : RequestHandlerBase<GetDriverAddressOrchestrator, Result<DriverAddressDto>>(parameters)
{
    public async override Task<Result<DriverAddressDto>> Handle(GetDriverAddressOrchestrator request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetDriverAddressQuery(request.userId), cancellationToken)
                                    .ConfigureAwait(false);

        if (result is null)
        {
            return Error.NullValue;
        }

        return result;
    }
}
