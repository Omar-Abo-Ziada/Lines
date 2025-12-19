using Lines.Application.Common.Email;
using Lines.Application.Interfaces;
using Lines.Application.Shared;
using Lines.Infrastructure.Configurations;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Net;

namespace Lines.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly MailSettings _mailSettings;

        public EmailService(IOptions<MailSettings> mailSettingsOptions)
        {
            _mailSettings = mailSettingsOptions.Value;
        }

        public (bool Succeeded, string[] Errors) SendMail(MailData mailData)
        {
            try
            {
                using (MimeMessage emailMessage = new MimeMessage())
                {
                    var emailFrom = new MailboxAddress(_mailSettings.SenderName, _mailSettings.SenderEmail);
                    emailMessage.From.Add(emailFrom);

                    var emailTo = new MailboxAddress(mailData.ToName, mailData.ToEmail);
                    emailMessage.To.Add(emailTo);

                    emailMessage.Subject = mailData.Subject;

                    var emailBodyBuilder = new BodyBuilder
                    {
                        TextBody = mailData.Format.ToString()
                    };
                    emailMessage.Body = emailBodyBuilder.ToMessageBody();

                    using (var mailClient = new SmtpClient())
                    {
                        mailClient.Connect(_mailSettings.Server, _mailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                        mailClient.Authenticate(new NetworkCredential(_mailSettings.SenderEmail, _mailSettings.Password));

                        mailClient.Send(emailMessage);
                        mailClient.Disconnect(true);
                    }
                }

                return (true, Array.Empty<string>());
            }
            catch (Exception ex)
            {
                return (false, new[] { ex?.InnerException?.Message ?? Error.General.ToString()});
            }
        }

        public async Task<(bool Succeeded, string[] Errors)> SendMailAsync(MailData mailData)
        {
            try
            {
                using (MimeMessage emailMessage = new MimeMessage())
                {
                    var emailFrom = new MailboxAddress(_mailSettings.SenderName, _mailSettings.SenderEmail);
                    emailMessage.From.Add(emailFrom);

                    var emailTo = new MailboxAddress(mailData.ToName, mailData.ToEmail);
                    emailMessage.To.Add(emailTo);

                    emailMessage.Subject = mailData.Subject;

                    var emailBodyBuilder = new BodyBuilder
                    {
                        HtmlBody = mailData.Format.ToString()
                    };
                    emailMessage.Body = emailBodyBuilder.ToMessageBody();

                    using (var mailClient = new SmtpClient())
                    {
                        await mailClient.ConnectAsync(_mailSettings.Server, _mailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                        await mailClient.AuthenticateAsync(new NetworkCredential(_mailSettings.SenderEmail, _mailSettings.Password));

                        await mailClient.SendAsync(emailMessage);
                        await mailClient.DisconnectAsync(true);
                    }
                }

                return (true , Array.Empty<string>());
            }
            catch (Exception ex)
            {
                return (false , new[] { ex?.InnerException?.Message?? Error.General.ToString()});
            }
        }

    }
}
