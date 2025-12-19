using Lines.Application.Features.RewardActions.GetRewardActionByType.Orchestrators;
using Lines.Application.Features.RewardActions.GetRewardActionByType.Queries;
using Lines.Application.Features.Users.GetUserIdByMail.Orchestrators;
using Lines.Application.Features.Users.LoginUser.Commands;
using Lines.Application.Features.Users.Register_User.DTOs;
using Lines.Application.Features.Users.Register_User_Google.Commands;

namespace Lines.Application.Features.Users.Register_User.Orchestrators;
public record RegisterUserWithGoogleOrchestrator(RegisterType RegisterType) : IRequest<Result<RegisterUserDTO>>;

public class RegisterUserWithGoogleOrchestratorHandler(RequestHandlerBaseParameters parameters)
    : RequestHandlerBase<RegisterUserWithGoogleOrchestrator, Result<RegisterUserDTO>>(parameters)
{
    public override async Task<Result<RegisterUserDTO>> Handle(RegisterUserWithGoogleOrchestrator request, CancellationToken cancellationToken)
    {
        //1- Authenticate with Google and extract email and name
        var authenticateResult = await _mediator.Send(new AuthenticateGoogleCommand(), cancellationToken);
        if (authenticateResult.IsFailure)
        {
            return Result<RegisterUserDTO>.Failure(authenticateResult.Error);
        }


        string email = authenticateResult.Value.Email;
        string firstName = authenticateResult.Value.FirstName;
        string lastName = authenticateResult.Value.LastName;
        string googleProviderKey = authenticateResult.Value.googleProviderKey;

        // 2- Check if user already exists by email
        var existingUserResult = await _mediator.Send(new GetUserIdByMailOrchestrator(email), cancellationToken);
        if (existingUserResult.IsSuccess)
        {
            // TODO can u redirect direct to login with google command ?
            return Result<RegisterUserDTO>.Failure(UserErrors.RegisterUserError($"User Already Exists with this Email : {email} , Please Login Instead"));
        }

        // 3- User does not exist, proceed with registration
        var referralCode = ReferralCodeGenerator.Generate();

        // 4- get points to be gained on register from reward actions table
        var completeProfileRewardActionDto = await _mediator.Send(new GetRewardActionByTypeOrchestrator(RewardActionType.CompleteProfile));
        if(completeProfileRewardActionDto.IsFailure)
        {
            return Result<RegisterUserDTO>.Failure(completeProfileRewardActionDto.Error);
        }

        var registerCommand = new RegisterUserWithGoogleCommand(FirstName: firstName,
            LastName: lastName,
            Email: email,
            RegisterType: request.RegisterType,
            googleProviderKey: googleProviderKey,
            referralCode: referralCode,
            Points: completeProfileRewardActionDto.Value.Points);

        var registerResult = await _mediator.Send(registerCommand, cancellationToken);
        if (registerResult.IsFailure)
        {
            return Result<RegisterUserDTO>.Failure(registerResult.Error);
        }

        // 4- Return registration details if success
        return Result<RegisterUserDTO>.Success(registerResult.Value);
    }
}