using AutoMapper;
using Lines.Application.Interfaces;
using MediatR;

namespace Lines.Application.Features.Examples;

public record CreateExampleCommand(string name , string description) : IRequest<CreateExampleDto>;
public class CreateExampleCommandHandler : IRequestHandler<CreateExampleCommand, CreateExampleDto>
{
    private readonly IRepository<Domain.Models.Example> _repository;
    private readonly IMapper _mapper;

    public CreateExampleCommandHandler(IRepository<Domain.Models.Example> repository, IMapper mapper)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper;
    }
    public async Task<CreateExampleDto> Handle(CreateExampleCommand request, CancellationToken cancellationToken)
    {
        var entity = new Domain.Models.Example();
        entity.Name = request.name;
        entity.Description = request.description;
        var result = await _repository.AddAsync(entity, cancellationToken);
        return _mapper.Map<CreateExampleDto>(result);

    }
}