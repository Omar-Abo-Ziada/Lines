using Lines.Application.Interfaces;
using Lines.Domain.Models.User;
using MediatR;

namespace Lines.Application.Features.Otps.DeleteOtpById.Commands
{
    public record DeleteOtpByIdCommand(Guid Id) : IRequest<bool>;

    public class DeleteOtpByIdCommandHandler(IRepository<Otp> otpRepository) : IRequestHandler<DeleteOtpByIdCommand, bool>
    {
        public async Task<bool> Handle(DeleteOtpByIdCommand request, CancellationToken cancellationToken)
        {
            await otpRepository.DeleteAsync(request.Id, cancellationToken, true);
            return true;
        }
    }


}