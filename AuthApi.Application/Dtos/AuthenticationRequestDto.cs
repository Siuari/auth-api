using System.ComponentModel.DataAnnotations;

namespace AuthApi.Application.Dtos
{
    public class AuthenticationRequestDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
