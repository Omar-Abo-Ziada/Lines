using Lines.Application.Features.FavoriteLocations.GetFavoriteLocationById.DTOs;
using Lines.Domain.Models.Sites;
using Microsoft.EntityFrameworkCore;

namespace Lines.Application.Features.FavoriteLocations.GetFavoriteLocationById.Queries;

public record GetFavoriteLocationByIdQuery(Guid Id) : IRequest<Result<GetFavoriteLocationByIdDto>>;
public class GetFavoriteLocationByIdQueryHandler : RequestHandlerBase<GetFavoriteLocationByIdQuery, Result<GetFavoriteLocationByIdDto>>
{
    private readonly IRepository<FavoriteLocation> _repository;
    public GetFavoriteLocationByIdQueryHandler(RequestHandlerBaseParameters parameters, IRepository<FavoriteLocation> repository) : base(parameters)
    {
        _repository = repository;
    }

    public override async Task<Result<GetFavoriteLocationByIdDto>> Handle(GetFavoriteLocationByIdQuery request, CancellationToken cancellationToken)
    {
        var res = await _repository
            .Get(x => x.Id == request.Id)
            .ProjectToType<GetFavoriteLocationByIdDto>()
            .FirstOrDefaultAsync(cancellationToken: cancellationToken)
            .ConfigureAwait(false);
            
        return Result<GetFavoriteLocationByIdDto>.Success(res);
    }
}