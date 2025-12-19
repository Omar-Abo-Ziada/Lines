using Lines.Application.Features.Offers.CreateOffer.DTOs;

namespace Lines.Application.Features.Offers.CreateOffer.Queries.GetTripRequestById;

public record GetTripRequestByIdQuery(Guid tripRequestId) : IRequest<Result<TripRequestDTO?>>;

public class GetTripRequestByIdQueryHandler : RequestHandlerBase<GetTripRequestByIdQuery, Result<TripRequestDTO?>>
{
    private readonly IRepository<TripRequest> _repository;

    public GetTripRequestByIdQueryHandler(RequestHandlerBaseParameters parameters, IRepository<TripRequest> repository)
        : base(parameters)
    {
        _repository = repository;
    }

    public override Task<Result<TripRequestDTO?>> Handle(GetTripRequestByIdQuery request, CancellationToken cancellationToken)
    {
        // 1 - Get TripRequest by ID
        var tripRequest = _repository.Get(q => q.Id == request.tripRequestId);

        // 2 - Map TripRequest to TripRequestDTO
        var tripRequestDto = tripRequest
            .ProjectToType<TripRequestDTO?>()
            .SingleOrDefault();

        // 3 - Handle not found
        if (tripRequestDto == null)
        {
            var error = new Error(
                Code: "TRIPREQUEST:NOTFOUND",
                Description: "Trip request not found",
                Type: ErrorType.NotFound
            );

            return Task.FromResult(Result<TripRequestDTO?>.Failure(error));
        }

        // 4 - Return success result
        return Task.FromResult(Result<TripRequestDTO?>.Success(tripRequestDto));
    }
}
