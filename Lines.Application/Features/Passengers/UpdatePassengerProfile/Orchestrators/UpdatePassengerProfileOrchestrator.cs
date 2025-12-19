using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lines.Application.Features.Drivers.UpdateDriverContact.Orchestrators;
using Lines.Application.Interfaces;

namespace Lines.Application.Features.Passengers.UpdatePassengerProfile.Orchestrators
{

    public record UpdatePassengerProfileOrchestrator( Guid Id,string FirstName, string LastName , string PhoneNumber) : IRequest<Result<bool>>;

    public class UpdatePassengerProfileOrchestratorHandler(RequestHandlerBaseParameters parameters, IApplicationUserService applicationUserService)
        : RequestHandlerBase<UpdatePassengerProfileOrchestrator, Result<bool>>(parameters)
    {

        public override async Task<Result<bool>> Handle(UpdatePassengerProfileOrchestrator request, CancellationToken cancellationToken)
        {

            try
            {
                // Use ApplicationUserService to update  directly
                var updateResult = await applicationUserService.UpdatePassenerProfileAsync(request.Id, request.FirstName, request.LastName, request.PhoneNumber);

                if (updateResult.IsFailure)
                {
                    return Result<bool>.Failure(updateResult.Error);
                }

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(new Application.Shared.Error("500", $"An error occurred while updating driver contact: {ex.Message}", Application.Shared.ErrorType.Failure));
            }
        }

    }
}
