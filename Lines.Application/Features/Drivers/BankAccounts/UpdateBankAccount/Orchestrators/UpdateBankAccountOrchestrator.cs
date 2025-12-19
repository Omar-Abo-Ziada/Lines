using Lines.Application.Features.Drivers.BankAccounts.DTOs;
using Lines.Application.Features.Drivers.BankAccounts.UpdateBankAccount.Commands;
using Lines.Application.Interfaces;

namespace Lines.Application.Features.Drivers.BankAccounts.UpdateBankAccount.Orchestrators;

public record UpdateBankAccountOrchestrator(Guid UserId, Guid BankAccountId, UpdateBankAccountDto BankAccountData) : IRequest<Result<BankAccountDto>>;

public class UpdateBankAccountOrchestratorHandler(
    RequestHandlerBaseParameters parameters,
    IApplicationUserService applicationUserService)
    : RequestHandlerBase<UpdateBankAccountOrchestrator, Result<BankAccountDto>>(parameters)
{
    public async override Task<Result<BankAccountDto>> Handle(UpdateBankAccountOrchestrator request, CancellationToken cancellationToken)
    {
        try
        {
            // Get driver ID from user ID
            var userDriverIds = await applicationUserService.GetPassengerAndDriverIdsByUserIdAsync(request.UserId);
            if (userDriverIds?.DriverId == null)
            {
                return Result<BankAccountDto>.Failure(new Application.Shared.Error("400", "User is not a driver", Application.Shared.ErrorType.Validation));
            }

            // Update bank account with ownership validation
            var result = await _mediator.Send(new UpdateBankAccountCommand(userDriverIds.DriverId.Value, request.BankAccountId, request.BankAccountData), cancellationToken);
            return result;
        }
        catch (Exception ex)
        {
            return Result<BankAccountDto>.Failure(new Application.Shared.Error("500", $"Error updating bank account: {ex.Message}", Application.Shared.ErrorType.Failure));
        }
    }
}

