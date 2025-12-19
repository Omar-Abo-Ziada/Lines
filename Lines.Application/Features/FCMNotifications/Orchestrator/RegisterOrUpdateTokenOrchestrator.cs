using Lines.Application.Features.FCMNotifications.Commands;
using Lines.Application.Interfaces.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lines.Application.Features.FCMNotifications.Orchestrator
{
    public class RegisterOrUpdateTokenOrchestrator
      : RequestHandlerBase<RegisterOrUpdateTokenCommand, Result<bool>>
    {
        private readonly IFCMUserTokenService _svc;

        public RegisterOrUpdateTokenOrchestrator(RequestHandlerBaseParameters p, IFCMUserTokenService svc)
            : base(p) { _svc = svc; }

        public override async Task<Result<bool>> Handle(RegisterOrUpdateTokenCommand request, CancellationToken ct)
        {
            await _svc.RegisterOrUpdateTokenAsync(request.Request);
            return Result<bool>.Success(true);
        }
    }
}
