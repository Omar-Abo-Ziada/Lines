using Lines.Application.Common;
using Lines.Application.Interfaces;
using Lines.Domain.Models.Sites;
using MediatR;

namespace Lines.Application.Features.Common.Queries;

public record CheckIfCityExistQuery(Guid Id) : IRequest<bool>;
public class CheckIfCityExistQueryHandler(RequestHandlerBaseParameters parameters, IRepository<City> repository)
    : RequestHandlerBase<CheckIfCityExistQuery, bool>(parameters)
{
    private readonly IRepository<City> _repository = repository;

    public override async Task<bool> Handle(CheckIfCityExistQuery request, CancellationToken cancellationToken)
    {
        return await _repository
                .AnyAsync(c => c.Id == request.Id, cancellationToken)
                .ConfigureAwait(false);
    }
}