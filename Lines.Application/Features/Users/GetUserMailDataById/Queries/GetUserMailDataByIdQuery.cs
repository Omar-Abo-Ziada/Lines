using Lines.Application.Features.Users.GetUserMailDataById.DTOs;
using Lines.Application.Interfaces;

namespace Lines.Application.Features.Users.GetUserMailDataById.Queries
{
    public record GetUserMailDataByIdQuery(Guid userId) : IRequest<UserMailDataDto?>;


        public class GetUserMailDataByIdQueryHandler(IApplicationUserService applicationUserService, RequestHandlerBaseParameters parameters) 
            : RequestHandlerBase<GetUserMailDataByIdQuery, UserMailDataDto?>(parameters)
         {
              public async override Task<UserMailDataDto?> Handle(GetUserMailDataByIdQuery request, CancellationToken cancellationToken)
                  {
                       return await applicationUserService.GetMailDataByIdAsync(request.userId);
                  }
         }
}
