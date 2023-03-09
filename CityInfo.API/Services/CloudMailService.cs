namespace CityInfo.API.Services
{
    public class CloudMailService : IMailService
    {
        private string mailTo = "example.to@gmail.com";
        private string mailFrom = "example.from@gmail.com";

        public CloudMailService(IConfiguration config)
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