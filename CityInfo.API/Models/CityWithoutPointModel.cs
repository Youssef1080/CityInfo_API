namespace CityInfo.API.Models
{
    public class CityWithoutPointModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string? Description { get; set; }
    }
}