using Lines.Application.Common;
using Lines.Application.Interfaces;
using Lines.Domain.Models.Users;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lines.Application.Features.EmergencyNumbers.DeleteEmergencyNumbersByUserId.Commands
{
    public record DeleteEmergencyNumbersByUserIdCommand(Guid UserId) : IRequest<bool>;


    public class DeleteEmergencyNumbersByUserIdCommandHandler(
         RequestHandlerBaseParameters parameters,
         IRepository<EmergencyNumber> repository)
         : RequestHandlerBase<DeleteEmergencyNumbersByUserIdCommand, bool>(parameters)
    {
        private readonly IRepository<EmergencyNumber> _repository = repository;

        public override async Task<bool> Handle(DeleteEmergencyNumbersByUserIdCommand request, CancellationToken cancellationToken)
        {
            var emergencyNumbers = _repository.Get(x => x.UserId == request.UserId).ToList();

            foreach (var number in emergencyNumbers)
            {
                number.IsDeleted = true; 
                await _repository.UpdateAsync(number, cancellationToken);
            }

            return true;
        }
    }
}
