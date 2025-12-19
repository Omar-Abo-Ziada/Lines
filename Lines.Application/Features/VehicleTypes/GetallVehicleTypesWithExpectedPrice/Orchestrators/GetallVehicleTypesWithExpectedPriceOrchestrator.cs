using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lines.Application.Features.Rewards.GetRewardByUserRewardId.Orchestrators;
using Lines.Application.Features.Rewards.GetRewardByUserRewardId.Queries;
using Lines.Application.Features.VehicleTypes.DTOs;
using Lines.Application.Features.VehicleTypes.GetallVehicleTypesWithExpectedPrice.Dtos;
using Lines.Application.Features.VehicleTypes.GetallVehicleTypesWithExpectedPrice.Queries;
using Lines.Domain.Models.User;
using Lines.Domain.Shared;

namespace Lines.Application.Features.VehicleTypes.GetallVehicleTypesWithExpectedPrice.Orchestrators
{

    public record GetallVehicleTypesWithExpectedPriceOrchestrator(decimal Km ,  Guid? UserRewardID) : IRequest<Result<List<GetAllVehicleTypesWithExpectedPriceDto>>>;

    public class GetallVehicleTypesWithExpectedPriceOrchestratorHandler : RequestHandlerBase<GetallVehicleTypesWithExpectedPriceOrchestrator, Result<List<GetAllVehicleTypesWithExpectedPriceDto>>>
    {
        public GetallVehicleTypesWithExpectedPriceOrchestratorHandler(RequestHandlerBaseParameters parameters) : base(parameters)
        {

        }

        public override async Task<Result<List<GetAllVehicleTypesWithExpectedPriceDto>>> Handle(GetallVehicleTypesWithExpectedPriceOrchestrator request, CancellationToken cancellationToken)
        {
            Reward? reward = null;

            if (request.UserRewardID.HasValue)
            {
                reward = await _mediator.Send(
                    new GetRewardByUserRewardIdQuery(request.UserRewardID.Value),
                    cancellationToken
                );
            }

            var result = await _mediator.Send(new GetallVehicleTypesWithExpectedPriceQuery(request.Km , reward), cancellationToken);
            return Result<List<GetAllVehicleTypesWithExpectedPriceDto>>.Success(result);
        
        }
    }
}
