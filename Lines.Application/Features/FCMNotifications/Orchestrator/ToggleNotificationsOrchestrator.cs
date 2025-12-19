//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Lines.Application.Features.FCMNotifications.Orchestrator
//{
//    public class ToggleNotificationsOrchestrator
//     : RequestHandlerBase<ToggleNotificationsCommand, Result<Unit>>
//    {
//        private readonly ApplicationDBContext _db;
//        public ToggleNotificationsOrchestrator(RequestHandlerBaseParameters p, ApplicationDBContext db)
//            : base(p) { _db = db; }

//        public override async Task<Result<Unit>> Handle(ToggleNotificationsCommand request, CancellationToken ct)
//        {
//            var tokens = await _db.FCMUserTokens.Where(t => t.UserId == request.UserId).ToListAsync(ct);
//            foreach (var t in tokens)
//            {
//                t.NotificationsEnabled = request.Enabled;
//                t.LastUpdatedOnUtc = DateTime.UtcNow;
//            }
//            await _db.SaveChangesAsync(ct);
//            return Result<Unit>.Success(Unit.Value);
//        }
//    }
//}
