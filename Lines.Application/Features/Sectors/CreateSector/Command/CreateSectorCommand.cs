using Lines.Application.Common;
using Lines.Application.Features.Sectors.DTOs;
using Lines.Application.Interfaces;
using Lines.Domain.Models.Sites;
using Mapster;
using MediatR;

namespace Lines.Application.Features.Sectors.Command;

public record CreateSectorCommand(string Name, Guid CityId) : IRequest<CreateSectorDto>;

public class CreateSectorCommandHandler(RequestHandlerBaseParameters parameters, IRepository<Sector> repository)
    : RequestHandlerBase<CreateSectorCommand, CreateSectorDto>(parameters)
{
    private readonly IRepository<Sector> _repository = repository;

    public override async Task<CreateSectorDto> Handle(CreateSectorCommand request, CancellationToken cancellationToken)
    {
        var entity = new Sector(request.Name, request.CityId);
        var result = await _repository.AddAsync(entity, cancellationToken);
        return result.Adapt<CreateSectorDto>();
    }
}