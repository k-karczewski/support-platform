using System.ComponentModel.DataAnnotations;

namespace SupportPlatform.Services
{
    public class UserToLoginDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
