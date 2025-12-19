using Lines.Domain.Models.Chats;

namespace Lines.Application.Features.Chat.SendMessage.Commands
{
    public record AddMessageCommand(Guid SenderID, Guid RecipientID, Guid TripID, string Content) : IRequest<Guid>;

    public class AddMessageCommandHandler : RequestHandlerBase<AddMessageCommand, Guid>
    {
        private readonly IRepository<ChatMessage> _repository;
        public AddMessageCommandHandler(RequestHandlerBaseParameters parameters, IRepository<ChatMessage> repository) : base(parameters)
        {
            _repository = repository;
        }

        public override async Task<Guid> Handle(AddMessageCommand request, CancellationToken cancellationToken)
        {
            var entity= _repository.Add(new ChatMessage(request.TripID , request.SenderID 
                                        , request.RecipientID , request.Content));
            
            _repository.SaveChanges();
            return entity.Id;
        }
    }
}
