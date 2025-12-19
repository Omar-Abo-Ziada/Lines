using Lines.Application.Common;
using Lines.Application.Features.Messages.DeleteMessagesById.Commands;
using Lines.Domain.Enums;
using Lines.Application.Interfaces;
using Lines.Domain.Models.Chats;
using MediatR;

namespace Lines.Application.Features.Messages.DeleteMessagesByUserId.Commands
{
    public record DeleteMessagesByUserIdCommand(Guid UserId) : IRequest<bool>;

    public class DeleteMessagesByUserIdCommandHandler(
         RequestHandlerBaseParameters parameters,
         IRepository<ChatMessage> chatMessageRepository,
         IRepository<SupportMsg> supportMessageRepository)
         : RequestHandlerBase<DeleteMessagesByUserIdCommand, bool>(parameters)
    {
        private readonly IRepository<ChatMessage> _chatMessageRepository = chatMessageRepository;
        private readonly IRepository<SupportMsg> _supportMessageRepository = supportMessageRepository;

        public override async Task<bool> Handle(DeleteMessagesByUserIdCommand request, CancellationToken cancellationToken)
        {
            // Delete ChatMessages where user is sender or recipient
            //var chatMessages = _chatMessageRepository.Get(x => x.SenderId == request.UserId || x.RecipientId == request.UserId).ToList();
            //foreach (var message in chatMessages)
            //{
            //    await _mediator.Send(new DeleteMessagesByIdAndTypeCommand(message.Id, MessageType.ChatMsg), cancellationToken);
            //}

            //// Delete SupportMessages where user is sender or recipient
            //var supportMessages = _supportMessageRepository.Get(x => x.SenderId == request.UserId || x.RecipientId == request.UserId).ToList();
            //foreach (var message in supportMessages)
            //{
            //    await _mediator.Send(new DeleteMessagesByIdAndTypeCommand(message.Id, MessageType.SupportMsg), cancellationToken);
            //}

            return true;
        }
    }
} 