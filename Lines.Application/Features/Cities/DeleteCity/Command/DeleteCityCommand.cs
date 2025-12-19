using Lines.Application.Common;
using Lines.Application.Interfaces;
using Lines.Domain.Models.Sites;
using MediatR;

namespace Lines.Application.Features.Cities.DeleteCity.Command;

public record DeleteCityCommand(Guid Id) : IRequest<Unit>;  
public class DeleteCityCommandHandler(RequestHandlerBaseParameters parameters, IRepository<City> repository)
    : RequestHandlerBase<DeleteCityCommand, Unit>(parameters)
{
    private readonly IRepository<City> _repository = repository;

    public override async Task<Unit> Handle(DeleteCityCommand request, CancellationToken cancellationToken)
    {
        await _repository
            .DeleteAsync(request.Id, cancellationToken)
            .ConfigureAwait(false);
        return Unit.Value;
    }
}