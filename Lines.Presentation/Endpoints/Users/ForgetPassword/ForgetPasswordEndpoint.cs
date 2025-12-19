//using Lines.Presentation.Common;
//using MediatR;
//using Microsoft.AspNetCore.Mvc;

//namespace Lines.Presentation.Endpoints.Users.ForgetPassword
//{
//    public class ForgetPasswordEndpoint : BaseController<ForgetPasswordRequest, Unit>
//    {
//        private readonly BaseControllerParams<ForgetPasswordRequest> _dependencyCollection;

//        public ForgetPasswordEndpoint(BaseControllerParams<ForgetPasswordRequest> dependencyCollection)
//            : base(dependencyCollection)
//        {
//            _dependencyCollection = dependencyCollection;
//        }

//        [HttpPost("users/forget-password")]
//        public async Task<ApiResponse<Unit>> HandleAsync(ForgetPasswordRequest request, CancellationToken cancellationToken)
//        {
//            var validationResult = await ValidateRequestAsync(request);
//            if (!validationResult.IsSuccess)
//            {
//                return validationResult;
//            }

//            var result = await _mediator.Send(new ForgetPasswordOrchestrator(request.email), cancellationToken);

//            if (result.IsFailure)
//            {
//                return ApiResponse<Unit>.ErrorResponse(result.Error, 400);
//            }

//            return ApiResponse<Unit>.SuccessResponse(Unit.Value, 200);
//        }
//    }
//}
