using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lines.Application.Features.Users.UpdateUserEmail.DTOs
{
    public class UpdateUserEmailDto
    {
        public string? CurrentEmail { get; set; }
        public string? NewEmail { get; set; }

    }
}
