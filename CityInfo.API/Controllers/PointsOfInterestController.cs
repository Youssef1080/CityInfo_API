using AutoMapper;
using CityInfo.API.Entities;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CityInfo.API.Controllers
{
    [Route("api/cities/{cityId}/[controller]/v{version:ApiVersion}")]
    [ApiController]
    [Authorize]
    // supported version
    [ApiVersion("2.0")]
    public class PointsOfInterestController : ControllerBase
    {
        private readonly IMailService localMail;
        private readonly IMapper mapper;
        private readonly ICityInfoRepository cityInfoRepository;

        public PointsOfInterestController(IMailService localMail, CitiesDataStore dataStore, IMapper mapper, ICityInfoRepository cityInfoRepository)
        {
            this.localMail = localMail;
            this.mapper = mapper;
            this.cityInfoRepository = cityInfoRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PointsOfInteristModel>>> GetAllPointsOfInterest(int cityId)
        {
            //var cityName = User.Claims.Where(c => c.Type == "city").FirstOrDefault();
            //if (!await cityInfoRepository.IsCityMatched(cityId, cityName.Value))
            //{
            //    return Forbid();
            //}

            if (!await cityInfoRepository.IsCityExist(cityId))
            {
                return NotFound();
            }
            return Ok(mapper.Map<IEnumerable<PointsOfInteristModel>>(await cityInfoRepository.GetPointsOfInterestAsync(cityId)));
        }

        [HttpGet("{id}", Name = "GetPointOfInterest")]
        public async Task<ActionResult<PointsOfInteristModel>> GetPointOfInterest(int cityId, int id)
        {
            //User.Claims.C
            var pointOfInterest = await cityInfoRepository.GetPointOfInterestAsync(cityId, id);

            if (pointOfInterest == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<PointsOfInteristModel>(pointOfInterest));
        }

        [HttpPost]
        public async Task<ActionResult> AddPointOfInterest(PointsOfInterestCreationModel createdPoint, int cityId)
        {
            if (!await cityInfoRepository.IsCityExist(cityId))
            {
                return NotFound();
            }
            var entityPoint = mapper.Map<PointOfInterest>(createdPoint);

            await cityInfoRepository.AddPointOfInterestAsync(cityId, entityPoint);

            if (!await cityInfoRepository.SaveChangesAsync())
            {
                return BadRequest();
            }

            var returnedPoint = mapper.Map<PointsOfInteristModel>(entityPoint);

            return CreatedAtRoute("GetPointOfInterest", new { cityId, id = returnedPoint.Id }, returnedPoint);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdatePointOfInterest(int cityId, int id, PointsOfInterestUpdate updatePoint)
        {
            if (!await cityInfoRepository.IsCityExist(cityId))
            {
                return NotFound();
            }

            var fetchedEntity = await cityInfoRepository.GetPointOfInterestAsync(cityId, id);

            mapper.Map(updatePoint, fetchedEntity);

            //await cityInfoRepository.UpdatePointOfInterestAsync(cityId, id, entity);

            if (!await cityInfoRepository.SaveChangesAsync())
            {
                return BadRequest();
            }

            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> PartiallyUpdatePointOfInterest(int cityId, int id, JsonPatchDocument<PointsOfInterestUpdate> jsonPatch)
        {
            if (!await cityInfoRepository.IsCityExist(cityId))
            {
                return NotFound();
            }

            var entity = await cityInfoRepository.GetPointOfInterestAsync(cityId, id);

            var pointToPatch = new PointsOfInterestUpdate();

            mapper.Map(pointToPatch, entity);

            jsonPatch.ApplyTo(pointToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!TryValidateModel(pointToPatch))
            {
                return BadRequest(ModelState);
            }

            mapper.Map(pointToPatch, entity);

            if (!await cityInfoRepository.SaveChangesAsync())
            {
                return BadRequest();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePointOfInterest(int cityId, int id)
        {
            if (!await cityInfoRepository.IsCityExist(cityId))
            {
                return NotFound();
            }

            var pointOfInterest = await cityInfoRepository.GetPointOfInterestAsync(cityId, id);
            if (pointOfInterest == null)
            {
                return NotFound();
            }

            cityInfoRepository.RemovePointOfInterest(pointOfInterest);

            if (!await cityInfoRepository.SaveChangesAsync())
            {
                BadRequest();
            }

            localMail.Send($"the PointOfInterest [{pointOfInterest.Name}] with id: ({pointOfInterest.Id}) was removed.", "PointOfInterest deleted");

            return NoContent();
        }
    }
}