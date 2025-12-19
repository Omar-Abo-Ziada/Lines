using Lines.Application.Interfaces;

namespace Lines.Application.Features.Users.ConfirmMail.Commands
{
    public record ConfirmMailCommand(Guid UserId) : IRequest<Result>;



    public class ConfirmMailCommandHandler(IApplicationUserService applicationUserService) : IRequestHandler<ConfirmMailCommand, Result>
    {
        public async Task<Result> Handle(ConfirmMailCommand request, CancellationToken cancellationToken)
        {
           return await applicationUserService.ConfirmMailAsync(request.UserId);
        }
    }
}
