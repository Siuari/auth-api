using System.ComponentModel.DataAnnotations;

namespace AuthApi.Application.Dtos
{
    public class InserUserDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
