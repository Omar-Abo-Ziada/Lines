using Lines.Application.Common;
using Lines.Application.Features.Messages.DeleteMessagesByTripId.DTOs;
using Lines.Application.Interfaces;
using Lines.Domain.Models.Chats;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore; 

namespace Lines.Application.Features.Messages.DeleteMessagesByTripId.Queries
{
    public record GetMessageIdsAndTypesByTripIdQuery(Guid TripId) : IRequest<List<GetMessageIdsAndTypesByTripIdDto>>;

    public class GetMessageIdsByTripIdQueryHandler
        : RequestHandlerBase<GetMessageIdsAndTypesByTripIdQuery, List<GetMessageIdsAndTypesByTripIdDto>>
    {
        private readonly IRepository<ChatMessage> _chatMessageRepository;

        public GetMessageIdsByTripIdQueryHandler(
            RequestHandlerBaseParameters parameters,
            IRepository<ChatMessage> chatMessageRepository
        ) : base(parameters)
        {
            _chatMessageRepository = chatMessageRepository;
        }

        public override async Task<List<GetMessageIdsAndTypesByTripIdDto>> Handle(
            GetMessageIdsAndTypesByTripIdQuery request,
            CancellationToken cancellationToken)
        {
            var chatMessageIds = await _chatMessageRepository
                .Get(c => c.TripId == request.TripId)
                .Select(x => new {x.Id , x.MessageType})
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            var messages = chatMessageIds.Adapt<List<GetMessageIdsAndTypesByTripIdDto>>();

            return messages;
        }
    }
}
