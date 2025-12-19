using Lines.Application.Features.Drivers.BankAccounts.DTOs;
using Lines.Application.Features.Drivers.BankAccounts.UpdateBankAccount.Orchestrators;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.Drivers.BankAccounts.UpdateBankAccount;

[Authorize]
public class UpdateBankAccountEndpoint : BaseController<UpdateBankAccountRequest, UpdateBankAccountResponse>
{
    public UpdateBankAccountEndpoint(BaseControllerParams<UpdateBankAccountRequest> dependencyCollection)
        : base(dependencyCollection)
    {
    }

    [HttpPut("driver/bank-accounts/{id}")]
    public async Task<ApiResponse<UpdateBankAccountResponse>> UpdateBankAccount(Guid id, UpdateBankAccountRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await ValidateRequestAsync(request);
        if (!validationResult.IsSuccess)
        {
            return ApiResponse<UpdateBankAccountResponse>.ErrorResponse(validationResult.Error!, validationResult.StatusCode);
        }

        var userId = GetCurrentUserId();
        if (userId == Guid.Empty)
        {
            return ApiResponse<UpdateBankAccountResponse>.ErrorResponse(
                new Lines.Application.Shared.Error("UNAUTHORIZED", "User not authenticated", Lines.Application.Shared.ErrorType.Validation), 401);
        }

        var bankAccountDto = new UpdateBankAccountDto
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

        var result = await _mediator.Send(new UpdateBankAccountOrchestrator(userId, id, bankAccountDto), cancellationToken);

        if (result.IsFailure)
        {
            var statusCode = result.Error?.Code == "404" ? 404 : (result.Error?.Code == "403" ? 403 : 400);
            return ApiResponse<UpdateBankAccountResponse>.ErrorResponse(result.Error, statusCode);
        }

        var response = new UpdateBankAccountResponse
        {
            Success = true,
            Message = "Bank account updated successfully."
        };

        return ApiResponse<UpdateBankAccountResponse>.SuccessResponse(response);
    }
}

