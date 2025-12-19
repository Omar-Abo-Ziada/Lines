using Lines.Application.Common;
using Lines.Domain.Enums;
using Lines.Application.Interfaces;
using Lines.Domain.Models.Chats;
using MediatR;

namespace Lines.Application.Features.Messages.DeleteMessagesById.Commands
{
    public record DeleteMessagesByIdAndTypeCommand(Guid MessageId, MessageType MessageType) : IRequest<bool>;

    public class DeleteMessagesByIdAndTypeCommandHandler(RequestHandlerBaseParameters parameters,
            IRepository<ChatMessage> chatMessageRepository,
            IRepository<SupportMsg> supportMessageRepository)
        : RequestHandlerBase<DeleteMessagesByIdAndTypeCommand, bool>(parameters)
    {
    
        public override async Task<bool> Handle(DeleteMessagesByIdAndTypeCommand request, CancellationToken cancellationToken)
        {
            switch (request.MessageType)
            {
                case MessageType.ChatMsg:
                    await chatMessageRepository.DeleteAsync(request.MessageId, cancellationToken);
                    return true;
                case MessageType.SupportMsg:
                    await supportMessageRepository.DeleteAsync(request.MessageId, cancellationToken);
                    return true;
                default:
                    return false;
            }
        }
    }
}