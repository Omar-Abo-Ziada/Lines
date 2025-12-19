using AutoMapper;
using Mapster;
using MediatR;
using System.Security.Claims;

namespace Lines.Presentation.Common;
[Route("api/")]
[ApiController]
public class BaseController<TRequest, TResponse> : ControllerBase
{
    protected readonly IMediator _mediator;
    protected readonly IValidator<TRequest> _validator;
    protected readonly IMapper _mapper;

    public BaseController(BaseControllerParams<TRequest> dependencyCollection)
    {
        _mediator = dependencyCollection.Mediator;
        _validator = dependencyCollection.Validator;
        _mapper = dependencyCollection.Mapper;
    }
    protected virtual async Task<ApiResponse<TResponse>> ValidateRequestAsync(TRequest request)
    {
        if (_validator is null)
            return ApiResponse<TResponse>.SuccessResponse(default, 200);

        var validationResults = await _validator.ValidateAsync(request);

        if (validationResults.IsValid)
            return ApiResponse<TResponse>.SuccessResponse(default, 200);


        var validationErrors = string.Join(", ", validationResults.Errors.Select(e => e.ErrorMessage));
        var errMsg = string.Format("Validation failed:\n {0}", validationErrors);
        var error = new Error("Validation.Error", errMsg, ErrorType.Validation);
        return ApiResponse<TResponse>.ErrorResponse(error, 400);
    }

    protected ApiResponse<TDist> HandleResult<TSource, TDist>(Result<TSource> result)
    {
        if (result is { IsSuccess: true, Value: null })
            return ApiResponse<TDist>.SuccessResponse(default!, 200);

        return result.IsSuccess ? ApiResponse<TDist>.SuccessResponse(result.Value.Adapt<TDist>()) : ApiResponse<TDist>.ErrorResponse(result.Error, 400);
    }

    protected Guid GetCurrentUserId()
    {
        var userIdClaim = User?.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return userId;
        }

        return Guid.Empty;
    }

    protected string GetCurrentUserRole()
    {
        var roleClaim = User?.FindFirst(ClaimTypes.Role);

        if (roleClaim != null)
        {
            return roleClaim.Value.ToString();
        }

        return string.Empty;
    }

    protected Guid GetCurrentPassengerId()
    {
        var passengerIdClaim = User?.FindFirst("passengerId");

        if (passengerIdClaim != null && Guid.TryParse(passengerIdClaim.Value, out var passengerId))
        {
            return passengerId;
        }

        return Guid.Empty;
    }

    protected Guid GetCurrentDriverId()
    {
        var driverIdClaim = User?.FindFirst("driverId");

        if (driverIdClaim != null && Guid.TryParse(driverIdClaim.Value, out var driverId))
        {
            return driverId;
        }

        return Guid.Empty;
    }

}