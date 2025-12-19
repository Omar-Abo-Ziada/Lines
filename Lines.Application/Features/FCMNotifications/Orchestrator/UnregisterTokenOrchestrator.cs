//using Lines.Application.Features.FCMNotifications.Commands;
//using Lines.Application.Interfaces;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Lines.Application.Features.FCMNotifications.Orchestrator
//{
//    public class UnregisterTokenOrchestrator
//    : RequestHandlerBase<UnregisterTokenCommand, Result<Unit>>
//    {
//        private readonly ApplicationDBContext _db;

//        public UnregisterTokenOrchestrator(RequestHandlerBaseParameters p, ApplicationDBContext db)
//            : base(p) { _db = db; }

//        public override async Task<Result<Unit>> Handle(UnregisterTokenCommand request, CancellationToken ct)
//        {
//            var row = await _db.FCMUserTokens.FirstOrDefaultAsync(t => t.Token == request.Token, ct);
//            if (row == null) return Result<Unit>.Success(Unit.Value); // لا مشكلة

//            if (request.Soft)
//            {
//                row.IsActive = false;
//                row.LastUpdatedOnUtc = DateTime.UtcNow;
//            }
//            else
//            {
//                _db.FCMUserTokens.Remove(row);
//            }
//            await _db.SaveChangesAsync(ct);
//            return Result<Unit>.Success(Unit.Value);
//        }
//    }
//}
