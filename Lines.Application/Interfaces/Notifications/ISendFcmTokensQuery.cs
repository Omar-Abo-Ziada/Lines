using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lines.Application.Interfaces.Notifications
{
    public interface ISendFcmTokensQuery
    {
        Task<List<string>> GetTokensAsync(List<Guid> userIds, CancellationToken cancellationToken);
    }

}
