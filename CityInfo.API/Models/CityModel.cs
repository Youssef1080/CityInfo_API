namespace CityInfo.API.Models
{
    public class CityModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string? Description { get; set; }
        public int NumberOfPointsOfInterist => PointsOfInteristsList.Count;
        public List<PointsOfInteristModel> PointsOfInteristsList { get; set; } = new List<PointsOfInteristModel>();
    }
}