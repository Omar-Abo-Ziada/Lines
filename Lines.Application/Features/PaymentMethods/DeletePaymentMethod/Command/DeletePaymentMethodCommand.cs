using Lines.Application.Common;
using Lines.Application.Interfaces;
using Lines.Domain.Models.PaymentMethods;
using MediatR;

namespace Lines.Application.Features.PaymentMethods.Command;

public record DeletePaymentMethodCommand(Guid Id) :  IRequest<bool>;
public class DeletePaymentMethodCommandHandler(
    RequestHandlerBaseParameters parameters,
    IRepository<PaymentMethod> repository)
    : RequestHandlerBase<DeletePaymentMethodCommand, bool>(parameters)
{
    private readonly IRepository<PaymentMethod>  _repository = repository;

    public override async Task<bool> Handle(DeletePaymentMethodCommand request, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(request.Id, cancellationToken);
        return true;
    }
}