using Lines.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lines.Application.Services
{
    public class TripCodeService : ITripCodeService
    {
        private readonly IRepository<Trip> _tripRepository;

        public TripCodeService(IRepository<Trip> tripRepository)
        {
            _tripRepository = tripRepository;
        }

        public async Task<string> GenerateUniqueTripCodeAsync(CancellationToken cancellationToken = default)
        {
            string code;

            while (true)
            {
                code = TripCodeGenerator.Generate(4);

                var exists = await _tripRepository.AnyAsync(
                    t => t.TripCode == code,
                    cancellationToken
                );

                if (!exists)
                    return code;
            }
        }
    }

}
