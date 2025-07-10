using System;
using System.ComponentModel.DataAnnotations;

namespace Product.DTOs
{
	public class ProductDto
	{
       [Required]
    public string? Name { get; set; }
    [Required, EmailAddress]
    public string? ManufactureEmail { get; set; }
    public string? ManufacturePhone { get; set; }
    [Required]
    public DateTime? ProduceDate { get; set; }
    public bool? IsAvailable { get; set; }
    }
}

