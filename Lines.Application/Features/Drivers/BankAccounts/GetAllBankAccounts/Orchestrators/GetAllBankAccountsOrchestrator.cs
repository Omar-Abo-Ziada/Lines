using Lines.Application.Features.Drivers.BankAccounts.DTOs;
using Lines.Application.Features.Drivers.BankAccounts.GetAllBankAccounts.Queries;
using Lines.Application.Interfaces;

namespace Lines.Application.Features.Drivers.BankAccounts.GetAllBankAccounts.Orchestrators;

public record GetAllBankAccountsOrchestrator(Guid UserId) : IRequest<Result<List<BankAccountDto>>>;

public class GetAllBankAccountsOrchestratorHandler(
    RequestHandlerBaseParameters parameters,
    IApplicationUserService applicationUserService)
    : RequestHandlerBase<GetAllBankAccountsOrchestrator, Result<List<BankAccountDto>>>(parameters)
{
    public async override Task<Result<List<BankAccountDto>>> Handle(GetAllBankAccountsOrchestrator request, CancellationToken cancellationToken)
    {
        try
        {
            // Get driver ID from user ID
            var userDriverIds = await applicationUserService.GetPassengerAndDriverIdsByUserIdAsync(request.UserId);
            if (userDriverIds?.DriverId == null)
            {
                return Result<List<BankAccountDto>>.Failure(new Application.Shared.Error("400", "User is not a driver", Application.Shared.ErrorType.Validation));
            }

            // Query bank accounts
            var result = await _mediator.Send(new GetAllBankAccountsQuery(userDriverIds.DriverId.Value), cancellationToken);
            return result;
        }
        catch (Exception ex)
        {
            return Result<List<BankAccountDto>>.Failure(new Application.Shared.Error("500", $"Error retrieving bank accounts: {ex.Message}", Application.Shared.ErrorType.Failure));
        }
    }
}

