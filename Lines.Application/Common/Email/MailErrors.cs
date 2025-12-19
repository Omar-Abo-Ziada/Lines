using Lines.Application.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lines.Application.Common.Email
{
    public static class MailErrors
    {
        public static Error SendMailError(string desc) => new Error("Mail.Send_Mail_ERROR", desc, ErrorType.Failure);

    }
}
