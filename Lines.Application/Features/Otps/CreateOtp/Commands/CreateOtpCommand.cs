using Lines.Application.Common;
using Lines.Application.Features.Otps.CreateOtp.DTOs;
using Lines.Application.Interfaces;
using Lines.Application.Interfaces;
using Lines.Domain.Models.User;
using Mapster;
using MediatR;

namespace Lines.Application.Features.Otps.CreateOtp.Commands;

public record CreateOtpCommand(Guid UserId) : IRequest<CreateOtpDto>;

public class CreateOtpCommandHandler(RequestHandlerBaseParameters parameters, IRepository<Otp> repository, IOtpService otpService)
    : RequestHandlerBase<CreateOtpCommand, CreateOtpDto>(parameters)
{
    private readonly IRepository<Otp> _repository = repository;

    public override async Task<CreateOtpDto> Handle(CreateOtpCommand request, CancellationToken cancellationToken)
    {
        Otp otp = new Otp(otpService.GenerateRandomOtp(), request.UserId);
        var result = await _repository.AddAsync(otp)
                         .ConfigureAwait(false);

        return result.Adapt<CreateOtpDto>();
    }
}
