using Lines.Application.Features.Drivers.BankAccounts.DTOs;
using Lines.Application.Features.Drivers.BankAccounts.GetBankAccountById.Queries;
using Lines.Application.Interfaces;

namespace Lines.Application.Features.Drivers.BankAccounts.GetBankAccountById.Orchestrators;

public record GetBankAccountByIdOrchestrator(Guid UserId, Guid BankAccountId) : IRequest<Result<BankAccountDto>>;

public class GetBankAccountByIdOrchestratorHandler(
    RequestHandlerBaseParameters parameters,
    IApplicationUserService applicationUserService)
    : RequestHandlerBase<GetBankAccountByIdOrchestrator, Result<BankAccountDto>>(parameters)
{
    public async override Task<Result<BankAccountDto>> Handle(GetBankAccountByIdOrchestrator request, CancellationToken cancellationToken)
    {
        try
        {
            // Get driver ID from user ID
            var userDriverIds = await applicationUserService.GetPassengerAndDriverIdsByUserIdAsync(request.UserId);
            if (userDriverIds?.DriverId == null)
            {
                return Result<BankAccountDto>.Failure(new Application.Shared.Error("400", "User is not a driver", Application.Shared.ErrorType.Validation));
            }

            // Query bank account with ownership validation
            var result = await _mediator.Send(new GetBankAccountByIdQuery(userDriverIds.DriverId.Value, request.BankAccountId), cancellationToken);
            return result;
        }
        catch (Exception ex)
        {
            return Result<BankAccountDto>.Failure(new Application.Shared.Error("500", $"Error retrieving bank account: {ex.Message}", Application.Shared.ErrorType.Failure));
        }
    }
}

