using Lines.Application.Common;
using Lines.Application.Interfaces;
using Lines.Domain.Models.Sites;
using Lines.Domain.Shared;
using MediatR;

namespace Lines.Application.Features.FavoriteLocations;

public record CreateFavoriteLocationCommand(double Latitude, double Longtude, string Name, Guid PassengerId, Guid CityId, Guid SectorId) :  IRequest<Result<bool>>;
public class CreateFavoriteLocationCommandHandler : RequestHandlerBase<CreateFavoriteLocationCommand, Result<bool>>
{
    private readonly IRepository<FavoriteLocation> _repository;
    public CreateFavoriteLocationCommandHandler(RequestHandlerBaseParameters parameters, IRepository<FavoriteLocation> repository) : base(parameters)
    {
        _repository = repository;
    }

    public override async Task<Result<bool>> Handle(CreateFavoriteLocationCommand request, CancellationToken cancellationToken)
    {
        var entity = new FavoriteLocation(request.Name, request.Latitude, request.Longtude, request.PassengerId, request.CityId, request.SectorId);
        await _repository.AddAsync(entity, cancellationToken);
        return Result<bool>.Success(true);
    }
}