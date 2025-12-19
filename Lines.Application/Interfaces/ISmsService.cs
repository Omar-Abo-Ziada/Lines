namespace Lines.Application.Interfaces
{
    public interface ISmsService
    {
        public Task SendSmsAsync(string toPhoneNumber, string message);
    }
}
