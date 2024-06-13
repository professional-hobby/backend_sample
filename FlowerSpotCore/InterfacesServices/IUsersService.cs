using FlowerSpotCore.ModelsServices;

namespace FlowerSpotCore.InterfacesServices
{
    public interface IUsersService
    {
        public bool AddUser(string UserName, string Password, string Email);
        public string LoginUser(string UserName, string Password);
        public JwtTokenModel? ValidateTokenAndGetData(string Token);
    }
}
