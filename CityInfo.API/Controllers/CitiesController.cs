using AutoMapper;
using CityInfo.API.Entities;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

//using Newtonsoft.Json;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Route("api/cities")]
    [Authorize]
    public class CitiesController : ControllerBase
    {
        private readonly ILogger<CitiesController> logger;
        private readonly IMailService localMail;
        private readonly ICityInfoRepository cityInfoRepository;
        private readonly IMapper mapper;

        private const int maxSize = 20;

        public CitiesController(ILogger<CitiesController> logger, IMailService localMail, ICityInfoRepository cityInfoRepository, IMapper mapper)
        {
            this.logger = logger;
            this.localMail = localMail;
            this.cityInfoRepository = cityInfoRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CityWithoutPointModel>>> GetCities(string? name, string? searchQuery, int pageNumber = 1, int pageSize = 10)
        {
            if (pageSize > maxSize)
            {
                pageSize = maxSize;
            }

            var (cityEntities, pagination) = await cityInfoRepository.GetCitiesAsync(name, searchQuery, pageNumber, pageSize);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagination));

            return Ok(mapper.Map<IEnumerable<CityWithoutPointModel>>(cityEntities));
        }

        [HttpGet("{id}", Name = "GetCity")]
        public async Task<IActionResult> GetCity(int id, bool includePoint = false)
        {
            try
            {
                var city = await cityInfoRepository.GetCityAsync(id, includePoint);

                if (city == null)
                {
                    logger.LogInformation($"this is error there is no city with this id: [{id}]");
                    return NotFound();
                }

                if (includePoint)
                {
                    return Ok(mapper.Map<CityModel>(city));
                }

                return Ok(mapper.Map<CityWithoutPointModel>(city));
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> AddCity(CityCreationModel city)
        {
            var entity = mapper.Map<City>(city);

            cityInfoRepository.AddCity(entity);

            if (!await cityInfoRepository.SaveChangesAsync())
            {
                return BadRequest();
            }

            var output = mapper.Map<CityModel>(entity);

            return CreatedAtRoute("GetCity", new { id = output.Id }, output);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCity(int id, CityUpdate cityUpdate)
        {
            if (!await cityInfoRepository.IsCityExist(id))
            {
                return NotFound();
            }

            var entity = await cityInfoRepository.GetCityAsync(id, true);

            mapper.Map(cityUpdate, entity);

            if (!await cityInfoRepository.SaveChangesAsync())
            {
                return BadRequest();
            }

            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> PartiallyUpdateCity(int id, JsonPatchDocument<CityUpdate> cityPatch)
        {
            if (!await cityInfoRepository.IsCityExist(id))
            {
                return NotFound();
            }

            var entity = await cityInfoRepository.GetCityAsync(id, true);

            var cityToUpdate = new CityUpdate();

            mapper.Map(entity, cityToUpdate);

            cityPatch.ApplyTo(cityToUpdate, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!TryValidateModel(cityToUpdate))
            {
                return BadRequest(ModelState);
            }

            mapper.Map(cityToUpdate, entity);

            if (!await cityInfoRepository.SaveChangesAsync())
            {
                return BadRequest();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCity(int id)
        {
            if (!await cityInfoRepository.IsCityExist(id))
            {
                return NotFound();
            }

            var city = await cityInfoRepository.GetCityAsync(id, false);

            cityInfoRepository.RemoveCity(city);

            if (!await cityInfoRepository.SaveChangesAsync())
            {
                BadRequest();
            }

            localMail.Send($"the city [{city.Name}] with id: ({city.Id}) was removed.", "city deleted");
            return NoContent();
        }
    }
}