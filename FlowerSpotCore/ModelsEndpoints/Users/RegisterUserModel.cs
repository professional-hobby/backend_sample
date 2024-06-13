using System.ComponentModel.DataAnnotations;

namespace FlowerSpotCore.ModelsEndpoints.Users
{
    public class RegisterUserModel
    {
        [Required]
        public string UserName { get; set; } = "";

        [Required]
        public string Password { get; set; } = "";

        [Required]
        public string Email { get; set; } = "";
    }
}
