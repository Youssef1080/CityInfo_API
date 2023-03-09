namespace CityInfo.API.Services
{
    public interface IMailService
    {
        void Send(string message, string subject);
    }
}