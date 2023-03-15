using CityInfo.API.Entities;

namespace CityInfo.API.Services
{
    public interface ICityInfoRepository
    {
        public Task<IEnumerable<City>> GetCitiesAsync();

        public Task<City?> GetCityAsync(int cityId, bool getPoint);

        public Task<IEnumerable<City>> GetCitiesAsync(string? name);

        public void AddCity(City city);

        public void RemoveCity(City city);

        public Task<IEnumerable<PointOfInterest>> GetPointsOfInterestAsync(int cityId);

        public Task<PointOfInterest?> GetPointOfInterestAsync(int cityId, int pointId);

        public Task AddPointOfInterestAsync(int cityId, PointOfInterest point);

        public void RemovePointOfInterest(PointOfInterest point);

        public Task<bool> SaveChangesAsync();

        public Task<bool> IsCityExist(int cityId);
    }
}