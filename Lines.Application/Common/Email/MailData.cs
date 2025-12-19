namespace Lines.Application.Common.Email
{
    public class MailData
    {
        public MailData(string toEmail , string toName , string subject , string format)
        {
            ToEmail = toEmail;
            ToName = toName;
            Subject = subject;
            Format = format;
        }
        public string ToEmail { get; private set; }

        public string ToName { get; private set; }

        public string Subject { get; private set; }

        public string Format { get; private set; }
    }
}
