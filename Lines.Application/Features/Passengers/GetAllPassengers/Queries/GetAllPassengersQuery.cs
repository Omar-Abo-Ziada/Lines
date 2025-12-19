using Application.Common.Helpers;
using Lines.Application.Features.Passengers.SharedDtos;
using Lines.Domain.Models.Passengers;
using LinqKit;

namespace Lines.Application.Features.Passengers.GetAllPassengers.Queries
{
    public record GetAllPassengersQuery (string? firstName, string? lastName, string? email, string? phoneNumber,
        int pageNumber = 1, int pageSize = 10) : IRequest<PagingDto<GetPassengersDto>>;


    public class GetAllPassengersQueryHandler(RequestHandlerBaseParameters parameters , IRepository<Passenger> repository) 
        : RequestHandlerBase<GetAllPassengersQuery , PagingDto<GetPassengersDto>>(parameters)
    {
        public override async Task<PagingDto<GetPassengersDto>> Handle(
       GetAllPassengersQuery request, CancellationToken cancellationToken)
        {
            var predicate = PredicateBuilder.New<Passenger>(true);
            if (!string.IsNullOrEmpty(request.firstName))
            {
                predicate = predicate.And(x => x.FirstName.Contains(request.firstName));
            }
            if (!string.IsNullOrEmpty(request.lastName))
            {
                predicate = predicate.And(x => x.LastName.Contains(request.lastName));
            }
            if (!string.IsNullOrEmpty(request.email))
            {
                predicate = predicate.And(x => x.Email.Value.Contains(request.email));
            }
            if (!string.IsNullOrEmpty(request.phoneNumber))
            {
                predicate = predicate.And(x => x.PhoneNumber.Value.Contains(request.phoneNumber));
            }

            var query = repository
                .Get(predicate)
                .ProjectToType<GetPassengersDto>();

            var result = await PagingHelper.CreateAsync(query, request.pageNumber, request.pageSize);
            return result;
        }
    }
}
