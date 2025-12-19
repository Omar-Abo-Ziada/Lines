using Lines.Application.Features.Drivers.BankAccounts.CreateBankAccount.Commands;
using Lines.Application.Features.Drivers.BankAccounts.DTOs;
using Lines.Application.Interfaces;

namespace Lines.Application.Features.Drivers.BankAccounts.CreateBankAccount.Orchestrators;

public record CreateBankAccountOrchestrator(Guid UserId, CreateBankAccountDto BankAccountData) : IRequest<Result<BankAccountDto>>;

public class CreateBankAccountOrchestratorHandler(
    RequestHandlerBaseParameters parameters,
    IApplicationUserService applicationUserService)
    : RequestHandlerBase<CreateBankAccountOrchestrator, Result<BankAccountDto>>(parameters)
{
    public async override Task<Result<BankAccountDto>> Handle(CreateBankAccountOrchestrator request, CancellationToken cancellationToken)
    {
        try
        {
            // Get driver ID from user ID
            var userDriverIds = await applicationUserService.GetPassengerAndDriverIdsByUserIdAsync(request.UserId);
            if (userDriverIds?.DriverId == null)
            {
                return Result<BankAccountDto>.Failure(new Application.Shared.Error("400", "User is not a driver", Application.Shared.ErrorType.Validation));
            }

            // Create bank account
            var result = await _mediator.Send(new CreateBankAccountCommand(userDriverIds.DriverId.Value, request.BankAccountData), cancellationToken);
            return result;
        }
        catch (Exception ex)
        {
            return Result<BankAccountDto>.Failure(new Application.Shared.Error("500", $"Error creating bank account: {ex.Message}", Application.Shared.ErrorType.Failure));
        }
    }
}

