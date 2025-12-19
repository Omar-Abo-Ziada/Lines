using Lines.Application.Common.Email;

namespace Lines.Application.Interfaces
{
    public interface IEmailService
    {
        (bool Succeeded, string[] Errors) SendMail(MailData mailData);
        Task<(bool Succeeded, string[] Errors)> SendMailAsync(MailData mailData);
    }
}
