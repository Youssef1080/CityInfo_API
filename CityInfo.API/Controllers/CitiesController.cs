using CityInfo.API.Models;
using Microsoft.AspNetCore.Http.HttpResults;
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
        public ActionResult AddCity(CityCreationModel cityCreation)
        {
            int cityId = CitiesDataStore.Current.Cities.Max(c => c.Id);
            cityId += 1;

            var returnedCity = new CityModel
            {
                Id = cityId,
                Description = cityCreation.Description,
                Name = cityCreation.Name,
                PointsOfInteristsList = cityCreation.PointsOfInteristsList,
            };

            CitiesDataStore.Current.Cities.Add(returnedCity);

            return CreatedAtRoute("GetCity", new { id = returnedCity.Id }, returnedCity);
        }
    }
}