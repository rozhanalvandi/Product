using System;
namespace Product.Models
{
	public class Products
	{
        public int Id { get; set; }
        public string? Name { get; set; }
        public required string ManufactureEmail { get; set; }
        public string? ManufacturePhone { get; set; }
        public DateTime ProduceDate { get; set; }
        public bool? IsAvailable { get; set; }
        public string? CreatedBy { get; set; }
    }
}

