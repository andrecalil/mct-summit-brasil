namespace Site
{
    public class SendMailOptions
    {
        public SendMailOptions()
        {

        }

        public string MailgunAPIKey { get; set; }
        public string MailgunBaseURL { get; set; }
        public string MailgunResourceURL { get; set; }
        public string EmailTo { get; set; }
    }
}