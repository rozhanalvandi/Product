using Xunit;
using FluentValidation.TestHelper;
using Product.Commands;  
using Product.Validators;
using System;

namespace Product.Tests.Validators
{
 public class ProductValidatorTests
{
    private readonly ProductDtoValidator _validator = new ProductDtoValidator();

    [Fact]
    public void Should_Have_Error_When_Name_Is_Empty()
    {
        var product = new CreateProductCommand
        {
            Name = "",
            ManufactureEmail = "test@company.com",
            ManufacturePhone = "1234567890",
            ProduceDate = new DateTime(2025, 7, 10),
            IsAvailable = true
        };
        var result = _validator.TestValidate(product);
        result.ShouldHaveValidationErrorFor(p => p.Name);
    }

    [Fact]
    public void Should_Have_Error_When_ManufactureEmail_Is_Invalid()
    {
        var product = new CreateProductCommand
        {
            Name = "Valid Name",
            ManufactureEmail = "not-an-email",
            ManufacturePhone = "1234567890",
            ProduceDate = new DateTime(2025, 7, 10),
            IsAvailable = true
        };
        var result = _validator.TestValidate(product);
        result.ShouldHaveValidationErrorFor(p => p.ManufactureEmail);
    }

    [Fact]
    public void Should_Not_Have_Error_For_Valid_Product()
    {
        var product = new CreateProductCommand
        {
            Name = "Test Product",
            ManufactureEmail = "test@company.com",
            ManufacturePhone = "09123603231",
            ProduceDate = new DateTime(2025, 7, 10),
            IsAvailable = true
        };

        var result = _validator.TestValidate(product);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
}
