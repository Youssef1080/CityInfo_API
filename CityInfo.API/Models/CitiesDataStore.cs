namespace CityInfo.API.Models
{
    public class CitiesDataStore
    {
        public List<CityModel> Cities { get; set; } = new List<CityModel>
        {
            new CityModel
            {
                Id = 1, Name = "Cairo", Description = "desc mesc fesc",
                PointsOfInteristsList = new List<PointsOfInteristModel>
                {
                   new PointsOfInteristModel{ Id = 1, Name = "Halawani", Description="good Halawani" },
                   new PointsOfInteristModel{ Id = 2, Name = "SAMKS", Description="good SAMKS" },
                }
            },
            new CityModel
            {
                Id = 2, Name = "Mario", Description = "desc mesc fesc",
                PointsOfInteristsList = new List<PointsOfInteristModel>
                {
                   new PointsOfInteristModel{ Id = 1, Name = "shobokshi", Description="good shobokshi" },
                   new PointsOfInteristModel{ Id = 2, Name = "Barbeque", Description="good Barbeque" },
                }
            },
            new CityModel
            {
                Id = 3, Name = "Qaliub", Description = "desc mesc fesc",
                PointsOfInteristsList = new List<PointsOfInteristModel>
                {
                   new PointsOfInteristModel{ Id = 1, Name = "Hanti", Description="good hanti" },
                   new PointsOfInteristModel{ Id = 2, Name = "Kanti", Description="good kanti" },
                }
            },
        };

        //public static CitiesDataStore Current { get; set; } = new CitiesDataStore();
    }
}