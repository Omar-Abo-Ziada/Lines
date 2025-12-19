using Lines.Application.Features.FCMNotifications.Queries;
using Lines.Application.Interfaces;
using Lines.Application.Interfaces.Notifications;
using Lines.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 

namespace Lines.Infrastructure.Services.Notifications
{
    

    public class SendFcmTokensQuery : ISendFcmTokensQuery
    {
        private readonly ApplicationDBContext _db;

        public SendFcmTokensQuery(ApplicationDBContext db)
        {
            _db = db;
        }

        public async Task<List<string>> GetTokensAsync(List<Guid> userIds, CancellationToken cancellationToken)
        {
            if (userIds == null || userIds.Count == 0)
                return new List<string>();

            return await _db.FCMUserTokens
                .Where(t => userIds.Contains(t.UserId) && t.IsActive)
                .Select(t => t.Token)
                .ToListAsync(cancellationToken);
        }
    }

}
