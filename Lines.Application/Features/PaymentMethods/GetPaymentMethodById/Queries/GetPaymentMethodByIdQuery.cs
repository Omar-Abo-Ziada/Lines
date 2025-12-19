using Lines.Domain.Models.PaymentMethods;
using LinqKit;

namespace Lines.Application.Features.PaymentMethods.GetPaymentMethodById.Queries;
public record GetPaymentMethodByIdQuery(Guid Id) : IRequest<PaymentMethod>;
public class GetPaymentMethodByIdQueryHandler(
    RequestHandlerBaseParameters parameters,
    IRepository<PaymentMethod> repository)
    : RequestHandlerBase<GetPaymentMethodByIdQuery, PaymentMethod>(parameters)
{
    private readonly IRepository<PaymentMethod> _repository = repository;

    public override async Task<PaymentMethod> Handle(GetPaymentMethodByIdQuery request, CancellationToken cancellationToken)
    {
        var predicate = PredicateBuilder.New<PaymentMethod>(true);
        predicate = predicate.And(x => x.Id == request.Id);
        return await _repository
            .Get(predicate)
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);
    }
}