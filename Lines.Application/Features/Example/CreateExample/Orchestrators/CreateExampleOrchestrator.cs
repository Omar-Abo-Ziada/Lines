using Lines.Application.Extensions;
using Lines.Domain.Shared;
using MediatR;

namespace Lines.Application.Features.Examples;

public record CreateExampleOrchestrator(string name, string desc) : IRequest<Result<CreateExampleResponse>>;

public class CreateExampleOrchestratorHandler : IRequestHandler<CreateExampleOrchestrator, Result<CreateExampleResponse>>
{
    private readonly IMediator _mediator;

    public CreateExampleOrchestratorHandler(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }
    public async Task<Result<CreateExampleResponse>> Handle(CreateExampleOrchestrator request, CancellationToken cancellationToken)
    {
        var validationResult = await Validate(request);
        if (!validationResult.IsSuccess)
            return validationResult;
        
        var result = await _mediator.Send(new CreateExampleCommand(request.name,request.desc), cancellationToken).ConfigureAwait(false);
        
        return Result<CreateExampleResponse>.Success(result.ToResponse());
    }

    private async Task<Result<CreateExampleResponse>> Validate(CreateExampleOrchestrator request)
    {
        var isFound = await _mediator.Send(new CheckIfExampleExistQuery(request.name)).ConfigureAwait(false);
        if (isFound)
        {
            return Result<CreateExampleResponse>.Failure(ExampleErrors.ExampleExist);
        }
        return Result<CreateExampleResponse>.Success(null!);
    }
}