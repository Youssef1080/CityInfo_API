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

        [HttpGet("{id}")]
        public ActionResult<CityModel> GetCity(int id)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(x => x.Id == id);

            if (city == null)
            {
                return NotFound();
            }

            return Ok(city);
        }
    }
}