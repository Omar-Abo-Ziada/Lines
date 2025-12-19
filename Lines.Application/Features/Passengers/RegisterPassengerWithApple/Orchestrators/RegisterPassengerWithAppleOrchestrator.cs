using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lines.Application.Features.Passengers.RegisterPassengerWithApple.Commands;
using Lines.Application.Features.Passengers.RegisterPassengerWithApple.DTOs;
using Lines.Application.Features.RewardActions.GetRewardActionByType.Orchestrators;
using Lines.Application.Features.Users;
using Lines.Application.Features.Users.GetUserIdByMail.Orchestrators;
using Lines.Application.Features.Users.LoginUser.Commands;
using Lines.Application.Features.Users.Register_User.DTOs;
using Lines.Application.Features.Users.Register_User_Google.Commands;

namespace Lines.Application.Features.Passengers.RegisterPassengerWithApple.Orchestrators
{

    public record RegisterPassengerWithAppleOrchestrator(RegisterType RegisterType) : IRequest<Result<RegisterPassengerWithAppleDTO>>;

    public class RegisterUserWithGoogleOrchestratorHandler(RequestHandlerBaseParameters parameters)
        : RequestHandlerBase<RegisterPassengerWithAppleOrchestrator, Result<RegisterPassengerWithAppleDTO>>(parameters)
    {
        public override async Task<Result<RegisterPassengerWithAppleDTO>> Handle(RegisterPassengerWithAppleOrchestrator request, CancellationToken cancellationToken)
        {
            //1- Authenticate with Apple and extract email and name
            var authenticateResult = await _mediator.Send(new AuthenticateAppleCommand(), cancellationToken);
            if (authenticateResult.IsFailure)
            {
                return Result<RegisterPassengerWithAppleDTO>.Failure(authenticateResult.Error);
            }
            string email = authenticateResult.Value.Email;
            string firstName = authenticateResult.Value.FirstName;
            string lastName = authenticateResult.Value.LastName;
            string appleProviderKey = authenticateResult.Value.AppleProviderKey;

            // 2- Check if user already exists by email
            var existingUserResult = await _mediator.Send(new GetUserIdByMailOrchestrator(email), cancellationToken);
            if (existingUserResult.IsSuccess)
            {
                // TODO can u redirect direct to login with apple command ?
                return Result<RegisterPassengerWithAppleDTO>.Failure(UserErrors.RegisterUserError($"User Already Exists with this Email : {email} , Please Login Instead"));
            }
            // 3- User does not exist, proceed with registration
            var referralCode = ReferralCodeGenerator.Generate();

            // 4- get points to be gained on register from reward actions table
            var completeProfileRewardActionDto = await _mediator.Send(new GetRewardActionByTypeOrchestrator(RewardActionType.CompleteProfile));
            if (completeProfileRewardActionDto.IsFailure)
            {
                return Result<RegisterPassengerWithAppleDTO>.Failure(completeProfileRewardActionDto.Error);
            }

            var registerCommand = new RegisterPassengerWithAppleCommand(FirstName: firstName,
        LastName: lastName,
        Email: email,
        RegisterType: request.RegisterType,
        AppleProviderKey: appleProviderKey,
        referralCode: referralCode,
        Points: completeProfileRewardActionDto.Value.Points);

            var registerResult = await _mediator.Send(registerCommand, cancellationToken);
            if (registerResult.IsFailure)
            {
                return Result<RegisterPassengerWithAppleDTO>.Failure(registerResult.Error);
            }

            // 4- Return registration details if success
            return Result<RegisterPassengerWithAppleDTO>.Success(registerResult.Value);


        }

    }
}