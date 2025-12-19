using Lines.Application.Common;
using Lines.Application.Interfaces;
using Lines.Domain.Models.Chats;
using MediatR;

namespace Lines.Application.Features.Chat.SendImage.Commands
{
    public record AddImageMessageCommand(Guid SenderID, Guid RecipientID, Guid TripID, string Content, string ImagePath) : IRequest<Guid>;

    public class AddImageMessageCommandHandler : RequestHandlerBase<AddImageMessageCommand, Guid>
    {
        private readonly IRepository<ChatMessage> _repository;
        public AddImageMessageCommandHandler(RequestHandlerBaseParameters parameters, IRepository<ChatMessage> repository) : base(parameters)
        {
            _repository = repository;
        }

        public override async Task<Guid> Handle(AddImageMessageCommand request, CancellationToken cancellationToken)
        {
            var entity = _repository.Add(new ChatMessage(request.TripID , request.SenderID , request.RecipientID 
                                        , request.Content , request.ImagePath));
            _repository.SaveChanges();
            return entity.Id;
        }
    }
}
