using AutoMapper;
using CityInfo.API.Entities;
using CityInfo.API.Models;

namespace CityInfo.API.Profiles
{
    public class CityProfile : Profile
    {
        public CityProfile()
        {
            CreateMap<City, CityWithoutPointModel>();
            CreateMap<City, CityModel>();
            CreateMap<CityCreationModel, City>();
            CreateMap<CityUpdate, City>();
            CreateMap<City, CityUpdate>();
        }
    }
}