using Application.Common.Helpers;
using AutoMapper.QueryableExtensions;
using Lines.Application.Common;
using Lines.Application.Extensions;
using Lines.Application.Shared;
using Lines.Domain.Enums;
using Lines.Application.Interfaces;
using Lines.Domain.Models.Users;
using LinqKit;
using Mapster;
using MediatR;
using Lines.Application.Features.EmergencyNumbers.Shared.DTOs;

namespace Lines.Application.Features.EmergencyNumbers;

public record GetAllEmergencyNumbersQuery(
    string? name,
    string? phoneNumber,
    int? emergencyNumberType,
    int pageNumber,
    int pageSize,
    Guid userId) : IRequest<PagingDto<GetEmergencyNumberDto>>;  

public class GetAllEmergencyNumbersQueryHandler(
    RequestHandlerBaseParameters parameters,
    IRepository<EmergencyNumber> repository)  
    : RequestHandlerBase<GetAllEmergencyNumbersQuery, PagingDto<GetEmergencyNumberDto>>(parameters)
{
    public override async Task<PagingDto<GetEmergencyNumberDto>> Handle(
        GetAllEmergencyNumbersQuery request, CancellationToken cancellationToken)
    {
        var predicate = PredicateBuilder.New<EmergencyNumber>(true);
        if (!string.IsNullOrEmpty(request.name))
        {
            predicate = predicate.And(x => x.Name.Contains(request.name));
        }
        if (!string.IsNullOrEmpty(request.phoneNumber))
        {
            predicate = predicate.And(x => x.PhoneNumber.Contains(request.phoneNumber));
        }
        if (request.emergencyNumberType.HasValue)
        {
            predicate = predicate.And(x => x.EmergencyNumberType == (EmergencyNumberType)request.emergencyNumberType.Value);
        }
        predicate = predicate.And(x => x.UserId == request.userId || x.UserId == null);

        var query = repository
            .Get(predicate)
            .ProjectToType<GetEmergencyNumberDto>();   

        var result = await PagingHelper.CreateAsync(query, request.pageNumber, request.pageSize);
        return result;
    }
}