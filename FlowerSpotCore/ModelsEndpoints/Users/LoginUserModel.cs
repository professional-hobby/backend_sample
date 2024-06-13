using System.ComponentModel.DataAnnotations;

namespace FlowerSpotCore.ModelsEndpoints.Users
{
    public class LoginUserModel
    {
        [Required]
        public string UserName { get; set; } = "";

        [Required]
        public string Password { get; set; } = "";
    }
}
