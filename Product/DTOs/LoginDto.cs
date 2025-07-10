using System.ComponentModel.DataAnnotations;

namespace Product.DTOs
{
    public class LoginDto
    {
        [Required]
        public string Username { get; set; } = null!; 

        [Required]
        public string Password { get; set; } = null!;
    }
}