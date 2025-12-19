using FluentValidation;
using Lines.Application.Features.Drivers.RegisterDriver.DTOs;
using Lines.Application.Features.Drivers.RegisterDriver.Queries;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.Drivers.RegisterDriver.Resume;

public class ResumeDriverRegistrationRequest
{
    public string Email { get; set; } = string.Empty;
}

public class ResumeDriverRegistrationRequestValidator : AbstractValidator<ResumeDriverRegistrationRequest>
{
    public ResumeDriverRegistrationRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");
    }
}

public class ResumeDriverRegistrationResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? RegistrationToken { get; set; }
    public string? NextStep { get; set; }
    public int CompletedSteps { get; set; }
    public int TotalSteps { get; set; }
    public DateTime CreatedDate { get; set; }
}

public class ResumeDriverRegistrationEndpoint : BaseController<ResumeDriverRegistrationRequest, ResumeDriverRegistrationResponse>
{
    public ResumeDriverRegistrationEndpoint(BaseControllerParams<ResumeDriverRegistrationRequest> dependencyCollection) : base(dependencyCollection)
    {
    }

    [HttpPost("drivers/register/resume")]
    public async Task<ApiResponse<ResumeDriverRegistrationResponse>> HandleAsync(
        ResumeDriverRegistrationRequest request, 
        CancellationToken cancellationToken)
    {
        try
        {
            // Call query to get registration by email
            var result = await _mediator.Send(new GetDriverRegistrationByEmailQuery(request.Email), cancellationToken);

            if (result.IsFailure)
            {
                return ApiResponse<ResumeDriverRegistrationResponse>.ErrorResponse(result.Error, 400);
            }

            var resumeData = result.Value;
            var response = new ResumeDriverRegistrationResponse
            {
                Success = true,
                Message = $"Registration resumed successfully. You can continue from {resumeData.NextStep}.",
                RegistrationToken = resumeData.RegistrationToken,
                NextStep = resumeData.NextStep,
                CompletedSteps = resumeData.CompletedSteps,
                TotalSteps = resumeData.TotalSteps,
                CreatedDate = resumeData.CreatedDate
            };

            return ApiResponse<ResumeDriverRegistrationResponse>.SuccessResponse(response);
        }
        catch (Exception ex)
        {
            return ApiResponse<ResumeDriverRegistrationResponse>.ErrorResponse(
                new Lines.Application.Shared.Error("General.Error", $"An error occurred: {ex.Message}", Lines.Application.Shared.ErrorType.Failure), 
                500);
        }
    }
}

