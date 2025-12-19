using Lines.Application.Features.Drivers.BankAccounts.DTOs;
using Lines.Application.Features.Drivers.BankAccounts.GetAllBankAccounts.Orchestrators;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.Drivers.BankAccounts.GetAllBankAccounts;

[Authorize]
public class GetAllBankAccountsEndpoint : BaseController<GetAllBankAccountsRequest, GetAllBankAccountsResponse>
{
    public GetAllBankAccountsEndpoint(BaseControllerParams<GetAllBankAccountsRequest> dependencyCollection)
        : base(dependencyCollection)
    {
    }

    [HttpGet("driver/bank-accounts")]
    public async Task<ApiResponse<GetAllBankAccountsResponse>> GetAllBankAccounts(CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        if (userId == Guid.Empty)
        {
            return ApiResponse<GetAllBankAccountsResponse>.ErrorResponse(
                new Lines.Application.Shared.Error("UNAUTHORIZED", "User not authenticated", Lines.Application.Shared.ErrorType.Validation), 401);
        }

        var result = await _mediator.Send(new GetAllBankAccountsOrchestrator(userId), cancellationToken);

        if (result.IsFailure)
        {
            return ApiResponse<GetAllBankAccountsResponse>.ErrorResponse(result.Error, 400);
        }

        var response = new GetAllBankAccountsResponse
        {
            BankAccounts = result.Value
        };

        return ApiResponse<GetAllBankAccountsResponse>.SuccessResponse(response);
    }
}

