using FlowerSpotAPI.CustomAttributes;
using FlowerSpotCore.InterfacesServices;
using FlowerSpotCore.ModelsEndpoints.Likes;
using Microsoft.AspNetCore.Mvc;

namespace FlowerSpotAPI.ApiControllers
{
    [CustomAuthorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LikesController : ControllerBase
    {
        private ILikesService _likesService;

        public LikesController(ILikesService LikesService)
        {
            _likesService = LikesService;
        }

        [Route("CountLikes")]
        [HttpPost]
        public JsonResult CountLikes(CountLikesModel LikeModel)
        {
            return new JsonResult(_likesService.CountLikes(LikeModel));
        }

        [Route("AddLike")]
        [HttpPost]
        public JsonResult AddLike(AddLikeModel LikeModel)
        {
            int? userID = HttpContext.Items.ContainsKey("UserID") ? (int?)HttpContext.Items["UserID"] : -1;
            return new JsonResult(_likesService.AddLike(userID != null ? userID.Value : -1, LikeModel));
        }

        [Route("DeleteLike")]
        [HttpPost]
        public JsonResult DeleteLike(DeleteLikeModel LikeModel)
        {
            int? userID = HttpContext.Items.ContainsKey("UserID") ? (int?)HttpContext.Items["UserID"] : -1;
            return new JsonResult(_likesService.DeleteLike(userID != null ? userID.Value : -1, LikeModel));
        }
    }
}
