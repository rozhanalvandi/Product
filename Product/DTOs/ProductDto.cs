using System;
using System.ComponentModel.DataAnnotations;

namespace Product.DTOs
{
    public class ProductDto
    {
        [Required]
        public string Name { get; set; } = null!; // Changed from string? to string

        [Required, EmailAddress]
        public string ManufactureEmail { get; set; } = null!; // Changed from string? to string

        public string? ManufacturePhone { get; set; } // Remains nullable, as per CreateProductCommand

        [Required]
        public DateTime ProduceDate { get; set; } // Changed from DateTime? to DateTime

        public bool? IsAvailable { get; set; } // Remains nullable, consider if it should be bool
    }
}