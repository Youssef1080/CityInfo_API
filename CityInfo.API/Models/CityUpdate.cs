namespace CityInfo.API.Models
{
    public class CityUpdate
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int NumberOfPointsOfInterist => PointsOfInteristsList.Count;
        public List<PointsOfInteristModel> PointsOfInteristsList { get; set; } = new List<PointsOfInteristModel>();
    }
}