using Lines.Application.Features.Users.GetPassengerAndDriverIdsByUserId.DTOs;
using Lines.Application.Interfaces;

namespace Lines.Application.Features.Users.GetUserById.Queries
{
    public record GetPassengerAndDriverIdsByUserIdQuery(Guid userId) : IRequest<GetPassengerAndDriverIdsDTO?>;

    public class GetPassengerAndDriverIdsByUserIdQueryHandler(RequestHandlerBaseParameters parameters, IApplicationUserService applicationUserService)
                                            : RequestHandlerBase<GetPassengerAndDriverIdsByUserIdQuery, GetPassengerAndDriverIdsDTO?>(parameters)
    {
        public async override Task<GetPassengerAndDriverIdsDTO?> Handle(GetPassengerAndDriverIdsByUserIdQuery request, CancellationToken cancellationToken)
        {
            return await applicationUserService.GetPassengerAndDriverIdsByUserIdAsync(request.userId);
        }
    }


}
