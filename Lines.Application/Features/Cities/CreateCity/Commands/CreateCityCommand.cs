using Lines.Application.Common;
using Lines.Application.Features.Cities.DTOs;
using Lines.Application.Interfaces;
using Lines.Domain.Models.Sites;
using Mapster;
using MediatR;

namespace Lines.Application.Features.Cities.Commands;

public record CreateCityCommand(string Name, double Latitude, double Longitude) : IRequest<CreateCityDto>;
public class CreateCityCommandHandler(RequestHandlerBaseParameters parameters, IRepository<City> repository)
    : RequestHandlerBase<CreateCityCommand, CreateCityDto>(parameters)
{
    private readonly IRepository<City> _repository = repository;

    public override async Task<CreateCityDto> Handle(CreateCityCommand request, CancellationToken cancellationToken)
    {
        var entity = new City(request.Name, request.Latitude, request.Longitude);
        var res = await _repository
            .AddAsync(entity)
            .ConfigureAwait(false);
        
        return res.Adapt<CreateCityDto>();
    }
}