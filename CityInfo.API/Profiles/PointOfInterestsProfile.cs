using AutoMapper;
using CityInfo.API.Entities;
using CityInfo.API.Models;

namespace CityInfo.API.Profiles
{
    public class PointOfInterestsProfile : Profile
    {
        public PointOfInterestsProfile()
        {
            CreateMap<PointOfInterest, PointsOfInteristModel>();
            CreateMap<PointsOfInterestCreationModel, PointOfInterest>();
            CreateMap<PointsOfInteristModel, PointOfInterest>();
            CreateMap<PointsOfInterestUpdate, PointOfInterest>();
            CreateMap<PointOfInterest, PointsOfInterestUpdate>();
        }
    }
}