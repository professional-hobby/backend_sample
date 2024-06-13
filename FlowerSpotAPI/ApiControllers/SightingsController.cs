using FlowerSpotAPI.CustomAttributes;
using FlowerSpotCore.InterfacesServices;
using FlowerSpotCore.ModelsEndpoints.Sightings;
using Microsoft.AspNetCore.Mvc;

namespace FlowerSpotAPI.ApiControllers
{
    [CustomAuthorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SightingsController : ControllerBase
    {
        private ISightingsService _sightingsService;

        public SightingsController(ISightingsService SightingsService)
        {
            _sightingsService = SightingsService;
        }

        [Route("GetSighting")]
        [HttpPost]
        public JsonResult GetSighting(GetSightingModel Sighting)
        {
            return new JsonResult(_sightingsService.GetSighting(Sighting));
        }

        [Route("GetSightingsForFlowerId")]
        [HttpPost]
        public JsonResult GetSightingsForFlowerId(GetSightingsForFlowerIdModel Flower)
        {
            return new JsonResult(_sightingsService.GetSightingsForFlowerId(Flower));
        }

        [Route("GetSightingsForFlowerName")]
        [HttpPost]
        public JsonResult GetSightingsForFlowerName(GetSightingsForFlowerNameModel Flower)
        {
            return new JsonResult(_sightingsService.GetSightingsForFlowerName(Flower));
        }

        [Route("AddSighting")]
        [HttpPost]
        public async Task<JsonResult> AddSightingAsync(AddSightingModel Sighting)
        {
            int? userID = HttpContext.Items.ContainsKey("UserID") ? (int?)HttpContext.Items["UserID"] : -1;
            return new JsonResult(await _sightingsService.AddSightingAsync(userID != null ? userID.Value : -1, Sighting));
        }

        [Route("DeleteSighting")]
        [HttpPost]
        public JsonResult DeleteSighting(DeleteSightingModel Sighting)
        {
            int? userID = HttpContext.Items.ContainsKey("UserID") ? (int?)HttpContext.Items["UserID"] : -1;
            return new JsonResult(_sightingsService.DeleteSighting(userID != null ? userID.Value : -1, Sighting));
        }
    }
}
