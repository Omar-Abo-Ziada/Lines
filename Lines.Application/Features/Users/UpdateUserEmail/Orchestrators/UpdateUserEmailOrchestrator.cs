using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lines.Application.Features.Drivers.UpdateDriverContact.DTOs;
using Lines.Application.Features.Drivers.UpdateDriverContact.Orchestrators;
using Lines.Application.Features.Users.UpdateUserEmail.DTOs;

namespace Lines.Application.Features.Users.UpdateUserEmail.Orchestrators
{
    public record UpdateUserEmailOrchestrator(Guid userId, UpdateUserEmailDto dto) : IRequest<Result<bool>>;
    public class UpdateUserEmailOrchestratorHandler(RequestHandlerBaseParameters parameters, IApplicationUserService applicationUserService)
        : RequestHandlerBase<UpdateUserEmailOrchestrator, Result<bool>>(parameters)
    {
        public async override Task<Result<bool>> Handle(UpdateUserEmailOrchestrator request, CancellationToken cancellationToken)
        {
            // Use ApplicationUserService to update contact info directly
            var updateResult = await applicationUserService.UpdateUserEmailAsync(
                request.userId,
                request.dto.CurrentEmail,
                request.dto.NewEmail);

            if (updateResult.IsFailure)
            {
                return Result<bool>.Failure(updateResult.Error);
            }
            return Result<bool>.Success(true);

        }
    }
    }
