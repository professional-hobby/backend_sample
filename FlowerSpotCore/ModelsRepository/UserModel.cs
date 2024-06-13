using System.ComponentModel.DataAnnotations;

namespace FlowerSpotCore.ModelsRepository
{
    public class UserModel
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string PasswordSalt { get; set; }

        [Required]
        public string Email { get; set; }

        public UserModel(string UserName, string Password, string PasswordSalt, string Email)
        {
            this.UserName = UserName;
            this.Password = Password;
            this.PasswordSalt = PasswordSalt;
            this.Email = Email;
        }
    }
}
