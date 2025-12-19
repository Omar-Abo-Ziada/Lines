using Lines.Application.DTOs;
using Lines.Application.Extensions;
using Lines.Application.Interfaces;
using Lines.Domain.Models.Chats;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace Lines.Application.Features.Chat.GetMessages.Queries
{
    public record GetMessagesQuery(Guid TripId, Guid UserId, int PageNumber = 1, int PageSize = 50) : IRequest<Result<PagingDto<MessageDto>>>;

    public class GetMessagesQueryHandler : RequestHandlerBase<GetMessagesQuery, Result<PagingDto<MessageDto>>>
    {
        private readonly IRepository<ChatMessage> _messageRepository;
        private readonly IRepository<Trip> _tripRepository;

        public GetMessagesQueryHandler(
            RequestHandlerBaseParameters parameters, 
            IRepository<ChatMessage> messageRepository,
            IRepository<Trip> tripRepository) : base(parameters)
        {
            _messageRepository = messageRepository;
            _tripRepository = tripRepository;
        }

        public override async Task<Result<PagingDto<MessageDto>>> Handle(GetMessagesQuery request, CancellationToken cancellationToken)
        {

                // First, verify that the user is eligible to view messages for this trip
                var tripPredicate = PredicateBuilder.New<Trip>(true);
                tripPredicate = tripPredicate.And(t => t.Id == request.TripId)
                    .And(t => t.DriverId == request.UserId || t.PassengerId == request.UserId)
                    .And(t => t.IsDeleted == false);

                var tripExists = await _tripRepository.Get(tripPredicate)
                    .AnyAsync(cancellationToken)
                    .ConfigureAwait(false);

                if (!tripExists)
                {
                    return Result<PagingDto<MessageDto>>.Failure(ChatErrors.TripDoesNotExist("Trip Not Found"));
                }

                // Build predicate for messages
                var messagePredicate = PredicateBuilder.New<ChatMessage>(true);
                messagePredicate = messagePredicate.And(m => m.TripId == request.TripId)
                    .And(m => m.IsDeleted == false);

                // Get messages with pagination
                var query = _messageRepository.Get(messagePredicate)
                    .OrderBy(m => m.CreatedDate)
                    .Select(m => new MessageDto
                    {
                        Id = m.Id,
                        TripId = m.TripId,
                        Content = m.Content,
                        ReadAt = m.ReadAt,
                        IsRead = m.IsRead,
                        SenderId = m.SenderId,
                        RecipientId = m.RecipientId,
                        CreatedDate = m.CreatedDate
                    });

                var pagedResult = await query.ToPagedListAsync(request.PageNumber, request.PageSize, cancellationToken);

                return Result<PagingDto<MessageDto>>.Success(pagedResult);
            
        }
    }
}
