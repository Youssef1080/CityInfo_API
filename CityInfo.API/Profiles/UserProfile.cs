using AutoMapper;
using CityInfo.API.Entities;
using CityInfo.API.Models;

namespace CityInfo.API.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, CityInfoUser>();
            CreateMap<CityInfoUser, User>();
            CreateMap<User, UserUpdateModel>();
            CreateMap<UserUpdateModel, User>();
        }
    }
}