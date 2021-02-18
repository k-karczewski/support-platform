using System.ComponentModel.DataAnnotations;

namespace SupportPlatform.Services
{
    public class AccountToConfirmDto
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string Token { get; set; }
    }
}
