using Lines.Application.DTOs;
using Lines.Application.Shared;

namespace Lines.Presentation.Endpoints.Chat.GetMessages
{
    public class GetMessagesResponse
    {
        public PagingDto<MessageDto> Messages { get; set; } = new();
    }
}
