using Lines.Application.Interfaces;


namespace Lines.Application.Features.Users.GetUserIdByMail.Queries;

public record GetUserIdByMailQuery(string email) : IRequest<Guid?>;

public class GetUserIdByMailQueryHandler(RequestHandlerBaseParameters parameters, IApplicationUserService applicationUserService) : RequestHandlerBase<GetUserIdByMailQuery, Guid?>(parameters)
{
    public async override Task<Guid?> Handle(GetUserIdByMailQuery request, CancellationToken cancellationToken)
    {
        return await applicationUserService.GetUserIdByMailAsync(request.email);
    }
}