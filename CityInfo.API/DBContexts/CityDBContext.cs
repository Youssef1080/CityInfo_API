using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.DBContexts
{
    public class CityDBContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>()
                .HasData(
                    new City("New York lbn")
                    {
                        Id = 1,
                        Description = "good lbn in new york",
                    },
                    new City("New Capital lbn")
                    {
                        Id = 2,
                        Description = "good lbn in new Capital"
                    }
                );

            modelBuilder.Entity<PointOfInterest>()
                .HasData(
                    new PointOfInterest("samk")
                    {
                        Id = 1,
                        Description = "good samk in new york",
                        CityId = 1
                    },
                    new PointOfInterest("new barkoof lbn")
                    {
                        Id = 2,
                        Description = "good lbn in new barkoof",
                        CityId = 1,
                    }
                );
        }

        public CityDBContext(DbContextOptions<CityDBContext> options)
            : base(options)
        {
        }

        public DbSet<City> Cities { get; set; }
        public DbSet<PointOfInterest> PointOfInterests { get; set; }
    }
}