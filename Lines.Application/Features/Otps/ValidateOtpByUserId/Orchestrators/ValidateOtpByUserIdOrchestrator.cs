using Lines.Application.Common;
using Lines.Application.Features.Otps.GetOtpByUserId.Orchestrators;
using Lines.Application.Features.Otps.ValidateOtpByUserId.DTOs;
using Lines.Domain.Models.User;
using Lines.Domain.Shared;
using MediatR;

namespace Lines.Application.Features.Otps.ValidateOtpByUserId.Orchestrators
{
    public record ValidateOtpByUserIdOrchestrator(Guid userId, string otp) : IRequest<Result<ValidateOtpByUserIdDto>>;

    public class ValidateOtpByUserIdOrchestratorHandler(RequestHandlerBaseParameters parameters)
                            : RequestHandlerBase<ValidateOtpByUserIdOrchestrator, Result<ValidateOtpByUserIdDto>>(parameters)
    {


        public override async Task<Result<ValidateOtpByUserIdDto>> Handle(ValidateOtpByUserIdOrchestrator request, CancellationToken cancellationToken)  
        {

            // get otp by user id orchestrator  >> first 
            var otp = await _mediator.Send(new GetOtpByUserIdOrchestrator(request.userId));

            if(otp.Value is null)
                return Result<ValidateOtpByUserIdDto>.Failure(OtpErrors.InvalidOtpError("OTP not found for the provided user ID"));

            // create otp using domain model 
            Otp domainOtp = new Otp(otp.Value.Code, otp.Value.UserId, otp.Value.OTPGenerationTime);

            // ask if it is valid
            var isValid = domainOtp.IsValid(request.otp);

           
                return Result<ValidateOtpByUserIdDto>.Success(new ValidateOtpByUserIdDto
                { 
                    IsValid = isValid,
                    Otp = isValid ? otp.Value : null
                });

        }
    }
}

