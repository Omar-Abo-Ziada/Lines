using Lines.Application.Features.EmergencyNumbers.Shared.DTOs;
using Lines.Domain.Models.Users;

namespace Lines.Application.Features.EmergencyNumbers.GetEmergencyNumberById.Queries
{
    public record GetEmergencyNumberByIdQuery(Guid Id) : IRequest<EmergencyNumber>;


    public class GetEmergencyNumberByIdQueryHandler(RequestHandlerBaseParameters parameters , IRepository<EmergencyNumber> _repository)
        : RequestHandlerBase<GetEmergencyNumberByIdQuery, EmergencyNumber>(parameters)
    {
        public override async Task<EmergencyNumber> Handle(GetEmergencyNumberByIdQuery request, CancellationToken cancellationToken)
        {
           var emergencyNumber = await _repository.GetByIdAsync(request.Id, cancellationToken);
           return emergencyNumber;
        }
    }

}
