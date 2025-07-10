using System;
using System.ComponentModel.DataAnnotations;

namespace Product.DTOs
{
    public class ProductDto
    {
        [Required]
        public string Name { get; set; } = null!; 

        [Required, EmailAddress]
        public string ManufactureEmail { get; set; } = null!; 

        public string? ManufacturePhone { get; set; }

        [Required]
        public DateTime ProduceDate { get; set; } 

        public bool? IsAvailable { get; set; } 
    }
}