namespace CityInfo.API.Models
{
    public class CityInfoUser
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }

        public CityInfoUser(int id, string userName, string firstName, string lastName, string city, string password)
        {
            UserName = userName;
            FirstName = firstName;
            LastName = lastName;
            City = city;
            Password = password;
            Id = id;
        }
    }
}