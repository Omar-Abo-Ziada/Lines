using Lines.Application.Common;
using Lines.Application.Interfaces;
using Lines.Domain.Models.User;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lines.Application.Features.Otps.GetOtpByUserId.Queries
{
    public record GetOtpByUserIdQuery(Guid UserId) : IRequest<Otp?>;

    public class GetOtpByUserIdQueryHandler(
  RequestHandlerBaseParameters parameters,
  IRepository<Otp> otpRepository)
  : RequestHandlerBase<GetOtpByUserIdQuery, Otp?>(parameters)
    {
        public override async Task<Otp?> Handle(GetOtpByUserIdQuery request, CancellationToken cancellationToken)
        {

            var otp = await otpRepository
                .Get(t => t.UserId == request.UserId)
                .FirstOrDefaultAsync();

            return otp;
        }
    }
}
