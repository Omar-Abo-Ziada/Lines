using AdminLine.Framework.UoW;
using AdminLine.Service.IService;
using Lines.Domain.Models.Trips;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminLine.Service.Service
{
    public class TripService : ITripService
    {
        private readonly IUnitOfWork _unitOfWork;
        public TripService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task getallTrip()
        {
            var trip = await _unitOfWork.GetRepository<Feedback>().GetAllAsync();
            throw new NotImplementedException();
        }
    }
}
