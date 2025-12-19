using Application.Common.Helpers;
using Lines.Application.Common;
using Lines.Application.Features.Sectors.DTOs;
using Lines.Application.Shared;
using Lines.Application.Interfaces;
using Lines.Domain.Models.Sites;
using LinqKit;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lines.Application.Features.Sectors.Command;

public record GetAllSectorsQuery(string? Name, Guid? CityId, int PageSize, int PageNumber) : IRequest<PagingDto<GetAllSectorsDto>>;

public class GetAllSectorsQueryHandler(RequestHandlerBaseParameters parameters, IRepository<Sector> repository)
    : RequestHandlerBase<GetAllSectorsQuery, PagingDto<GetAllSectorsDto>>(parameters)
{
    private readonly IRepository<Sector> _repository = repository;

    public override async Task<PagingDto<GetAllSectorsDto>> Handle(GetAllSectorsQuery request, CancellationToken cancellationToken)
    {
        var predicate = PredicateBuilder.New<Sector>(true);
        
        if (!string.IsNullOrEmpty(request.Name))
        {
            predicate = predicate.And(p => p.Name.Contains(request.Name));
        }

        if (request.CityId.HasValue &&  request.CityId != Guid.Empty)
        {
            predicate = predicate.And(p => p.CityId == request.CityId);
        }

        var query = _repository
            .Get(predicate)
            .ProjectToType<GetAllSectorsDto>();
        
        var result = await PagingHelper.CreateAsync(query, request.PageSize, request.PageNumber, cancellationToken);
        return result;
    }
}