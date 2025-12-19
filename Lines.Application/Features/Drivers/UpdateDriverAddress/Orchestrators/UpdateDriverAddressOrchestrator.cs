using Lines.Application.Features.Drivers.UpdateDriverAddress.Commands;
using Lines.Application.Features.Drivers.UpdateDriverAddress.DTOs;
using Lines.Application.Interfaces;
using Lines.Domain.Shared;
using MediatR;

namespace Lines.Application.Features.Drivers.UpdateDriverAddress.Orchestrators;

public record UpdateDriverAddressOrchestrator(Guid userId, UpdateDriverAddressDto addressInfo) : IRequest<Result<bool>>;

public class UpdateDriverAddressOrchestratorHandler(RequestHandlerBaseParameters parameters, IApplicationUserService applicationUserService)
    : RequestHandlerBase<UpdateDriverAddressOrchestrator, Result<bool>>(parameters)
{
    public async override Task<Result<bool>> Handle(UpdateDriverAddressOrchestrator request, CancellationToken cancellationToken)
    {
        try
        {
            var userDriverIds = await applicationUserService.GetPassengerAndDriverIdsByUserIdAsync(request.userId);
            if (userDriverIds?.DriverId == null)
            {
                return Result<bool>.Failure(new Application.Shared.Error("400", "User is not a driver.", Application.Shared.ErrorType.Validation));
            }

            var updateResult = await _mediator.Send(new UpdateDriverAddressCommand(
                userDriverIds.DriverId.Value,
                request.addressInfo.Address,
                request.addressInfo.CityId,
                request.addressInfo.SectorId,
                request.addressInfo.PostalCode
            ), cancellationToken);
            
            if (updateResult.IsFailure)
            {
                return Result<bool>.Failure(updateResult.Error);
            }

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure(new Application.Shared.Error("500", $"An error occurred while updating driver address: {ex.Message}", Application.Shared.ErrorType.Failure));
        }
    }
}
