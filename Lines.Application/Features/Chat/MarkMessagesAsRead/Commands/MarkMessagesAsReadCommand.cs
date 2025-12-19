using Lines.Application.Common;
using Lines.Application.Interfaces;
using Lines.Domain.Models.Chats;
using LinqKit;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lines.Application.Features.Chat.MarkMessagesAsRead.Commands
{
    public record MarkMessagesAsReadCommand(Guid TripId, Guid UserId) : IRequest<Unit>;

    public class MarkMessagesAsReadCommandHandler : RequestHandlerBase<MarkMessagesAsReadCommand, Unit>
    {
        private readonly IRepository<ChatMessage> _messageRepository;

        public MarkMessagesAsReadCommandHandler(
            RequestHandlerBaseParameters parameters, 
            IRepository<ChatMessage> messageRepository) : base(parameters)
        {
            _messageRepository = messageRepository;
        }

        public override async Task<Unit> Handle(MarkMessagesAsReadCommand request, CancellationToken cancellationToken)
        {
            // Build predicate to find unread messages for this trip where the user is the recipient
            var predicate = PredicateBuilder.New<ChatMessage>(true);
            predicate = predicate.And(m => m.TripId == request.TripId)
                .And(m => m.RecipientId == request.UserId)
                .And(m => m.IsRead == false)
                .And(m => m.IsDeleted == false);

            // Get unread messages
            var unreadMessages = await _messageRepository.Get(predicate)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            // Mark messages as read
            foreach (var message in unreadMessages)
            {
                message.IsRead = true;
                message.ReadAt = DateTime.UtcNow;
            }

            if (unreadMessages.Any())
            {
                _messageRepository.SaveChanges();
            }

            return Unit.Value;
        }
    }
}
