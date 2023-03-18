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

        public async Task<(IEnumerable<City>, PaginationMetadata)> GetCitiesAsync(string? name, string? searchQuery, int pageNumber, int pageSize)
        {
            var collection = context.Cities as IQueryable<City>;

            if (!string.IsNullOrEmpty(name))
            {
                name = name.ToLower().Replace(" ", "");

                collection = collection.Where(c => c.Name.ToLower().Replace(" ", "") == name);

                //return await collection.Skip(pageSize * (pageNumber - 1)).Take(pageSize).OrderBy(c => c.Name).ToListAsync();
            }

            if ((!string.IsNullOrEmpty(searchQuery) && !string.IsNullOrEmpty(name)) || !string.IsNullOrEmpty(searchQuery))
            {
                //collection = context.Cities as IQueryable<City>;

                searchQuery = searchQuery.ToLower().Trim();

                collection = collection.Where(c => (c.Name.ToLower().Contains(searchQuery))
                || (c.Description != null && c.Description.ToLower().Contains(searchQuery))
                );
            }

            PaginationMetadata pagination = new PaginationMetadata(pageNumber, await collection.CountAsync(), pageSize);

            var output = await collection.Skip(pageSize * (pageNumber - 1)).Take(pageSize).OrderBy(c => c.Name).ToListAsync();

            return (output, pagination);
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

        public async Task<User> AuthenticateUser(string? username, string? password)
        {
            var user = await context.Users.Where(u => u.UserName == username && u.Password == password).FirstOrDefaultAsync();

            return user;
        }

        public async Task<User> GetUserAsync(int id)
        {
            return await context.Users.Where(u => u.Id == id).FirstOrDefaultAsync();
        }

        public async Task<bool> IsCityMatched(int id, string cityName)
        {
            return await context.Cities.AnyAsync(c => c.Id == id && c.Name == cityName);
        }
    }
}