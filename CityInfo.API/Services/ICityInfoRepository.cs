using CityInfo.API.Entities;

namespace CityInfo.API.Services
{
    public interface ICityInfoRepository
    {
        public Task<IEnumerable<City>> GetCitiesAsync();

        public Task<City?> GetCityAsync(int cityId, bool getPoint);

        public Task<IEnumerable<PointOfInterest>> GetPointsOfInterestAsync(int cityId);

        public Task<PointOfInterest?> GetPointOfInterestAsync(int cityId, int pointId);
    }
}