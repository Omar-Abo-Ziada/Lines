using Lines.Application.Common;
using Lines.Application.Features.Sectors.DTOs;
using Lines.Application.Interfaces;
using Lines.Domain.Models.Sites;
using Mapster;
using MediatR;

namespace Lines.Application.Features.Sectors.Command;

public record DeleteSectorCommand(Guid Id) : IRequest<bool>;

public class DeleteSectorCommandHandler(RequestHandlerBaseParameters parameters, IRepository<Sector> repository)
    : RequestHandlerBase<DeleteSectorCommand, bool>(parameters)
{
    private readonly IRepository<Sector> _repository = repository;

    public override async Task<bool> Handle(DeleteSectorCommand request, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(request.Id, cancellationToken);
        return true;
    }
}