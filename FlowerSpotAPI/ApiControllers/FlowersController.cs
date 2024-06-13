using FlowerSpotAPI.CustomAttributes;
using FlowerSpotCore.InterfacesServices;
using FlowerSpotCore.ModelsEndpoints.Flowers;
using FlowerSpotCore.ModelsRepository;
using Microsoft.AspNetCore.Mvc;

namespace FlowerSpotAPI.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlowersController : ControllerBase
    {
        private IFlowersService _flowersService;

        public FlowersController(IFlowersService FlowersService)
        {
            _flowersService = FlowersService;
        }

        [Route("GetAll")]
        [HttpGet]
        public JsonResult GetAll()
        {
            return new JsonResult(_flowersService.GetAllFlowers());
        }

        [Route("GetFlower/{FlowerId}")]
        [HttpGet]
        public JsonResult GetFlower(int FlowerId)
        {
            return new JsonResult(_flowersService.GetFlower(FlowerId));
        }

        [CustomAuthorize]
        [Route("AddFlower")]
        [HttpPost]
        public JsonResult AddFlower(AddFlowerModel Flower)
        {
            return new JsonResult(_flowersService.AddFlower(new FlowerModel(Flower.Name, Flower.ImageRef, Flower.Description)));
        }

    }
}
