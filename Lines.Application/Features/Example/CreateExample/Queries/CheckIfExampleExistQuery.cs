using Lines.Application.Interfaces;
using MediatR;

namespace Lines.Application.Features.Examples;

public record CheckIfExampleExistQuery(string name) : IRequest<bool>;
public class CheckIfExampleExistQueryHandler : IRequestHandler<CheckIfExampleExistQuery, bool>
{
    private readonly IRepository<Domain.Models.Example> _repository;

    public CheckIfExampleExistQueryHandler(IRepository<Domain.Models.Example> repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }
    
    public async Task<bool> Handle(CheckIfExampleExistQuery request, CancellationToken cancellationToken)
    {
        return await _repository
            .AnyAsync(e => e.Name == request.name, cancellationToken)
            .ConfigureAwait(false);
    }
}