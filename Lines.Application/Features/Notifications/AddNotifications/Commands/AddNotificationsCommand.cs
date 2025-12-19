using Lines.Application.Features.Payments.Stripe;
using Lines.Domain.Models.Users;
using Microsoft.Extensions.Logging;

namespace Lines.Application.Features.Notifications.AddNotifications.Commands;

public record AddNotificationsCommand(
        Guid UserId,
        string Title,
        string Message,
        NotificationType NotificationType
        ) : IRequest<Guid>;
public class AddNotificationsCommandHandler : RequestHandlerBase<AddNotificationsCommand, Guid>
{
    private readonly IRepository<Notification> _repository;
    private readonly IApplicationUserService _appUserService;
    private readonly ILogger<HandleStripePaymentIntentSucceededCommandHandler> _logger;

    public AddNotificationsCommandHandler(RequestHandlerBaseParameters parameters, IRepository<Notification> repository, IApplicationUserService appUserService, ILogger<HandleStripePaymentIntentSucceededCommandHandler> logger) : base(parameters)
    {
        _repository = repository;
        _appUserService = appUserService;
        _logger = logger;
    }

    public override async Task<Guid> Handle(AddNotificationsCommand request, CancellationToken cancellationToken)
    {

        var entity = await _repository.AddAsync(new Notification
        {
          NotificationType = request.NotificationType,
          IsRead = false,
          Message = request.Message,
          Title = request.Title,
          UserId = request.UserId

        }, cancellationToken);

        _repository.SaveChanges();
        return entity.Id;
    }
}
