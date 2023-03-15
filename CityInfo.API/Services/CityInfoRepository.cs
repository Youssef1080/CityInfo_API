using CityInfo.API.DBContexts;
using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.Services
{
    public class CityInfoRepository : ICityInfoRepository
    {
        private readonly CityDBContext context;

        public CityInfoRepository(CityDBContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<City>> GetCitiesAsync()
        {
            return await context.Cities.OrderBy(c => c.Name).ToListAsync();
        }

        public async Task<IEnumerable<City>> GetCitiesAsync(string? name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return await GetCitiesAsync();
            }

            name = name.ToLower().Replace(" ", "");

            return await context.Cities.Where(c => c.Name.ToLower().Replace(" ", "") == name).OrderBy(c => c.Name).ToListAsync();
        }

        public async Task<City?> GetCityAsync(int cityId, bool getPoint)
        {
            if (getPoint)
            {
                return await context.Cities.Include(c => c.PointsOfInteristsList).Where(c => c.Id == cityId).FirstOrDefaultAsync();
            }

            return await context.Cities.Where(c => c.Id == cityId).FirstOrDefaultAsync();
        }

        public void AddCity(City city)
        {
            context.Cities.Add(city);
        }

        public void RemoveCity(City city)
        {
            context.Cities.Remove(city);
        }

        public async Task<PointOfInterest?> GetPointOfInterestAsync(int cityId, int pointId)
        {
            return await context.PointOfInterests.Where(p => p.CityId == cityId && p.Id == pointId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<PointOfInterest>> GetPointsOfInterestAsync(int cityId)
        {
            return await context.PointOfInterests.Where(p => p.CityId == cityId).ToListAsync();
        }

        public async Task AddPointOfInterestAsync(int cityId, PointOfInterest point)
        {
            var city = await GetCityAsync(cityId, false);

            city?.PointsOfInteristsList.Add(point);
        }

        public void RemovePointOfInterest(PointOfInterest point)
        {
            context.PointOfInterests.Remove(point);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await context.SaveChangesAsync() >= 0);
        }

        public async Task<bool> IsCityExist(int cityId)
        {
            return await context.Cities.AnyAsync(c => c.Id == cityId);
        }
    }
}