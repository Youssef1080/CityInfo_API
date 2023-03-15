namespace CityInfo.API.Models
{
    public class CityCreationModel
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int NumberOfPointsOfInterist => PointsOfInteristsList.Count;
        public List<PointsOfInterestCreationModel> PointsOfInteristsList { get; set; } = new List<PointsOfInterestCreationModel>();
    }
}