using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Helpers;
using Lines.Application.Features.VehicleTypes.DTOs;
using Lines.Application.Features.VehicleTypes.GetallVehicleTypesWithExpectedPrice.Dtos;
using Lines.Domain.Models.User;
using Lines.Domain.Models.Vehicles;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace Lines.Application.Features.VehicleTypes.GetallVehicleTypesWithExpectedPrice.Queries
{

    public record GetallVehicleTypesWithExpectedPriceQuery(decimal Km, Reward? reward) : IRequest<List<GetAllVehicleTypesWithExpectedPriceDto>>;
    public class GetallVehicleTypesWithExpectedPriceQueryHandler(
        RequestHandlerBaseParameters parameters,
        IRepository<VehicleType> repository)
        : RequestHandlerBase<GetallVehicleTypesWithExpectedPriceQuery, List<GetAllVehicleTypesWithExpectedPriceDto>>(parameters)
    {
        private readonly IRepository<VehicleType> _repository = repository;
        public override async Task<List<GetAllVehicleTypesWithExpectedPriceDto>> Handle(  GetallVehicleTypesWithExpectedPriceQuery request,CancellationToken cancellationToken)
        {
            var vehicles = await _repository.Get()
                .AsNoTracking()
                .Select(v => new GetAllVehicleTypesWithExpectedPriceDto
                {
                    Id = v.Id,
                    Name = v.Name,
                    PerKmCharge = v.PerKmCharge,
                    Capacity = v.Capacity,
                    AverageSpeedKmPerHour = v.AverageSpeedKmPerHour,
                    ExpectedPrice = v.PerKmCharge * request.Km,
                    EstimatedTimeInMinutes = v.AverageSpeedKmPerHour > 0
                        ? (int)Math.Ceiling((request.Km / v.AverageSpeedKmPerHour) * 60)
                        : 0
                })
                .ToListAsync();

            if (request.reward is not null)
            {
                foreach (var vehicle in vehicles)
                {
                    var discountAmount = vehicle.ExpectedPrice * request.reward.DiscountPercentage;

                    if (discountAmount > request.reward.MaxValue)
                        discountAmount = request.reward.MaxValue;

                    vehicle.ExpectedPriceAfterDiscount = vehicle.ExpectedPrice - discountAmount;
                }
            }


            return vehicles;


        }

    }
}
    
