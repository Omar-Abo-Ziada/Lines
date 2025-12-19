using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lines.Application.Interfaces
{
    public interface ITripCodeService
    {
        Task<string> GenerateUniqueTripCodeAsync(CancellationToken cancellationToken = default);
    }

}
