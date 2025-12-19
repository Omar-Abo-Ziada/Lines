using Lines.Application.Interfaces;

namespace Lines.Application.Features.Users.UpdatePassword.Commands
{
    public record UpdatePasswordCommand(Guid UserId, string NewPassword, string? CurrentPassword) : IRequest<Result>;

    public class UpdatePasswordCommandHandler(
        RequestHandlerBaseParameters parameters,
        IApplicationUserService applicationUserService)
        : RequestHandlerBase<UpdatePasswordCommand, Result>(parameters)
    {
        private readonly IApplicationUserService _applicationUserService = applicationUserService;

        public override async Task<Result> Handle(UpdatePasswordCommand request, CancellationToken cancellationToken)
        {
            return await _applicationUserService.UpdatePasswordAsync(request.UserId, request.NewPassword, request.CurrentPassword);
        }
    }
}
