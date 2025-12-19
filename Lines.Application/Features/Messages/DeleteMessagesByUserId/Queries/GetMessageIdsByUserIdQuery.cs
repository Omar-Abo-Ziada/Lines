using Lines.Application.Common;
using Lines.Application.Interfaces;
using Lines.Domain.Models.Chats;
using MediatR;
using System.Data.Entity;

namespace Lines.Application.Features.Messages.DeleteMessagesByUserId.Queries
{
    public record GetMessageIdsByUserIdQuery(Guid UserId) : IRequest<List<Guid>>;


    public class GetMessageIdsByUserIdHandler(
       RequestHandlerBaseParameters parameters,
       IRepository<ChatMessage> chatMessageRepository,
       IRepository<SupportMsg> supportMsgRepository)
       : RequestHandlerBase<GetMessageIdsByUserIdQuery, List<Guid>>(parameters)
    {
        public override async Task<List<Guid>> Handle(GetMessageIdsByUserIdQuery request, CancellationToken cancellationToken)
        {
            //var chatMessageIds = await chatMessageRepository
            //    .Get(m => m.SenderId == request.UserId || m.RecipientId == request.UserId)
            //    .Select(m => m.Id)
            //    .ToListAsync(cancellationToken);

            //var supportMessageIds = await supportMsgRepository
            //    .Get(m => m.SenderId == request.UserId || m.RecipientId == request.UserId)
            //    .Select(m => m.Id)
            //    .ToListAsync(cancellationToken);

            //return chatMessageIds.Concat(supportMessageIds).ToList();
            return default;
        }
    }
}
