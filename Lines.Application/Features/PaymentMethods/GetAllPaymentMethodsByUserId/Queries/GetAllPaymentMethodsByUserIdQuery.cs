using Lines.Domain.Models.PaymentMethods;
using LinqKit;

namespace Lines.Application.Features.PaymentMethods.GetAllPaymentMethodsByUserId.Queries
{
    public record GetAllPaymentMethodsByUserIdQuery(Guid userId) : IRequest<List<PaymentMethod>>;


    public class GetAllPaymentMethodsByUserIdQueryHandler(RequestHandlerBaseParameters parameters, IRepository<PaymentMethod> repository)
        : RequestHandlerBase<GetAllPaymentMethodsByUserIdQuery, List<PaymentMethod>>(parameters)
    {
        private readonly IRepository<PaymentMethod> _repository = repository;

        public override async Task<List<PaymentMethod>> Handle(GetAllPaymentMethodsByUserIdQuery request, CancellationToken cancellationToken)
        {
            var predicate = PredicateBuilder.New<PaymentMethod>(true);

            predicate = predicate.And(x => x.UserId == request.userId);

            return await _repository
                .Get(predicate)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
