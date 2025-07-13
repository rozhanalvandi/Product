using System;

namespace Product.Application.Commands;

public class CreateProductCommand
{
    public required string Name { get; set; }
    public required string ManufactureEmail { get; set; }
    public  string? ManufacturePhone { get; set; }
    public DateTime ProduceDate { get; set; }
    public bool IsAvailable { get; set; }
}
