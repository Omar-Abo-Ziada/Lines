using Lines.Application.Interfaces;
using Lines.Domain.Models.User;
using MediatR;

namespace Lines.Application.Features.Otps.UpdateOtp.Commands
{
    public record UpdateOtpCommand(Otp otp) : IRequest<bool>;

    public class UpdateOtpCommandHandler(IRepository<Otp> repository) : IRequestHandler<UpdateOtpCommand, bool>
    {
        public async Task<bool> Handle(UpdateOtpCommand request, CancellationToken cancellationToken)
        {
            await repository.UpdateAsync(request.otp, cancellationToken);
            return true;
        }
    }


}
