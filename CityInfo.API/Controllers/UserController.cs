using AutoMapper;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Runtime;

namespace CityInfo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ICityInfoRepository cityInfo;
        private readonly IMapper mapper;

        public UserController(ICityInfoRepository cityInfo, IMapper mapper)
        {
            this.cityInfo = cityInfo;
            this.mapper = mapper;
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PartiallyUpdateUser(JsonPatchDocument<UserUpdateModel> userPatch, int id)
        {
            var entity = await cityInfo.GetUserAsync(id);
            if (entity == null)
            {
                return NotFound();
            }
            UserUpdateModel userUpdate = new UserUpdateModel();

            mapper.Map(entity, userUpdate);

            userPatch.ApplyTo(userUpdate);

            //var cityUser = new CityInfoUser();

            // mapper.Map(userUpdate, cityUser);

            mapper.Map(userUpdate, entity);

            if (!await cityInfo.SaveChangesAsync())
            {
                return BadRequest();
            }

            return NoContent();
        }
    }
}