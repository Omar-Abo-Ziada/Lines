using AutoMapper;

namespace Lines.Application.Common;

public abstract class RequestHandlerBase<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
{

    protected readonly IMapper _mapper;
    protected readonly IMediator _mediator;

    public RequestHandlerBase(RequestHandlerBaseParameters parameters)
    {
        _mediator = parameters.Mediator;
        _mapper = parameters.Mapper;
    }

    public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}