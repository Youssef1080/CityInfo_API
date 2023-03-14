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

        public async Task<City?> GetCityAsync(int cityId, bool getPoint)
        {
            if (getPoint)
            {
                return await context.Cities.Include(c => c.PointsOfInteristsList).Where(c => c.Id == cityId).FirstOrDefaultAsync();
            }

            return await context.Cities.Where(c => c.Id == cityId).FirstOrDefaultAsync();
        }

        public async Task<PointOfInterest?> GetPointOfInterestAsync(int cityId, int pointId)
        {
            return await context.PointOfInterests.Where(p => p.CityId == cityId && p.Id == pointId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<PointOfInterest>> GetPointsOfInterestAsync(int cityId)
        {
            return await context.PointOfInterests.Where(p => p.CityId == cityId).ToListAsync();
        }
    }
}