using Lines.Domain.Models.PaymentMethods;

namespace Lines.Application.Features.PaymentMethods.CreatePaymentMethod.Commands
{
    public record CreatePaymentMethodCommand(string PaymentMethodId, string CustomerId, PaymentMethodType Type, bool IsDefault, Guid UserId) 
            : IRequest<Result>;


    public class CreatePaymentMethodCommandHandler : RequestHandlerBase<CreatePaymentMethodCommand, Result>
    {
        private readonly IRepository<PaymentMethod> _repository;
        public CreatePaymentMethodCommandHandler(RequestHandlerBaseParameters parameters, IRepository<PaymentMethod> repository) : base(parameters)
        {
            _repository = repository;
        }

        public override async Task<Result> Handle(CreatePaymentMethodCommand request, CancellationToken cancellationToken)
        {
            var paymentMethod = new PaymentMethod(request.PaymentMethodId, request.CustomerId, request.UserId, request.Type, request.IsDefault);
            await _repository.AddAsync(paymentMethod);
            return Result.Success();
        }
    }

}
