using CityInfo.API.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Route("api/cities")]
    public class CitiesController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<CityModel>> GetCities()
        {
            return Ok(CitiesDataStore.Current.Cities);
        }

        [HttpGet("{id}", Name = "GetCity")]
        public ActionResult<CityModel> GetCity(int id)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(x => x.Id == id);

            if (city == null)
            {
                return NotFound();
            }

            return Ok(city);
        }

        [HttpPost]
        public ActionResult AddCity(JsonPatchDocument<CityCreationModel> cityPatch)
        {
            int cityId = CitiesDataStore.Current.Cities.Max(c => c.Id);
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

            CitiesDataStore.Current.Cities.Add(returnedCity);

            return CreatedAtRoute("GetCity", new { id = returnedCity.Id }, returnedCity);
        }

        [HttpPut("{id}")]
        public ActionResult UpdateCity(int id, CityUpdate cityUpdate)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == id);
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
            var creationCity = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == id);
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
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == id);
            if (city == null)
            {
                return NotFound();
            }

            CitiesDataStore.Current.Cities.Remove(city);

            return NoContent();
        }
    }
}