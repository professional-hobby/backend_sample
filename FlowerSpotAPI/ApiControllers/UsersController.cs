using FlowerSpotCore.InterfacesServices;
using FlowerSpotCore.ModelsEndpoints.Users;
using Microsoft.AspNetCore.Mvc;

namespace FlowerSpotAPI.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController
    {
        private IUsersService _usersService;

        public UsersController(IUsersService UsersService)
        {
            _usersService = UsersService;
        }

        [Route("RegisterUser")]
        [HttpPost]
        public JsonResult RegisterUser(RegisterUserModel UserModel)
        {
            return new JsonResult(_usersService.AddUser(UserModel.UserName, UserModel.Password, UserModel.Email));
        }

        [Route("LoginUser")]
        [HttpPost]
        public JsonResult LoginUser(LoginUserModel UserModel)
        {
            return new JsonResult(_usersService.LoginUser(UserModel.UserName, UserModel.Password));
        }
    }
}
