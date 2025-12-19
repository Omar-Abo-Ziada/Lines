using Lines.Application.Features.Drivers.BankAccounts.DTOs;
using Lines.Application.Features.Drivers.BankAccounts.GetBankAccountById.Orchestrators;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.Drivers.BankAccounts.GetBankAccountById;

[Authorize]
public class GetBankAccountByIdEndpoint : BaseController<GetBankAccountByIdRequest, GetBankAccountByIdResponse>
{
    public GetBankAccountByIdEndpoint(BaseControllerParams<GetBankAccountByIdRequest> dependencyCollection)
        : base(dependencyCollection)
    {
    }

    [HttpGet("driver/bank-accounts/{id}")]
    public async Task<ApiResponse<GetBankAccountByIdResponse>> GetBankAccountById(Guid id, CancellationToken cancellationToken)
    {
        var request = new GetBankAccountByIdRequest { Id = id };
        
        var validationResult = await ValidateRequestAsync(request);
        if (!validationResult.IsSuccess)
        {
            return ApiResponse<GetBankAccountByIdResponse>.ErrorResponse(validationResult.Error!, validationResult.StatusCode);
        }

        var userId = GetCurrentUserId();
        if (userId == Guid.Empty)
        {
            return ApiResponse<GetBankAccountByIdResponse>.ErrorResponse(
                new Lines.Application.Shared.Error("UNAUTHORIZED", "User not authenticated", Lines.Application.Shared.ErrorType.Validation), 401);
        }

        var result = await _mediator.Send(new GetBankAccountByIdOrchestrator(userId, id), cancellationToken);

        if (result.IsFailure)
        {
            var statusCode = result.Error?.Code == "404" ? 404 : (result.Error?.Code == "403" ? 403 : 400);
            return ApiResponse<GetBankAccountByIdResponse>.ErrorResponse(result.Error, statusCode);
        }

        var response = new GetBankAccountByIdResponse
        {
            BankAccount = result.Value
        };

        return ApiResponse<GetBankAccountByIdResponse>.SuccessResponse(response);
    }
}

