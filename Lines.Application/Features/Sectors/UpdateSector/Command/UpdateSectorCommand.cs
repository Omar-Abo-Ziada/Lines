using Lines.Application.Common;
using Lines.Application.Features.Sectors.DTOs;
using Lines.Application.Interfaces;
using Lines.Domain.Models.Sites;
using Mapster;
using MediatR;

namespace Lines.Application.Features.Sectors.Command;

public record UpdateSectorCommand(Guid Id, string Name, Guid CityId) : IRequest<bool>;

public class UpdateSectorCommandHandler(RequestHandlerBaseParameters parameters, IRepository<Sector> repository)
    : RequestHandlerBase<UpdateSectorCommand, bool>(parameters)
{
    private readonly IRepository<Sector> _repository = repository;

    public override async Task<bool> Handle(UpdateSectorCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        
        entity.UpdateName(request.Name);
        entity.UpdateCity(request.CityId);
        
        await _repository.UpdateAsync(entity, cancellationToken);
        
        return true;
    }
}