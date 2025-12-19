using Lines.Application.Features.Drivers.UpdateDriverContact.DTOs;
using Lines.Application.Interfaces;
using Lines.Domain.Shared;

namespace Lines.Application.Features.Drivers.UpdateDriverContact.Orchestrators;

public record UpdateDriverContactOrchestrator(Guid userId, UpdateDriverContactDto contactInfo) : IRequest<Result<bool>>;

public class UpdateDriverContactOrchestratorHandler(RequestHandlerBaseParameters parameters, IApplicationUserService applicationUserService)
    : RequestHandlerBase<UpdateDriverContactOrchestrator, Result<bool>>(parameters)
{
    public async override Task<Result<bool>> Handle(UpdateDriverContactOrchestrator request, CancellationToken cancellationToken)
    {
        try
        {
            // Use ApplicationUserService to update contact info directly
            var updateResult = await applicationUserService.UpdateDriverContactAsync(
                request.userId, 
                request.contactInfo.Email, 
                request.contactInfo.PhoneNumber);
            
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
