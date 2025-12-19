using Lines.Application.Features.Passengers.UpdatePassengerProfile.Orchestrators;


namespace Lines.Presentation.Endpoints.Passengers.UpdatePassengerProfile
{
    
    public class UpdatePassengerProfileEndPoint : BaseController<UpdatePassengerProfileRequest, UpdatePassengerProfileResponse>
    {
        private readonly BaseControllerParams<UpdatePassengerProfileRequest> _dependencyCollection;

        public UpdatePassengerProfileEndPoint(BaseControllerParams<UpdatePassengerProfileRequest> dependencyCollection)
            : base(dependencyCollection)
        {
            _dependencyCollection = dependencyCollection;
        }

        [HttpPut("passengers/update-passenger-profile")]
        public async Task<ApiResponse<UpdatePassengerProfileResponse>> HandleAsync(UpdatePassengerProfileRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await ValidateRequestAsync(request);
            if (!validationResult.IsSuccess)
                return ApiResponse<UpdatePassengerProfileResponse>.ErrorResponse(validationResult.Error!, validationResult.StatusCode);

            var userId = GetCurrentUserId();
            if (userId == Guid.Empty)
                return ApiResponse<UpdatePassengerProfileResponse>.ErrorResponse(
                    new Lines.Application.Shared.Error("UNAUTHORIZED", "User not authenticated", Lines.Application.Shared.ErrorType.Validation), 401);


            var result = await _mediator.Send(
                new UpdatePassengerProfileOrchestrator(userId, request.FirstName, request.LastName, request.PhoneNumber),
                cancellationToken);

            if (result.IsFailure)
                return ApiResponse<UpdatePassengerProfileResponse>.ErrorResponse(result.Error, 400);


            var response = new UpdatePassengerProfileResponse
            {
                Success = true,
                Message = "Passenger profile updated successfully",
                //FullName = $"{request.FristName} {request.LastName}",
            };
            return ApiResponse<UpdatePassengerProfileResponse>.SuccessResponse(response, 200);

        }
    }
}
