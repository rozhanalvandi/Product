using System.ComponentModel.DataAnnotations;

namespace Product.DTOs
{
    public class LoginDto
    {
        [Required]
        public string Username { get; set; } = null!; // Use null-forgiving operator as it's required

        [Required]
        public string Password { get; set; } = null!;
    }
}