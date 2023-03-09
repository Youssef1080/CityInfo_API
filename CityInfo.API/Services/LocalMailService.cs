namespace CityInfo.API.Services
{
    public class LocalMailService : IMailService
    {
        private string mailTo = "";
        private string mailFrom = "";

        public LocalMailService(IConfiguration config)
        {
            mailTo = config["mailSettings:mailToAddress"];
            mailFrom = config["mailSettings:mailFromAddress"];
        }

        public void Send(string message, string subject)
        {
            Console.WriteLine($"Mail From {mailFrom} to {mailTo}, " +
                $"with {nameof(LocalMailService)}.");
            Console.WriteLine($"Subject: {subject}");
            Console.WriteLine($"Message: {message}");
        }
    }
}