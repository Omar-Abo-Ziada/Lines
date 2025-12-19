using Lines.Application.Common;
using Lines.Application.Interfaces;
using Lines.Domain.Models.Sites;
using Lines.Domain.Shared;
using MediatR;

namespace Lines.Application.Features.FavoriteLocations;

public record UpdateFavoriteLocationCommand(Guid Id, double Latitude, double Longtude, string Name, Guid PassengerId, Guid CityId, Guid SectorId) :  IRequest<Result<bool>>;
public class UpdateFavoriteLocationCommandHandler : RequestHandlerBase<UpdateFavoriteLocationCommand, Result<bool>>
{
    private readonly IRepository<FavoriteLocation> _repository;
    public UpdateFavoriteLocationCommandHandler(RequestHandlerBaseParameters parameters, IRepository<FavoriteLocation> repository) : base(parameters)
    {
        _repository = repository;
    }

    public override async Task<Result<bool>> Handle(UpdateFavoriteLocationCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
        entity.Update(request.Name, request.Latitude, request.Longtude, request.PassengerId, request.CityId, request.SectorId);
        await _repository.UpdateAsync(entity, cancellationToken);
        return Result<bool>.Success(true);
    }
}