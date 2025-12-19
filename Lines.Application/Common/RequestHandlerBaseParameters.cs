using AutoMapper;
using MediatR;

namespace Lines.Application.Common;

public class RequestHandlerBaseParameters
{
    public IMediator Mediator => _mediator;
    public IMapper Mapper => _mapper;
    
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public RequestHandlerBaseParameters(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }
}