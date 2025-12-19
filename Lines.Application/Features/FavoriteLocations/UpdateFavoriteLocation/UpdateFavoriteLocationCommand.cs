using Lines.Application.Common;
using Lines.Application.Interfaces;
using Lines.Domain.Models.Sites;
using Lines.Domain.Shared;
using MediatR;

namespace Lines.Application.Features.FavoriteLocations;

public record DeleteFavoriteLocationCommand(Guid Id) :  IRequest<Result<bool>>;
public class DeleteFavoriteLocationCommandHandler : RequestHandlerBase<DeleteFavoriteLocationCommand, Result<bool>>
{
    private readonly IRepository<FavoriteLocation> _repository;
    public DeleteFavoriteLocationCommandHandler(RequestHandlerBaseParameters parameters, IRepository<FavoriteLocation> repository) : base(parameters)
    {
        _repository = repository;
    }

    public override async Task<Result<bool>> Handle(DeleteFavoriteLocationCommand request, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(request.Id, cancellationToken);
        return Result<bool>.Success(true);
    }
}