using Lines.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lines.Application.Features.Messages.DeleteMessagesByTripId.DTOs
{
    public class GetMessageIdsAndTypesByTripIdDto
    {
        public Guid Id { get; set; }
        public MessageType Type { get; set; }

    }
}
