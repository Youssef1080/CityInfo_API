using CityInfo.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [Route("api/cities/{cityId}/[controller]")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<PointsOfInteristModel>> GetAllPointsOfInterest(int cityId)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(x => x.Id == cityId);

            if (city == null)
            {
                return NotFound();
            }

            return Ok(city?.PointsOfInteristsList);
        }

        [HttpGet("{id}", Name = "GetPointOfInterest")]
        public ActionResult<PointsOfInteristModel> GetPointOfInterest(int cityId, int id)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(x => x.Id == cityId);

            if (city == null)
            {
                return NotFound();
            }

            var pointOfInterest = city?.PointsOfInteristsList.FirstOrDefault(x => x.Id == id);

            if (pointOfInterest == null)
            {
                return NotFound();
            }

            return Ok(pointOfInterest);
        }

        [HttpPost]
        public ActionResult AddPointOFInterest(PointsOfInterestCreationModel createdPoint, int cityId)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(x => x.Id == cityId);

            if (city == null)
            {
                return NotFound();
            }

            int pointId = city.PointsOfInteristsList.Max(p => p.Id);

            pointId += 1;

            var returnedPoint = new PointsOfInteristModel
            {
                Id = pointId,
                Description = createdPoint.Description,
                Name = createdPoint.Name,
            };

            city.PointsOfInteristsList.Add(returnedPoint);

            return CreatedAtRoute("GetPointOfInterest", new { cityId, id = returnedPoint.Id }, returnedPoint);
        }

        [HttpPut("{id}")]
        public ActionResult UpdatePointOfInterest(int cityId, int id, PointsOfInterestUpdate updatePoint)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);

            if (city == null)
            {
                return NotFound();
            }

            var pointOfInterest = city.PointsOfInteristsList.FirstOrDefault(p => p.Id == id);

            if (pointOfInterest == null)
            {
                return NotFound();
            }

            pointOfInterest.Name = updatePoint.Name;
            pointOfInterest.Description = updatePoint.Description;

            return NoContent();
        }

        [HttpPatch("{id}")]
        public ActionResult PartiallyUpdatePointOfInterest(int cityId, int id, JsonPatchDocument<PointsOfInterestUpdate> jsonPatch)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            var pointOfInterest = city.PointsOfInteristsList.FirstOrDefault(p => p.Id == id);
            if (pointOfInterest == null)
            {
                return NotFound();
            }

            var pointToPatch = new PointsOfInterestUpdate
            {
                Name = pointOfInterest.Name,
                Description = pointOfInterest.Description
            };

            jsonPatch.ApplyTo(pointToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!TryValidateModel(pointToPatch))
            {
                return BadRequest(ModelState);
            }

            pointOfInterest.Name = pointToPatch.Name;
            pointOfInterest.Description = pointToPatch.Description;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeletePointOfInterest(int cityId, int id)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            var pointOfInterest = city.PointsOfInteristsList.FirstOrDefault(p => p.Id == id);
            if (pointOfInterest == null)
            {
                return NotFound();
            }

            city.PointsOfInteristsList.Remove(pointOfInterest);

            return NoContent();
        }
    }
}