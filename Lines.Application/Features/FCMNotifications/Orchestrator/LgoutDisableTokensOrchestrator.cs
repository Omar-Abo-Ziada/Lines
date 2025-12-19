//using Lines.Application.Features.FCMNotifications.Commands;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Lines.Application.Features.FCMNotifications.Orchestrator
//{
//    public class LogoutDisableTokensOrchestrator 
//    : RequestHandlerBase<LogoutDisableTokensCommand, Result<Unit>>
//{
//    private readonly ApplicationDBContext _db;
//    public LogoutDisableTokensOrchestrator(RequestHandlerBaseParameters p, ApplicationDBContext db)
//        : base(p) { _db = db; }

//    public override async Task<Result<Unit>> Handle(LogoutDisableTokensCommand request, CancellationToken ct)
//    {
//        var tokens = await _db.FCMUserTokens
//            .Where(t => t.UserId == request.UserId && t.IsActive)
//            .ToListAsync(ct);

//        foreach (var t in tokens)
//        {
//            t.IsActive = false;
//            t.LastUpdatedOnUtc = DateTime.UtcNow;
//        }
//        await _db.SaveChangesAsync(ct);
//        return Result<Unit>.Success(Unit.Value);
//    }
//}
//}
