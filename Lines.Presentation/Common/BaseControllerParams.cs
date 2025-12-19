using AutoMapper;
using FluentValidation;
using MediatR;

namespace Lines.Presentation.Common;

public class BaseControllerParams<TRequest>
{
    public IMediator Mediator { get; }
    public IValidator<TRequest>? Validator { get; }
    public IMapper Mapper { get; }

    public BaseControllerParams(
        IMediator mediator,
        IMapper mapper,
        IServiceProvider serviceProvider)
    {
        Mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        Validator = serviceProvider.GetService<IValidator<TRequest>>();  
    }
}