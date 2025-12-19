using Lines.Application.Interfaces;
using Lines.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Lines.Infrastructure.Services
{
    public class TwilioService : ISmsService
    {
        private readonly TwilioSettings _twilioSettings;
        public TwilioService(IOptions<TwilioSettings> twilioSettings)
        {
            _twilioSettings = twilioSettings.Value;
        }
        public Task SendSmsAsync(string toPhoneNumber, string message)
        {
            TwilioClient.Init(_twilioSettings.AccountSID, _twilioSettings.AuthToken);

            MessageResource result = MessageResource.Create(body: message,
                                                                    from: new Twilio.Types.PhoneNumber(_twilioSettings.TwilioPhoneNumber),
                                                                    to: toPhoneNumber);
            return Task.CompletedTask;  
        }
    }
}
