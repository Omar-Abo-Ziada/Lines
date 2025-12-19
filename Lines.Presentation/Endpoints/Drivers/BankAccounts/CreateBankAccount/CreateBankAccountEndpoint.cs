using Lines.Application.Features.Drivers.BankAccounts.DTOs;
using Lines.Application.Features.Drivers.BankAccounts.CreateBankAccount.Orchestrators;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.Drivers.BankAccounts.CreateBankAccount;

[Authorize]
public class CreateBankAccountEndpoint : BaseController<CreateBankAccountRequest, CreateBankAccountResponse>
{
    public CreateBankAccountEndpoint(BaseControllerParams<CreateBankAccountRequest> dependencyCollection)
        : base(dependencyCollection)
    {
    }

    [HttpPost("driver/bank-accounts")]
    public async Task<ApiResponse<CreateBankAccountResponse>> CreateBankAccount(CreateBankAccountRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await ValidateRequestAsync(request);
        if (!validationResult.IsSuccess)
        {
            return ApiResponse<CreateBankAccountResponse>.ErrorResponse(validationResult.Error!, validationResult.StatusCode);
        }

        var userId = GetCurrentUserId();
        if (userId == Guid.Empty)
        {
            return ApiResponse<CreateBankAccountResponse>.ErrorResponse(
                new Lines.Application.Shared.Error("UNAUTHORIZED", "User not authenticated", Lines.Application.Shared.ErrorType.Validation), 401);
        }

        var bankAccountDto = new CreateBankAccountDto
        {
            BankName = request.BankName,
            IBAN = request.IBAN,
            SWIFT = request.SWIFT,
            AccountHolderName = request.AccountHolderName,
            AccountNumber = request.AccountNumber,
            BranchName = request.BranchName,
            IsPrimary = request.IsPrimary,
            CardNumber = request.CardNumber,
            ExpiryDate = request.ExpiryDate,
            CVV = request.CVV
        };

        var result = await _mediator.Send(new CreateBankAccountOrchestrator(userId, bankAccountDto), cancellationToken);

        if (result.IsFailure)
        {
            return ApiResponse<CreateBankAccountResponse>.ErrorResponse(result.Error, 400);
        }

        var response = new CreateBankAccountResponse
        {
            Id = result.Value.Id,
            Success = true,
            Message = "Bank account created successfully."
        };

        return ApiResponse<CreateBankAccountResponse>.SuccessResponse(response);
    }
}

