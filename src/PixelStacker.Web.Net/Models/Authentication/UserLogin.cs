using System.ComponentModel.DataAnnotations;

namespace PixelStacker.Web.Net.Models.Authentication
{

    public class UserLoginResult
    {
        public string JwtToken { get; set; }
    }

    public class UserLogin
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(4)]
        [MaxLength(64)]
        public string Password { get; set; }


        public bool RememberMe { get; set; } = false;
    }
}
