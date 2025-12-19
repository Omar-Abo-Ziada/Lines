using Lines.Application.Common;
using Lines.Application.Features.Sectors.DTOs;
using Lines.Application.Interfaces;
using Lines.Domain.Models.Sites;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lines.Application.Features.Sectors.Command;

public record GetSectorByIdQuery(Guid Id) : IRequest<GetSectorByIdDto>;

public class GetSectorByIdQueryHandler(RequestHandlerBaseParameters parameters, IRepository<Sector> repository)
    : RequestHandlerBase<GetSectorByIdQuery, GetSectorByIdDto>(parameters)
{
    private readonly IRepository<Sector> _repository = repository;

    public override async Task<GetSectorByIdDto> Handle(GetSectorByIdQuery request, CancellationToken cancellationToken)
    {
        return await _repository
                .Get(s => s.Id == request.Id)
                .ProjectToType<GetSectorByIdDto>()
                .FirstOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);
    }
}