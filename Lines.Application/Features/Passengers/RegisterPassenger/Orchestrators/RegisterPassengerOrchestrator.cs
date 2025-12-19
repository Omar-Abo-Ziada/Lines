using Lines.Application.Common.Email;
using Lines.Application.Features.Otps.CreateOtp.Orchestrators;
using Lines.Application.Features.Passengers.GetPassengerByReferralCode.Orchestrators;
using Lines.Application.Features.RewardActions.GetRewardActionByType.Orchestrators;
using Lines.Application.Features.RewardActions.GetRewardActionByType.Queries;

namespace Lines.Application.Features.Passengers;
public record RegisterPassengerOrchestrator(string firstName, string lastName, string email, string phoneNumber, string password, string? friendReferralCode)
    : IRequest<Result<Guid>>;

public class RegisterPassengerOrchestratorHandler(RequestHandlerBaseParameters parameters, IEmailService emailService)
    : RequestHandlerBase<RegisterPassengerOrchestrator, Result<Guid>>(parameters)
{
    public override async Task<Result<Guid>> Handle(RegisterPassengerOrchestrator request, CancellationToken cancellationToken)
    {
        bool isReferralCodeSubmitted = false;

        if (!string.IsNullOrEmpty(request.friendReferralCode))
        {
            // get passenger by referral code
            var passenger = await _mediator.Send(new GetPassengerByReferralCodeOrchestrator(request.friendReferralCode));

            if(passenger.IsFailure)
            {
                return Result<Guid>.Failure(PassengerErrors.InvalidReferralCodeError());
            }

            // get points to be gained on referral from reward actions table
            var referFriendRewardActionDto = await _mediator.Send(new GetRewardActionByTypeQuery(RewardActionType.ReferFriend));

            passenger.Value.AddPoints(referFriendRewardActionDto.Points);
            isReferralCodeSubmitted = true;
        }

        // get points to be gained on register from reward actions table
        var completeProfileRewardActionDto = await _mediator.Send(new GetRewardActionByTypeOrchestrator(RewardActionType.CompleteProfile));
        if(completeProfileRewardActionDto.IsFailure)
        {
            return Result<Guid>.Failure(completeProfileRewardActionDto.Error);
        }

        // generate random refer code and add it to passenger
        var personalReferralCode = ReferralCodeGenerator.Generate();

        var registerResult = await _mediator.Send(new RegisterPassengerCommand(request.firstName, request.lastName, request.email, 
            request.phoneNumber, request.password, completeProfileRewardActionDto.Value.Points, personalReferralCode, isReferralCodeSubmitted), cancellationToken);
        if (!registerResult.Succeeded || registerResult.userId is null)
        {
            return Result<Guid>.Failure(PassengerErrors.RegisterPassengerError(string.Join(" , ", registerResult.Errors)));
        }

        var createOtpResult = await _mediator.Send(new CreateOtpOrchestrator((Guid)registerResult.userId));

        string fullName = request.firstName + " " + request.lastName;

        MailData mailData = new(request.email, fullName, MailSubjects.Otp,
                                         string.Format(MailFormat.Otp.Html, fullName, createOtpResult.Value.Code));

        var sendMailResult = await emailService.SendMailAsync(mailData);

        if (!sendMailResult.Succeeded)
        {
            return Result<Guid>.Failure(MailErrors.SendMailError(string.Join(" , ", sendMailResult.Errors)));
        }


        return Result<Guid>.Success((Guid)registerResult.userId);
    }
}