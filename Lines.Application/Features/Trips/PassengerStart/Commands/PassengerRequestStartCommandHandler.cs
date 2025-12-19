
//using Microsoft.EntityFrameworkCore;

//namespace Lines.Application.Features.Trips.PassengerStart.Commands
//{
//    public record PassengerRequestStartCommand(Guid TripId, Guid PassengerId)
//        : IRequest<Result<bool>>;

//    public class PassengerRequestStartCommandHandler(
//        RequestHandlerBaseParameters parameters,
//        IRepository<Trip> tripRepository)
//        : RequestHandlerBase<PassengerRequestStartCommand, Result<bool>>(parameters)
//    {
//        private readonly IRepository<Trip> _tripRepository = tripRepository;

//        public override async Task<Result<bool>> Handle(PassengerRequestStartCommand request, CancellationToken cancellationToken)
//        {
//            var trip = await _tripRepository
//                .Get(t => !t.IsDeleted && t.Id == request.TripId)
//                .Include(t => t.Passenger)
//                .FirstOrDefaultAsync(cancellationToken);

//            if (trip is null)
//                return Result<bool>.Failure(new Error("Trip.NotFound", "Trip not found.", ErrorType.NotFound));
            
//            if (trip.PassengerId != request.PassengerId)
//                return Result<bool>.Failure(new Error("Trip.Forbidden", "You are not the passenger of this trip.", ErrorType.Validation));

//            // 3️⃣ تحقق من حالة الرحلة (لازم تكون لسه ما بدأتش)
//            if (trip.Status == TripStatus.InProgress || trip.Status == TripStatus.Completed)
//                return Result<bool>.Failure(new Error("Trip.InvalidState", "Trip has already started or completed.", ErrorType.Validation));

//            // 4️⃣ تحقق إن السائق وصل فعلاً قبل ما الراكب يطلب البدء
//            if (trip.DriverArrivedAt is null)
//                return Result<bool>.Failure(new Error("Trip.DriverNotArrived", "Driver must arrive before passenger can request start.", ErrorType.Validation));

//            // 5️⃣ Idempotency — لو تم الضغط مرتين "Start Trip"
//            if (trip.PassengerStartRequestedAt is not null)
//                return Result<bool>.Success(true);

//            trip.PassengerRequestStart();

//            await _tripRepository.UpdateAsync(trip, cancellationToken);
//            await _tripRepository.SaveChangesAsync(cancellationToken);

//            // TODO: ممكن هنا تبعت إشعار للسائق عبر SignalR زي "PassengerRequestedStart"
//            return Result<bool>.Success(true);
//        }
//    }
//}


 