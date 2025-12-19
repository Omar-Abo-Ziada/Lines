using Lines.Application.Common;
using Lines.Application.Interfaces;
using Lines.Domain.Models.Sites;
using MediatR;

namespace Lines.Application.Features.Cities.UpdateCity;

public record UpdateCityCommand(Guid Id, string Name) : IRequest<bool>;
public class UpdateCityCommandHandler(RequestHandlerBaseParameters parameters, IRepository<City> repository)
    : RequestHandlerBase<UpdateCityCommand, bool>(parameters)
{
    private readonly IRepository<City> _repository = repository;

    public override async Task<bool> Handle(UpdateCityCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
        entity.Name = request.Name;
        await _repository.UpdateAsync(entity, cancellationToken);
        return true;
    }
}