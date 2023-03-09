using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Route("api/cities")]
    public class CitiesController : ControllerBase
    {
        private readonly ILogger<CitiesController> logger;
        private readonly IMailService localMail;
        private readonly CitiesDataStore dataStore;

        public CitiesController(ILogger<CitiesController> logger, IMailService localMail, CitiesDataStore dataStore)
        {
            this.logger = logger;
            this.localMail = localMail;
            this.dataStore = dataStore;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CityModel>> GetCities()
        {
            return Ok(dataStore.Cities);
        }

        [HttpGet("{id}", Name = "GetCity")]
        public ActionResult<CityModel> GetCity(int id)
        {
            try
            {
                var city = dataStore.Cities.FirstOrDefault(x => x.Id == id);

                if (city == null)
                {
                    logger.LogInformation($"this is error there is no city with this id: [{id}]");
                    return NotFound();
                }

                return Ok(city);
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public ActionResult AddCity(JsonPatchDocument<CityCreationModel> cityPatch)
        {
            int cityId = dataStore.Cities.Max(c => c.Id);
            cityId += 1;

            var creationCity = new CityCreationModel();

            cityPatch.ApplyTo(creationCity, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (TryValidateModel(creationCity))
            {
                return BadRequest(ModelState);
            }

            var returnedCity = new CityModel
            {
                Id = cityId,
                Name = creationCity.Name,
                Description = creationCity.Description,
                PointsOfInteristsList = creationCity.PointsOfInteristsList
            };

            dataStore.Cities.Add(returnedCity);

            return CreatedAtRoute("GetCity", new { id = returnedCity.Id }, returnedCity);
        }

        [HttpPut("{id}")]
        public ActionResult UpdateCity(int id, CityUpdate cityUpdate)
        {
            var city = dataStore.Cities.FirstOrDefault(c => c.Id == id);
            if (city == null)
            {
                return NotFound();
            }

            city.Name = cityUpdate.Name;
            city.Description = cityUpdate.Description;
            city.PointsOfInteristsList = cityUpdate.PointsOfInteristsList;

            return NoContent();
        }

        [HttpPatch("{id}")]
        public ActionResult PartiallyUpdateCity(int id, JsonPatchDocument<CityUpdate> cityPatch)
        {
            var creationCity = dataStore.Cities.FirstOrDefault(c => c.Id == id);
            if (creationCity == null)
            {
                return NotFound();
            }

            var cityToUpdate = new CityUpdate
            {
                Name = creationCity.Name,
                Description = creationCity.Description,
                PointsOfInteristsList = creationCity.PointsOfInteristsList
            };

            cityPatch.ApplyTo(cityToUpdate, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!TryValidateModel(cityToUpdate))
            {
                return BadRequest(ModelState);
            }

            creationCity.Name = cityToUpdate.Name;
            creationCity.Description = cityToUpdate.Description;
            creationCity.PointsOfInteristsList = cityToUpdate.PointsOfInteristsList;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteCity(int id)
        {
            var city = dataStore.Cities.FirstOrDefault(c => c.Id == id);
            if (city == null)
            {
                return NotFound();
            }

            dataStore.Cities.Remove(city);
            localMail.Send($"the city [{city.Name}] with id: ({city.Id}) was removed.", "city deleted");
            return NoContent();
        }
    }
}