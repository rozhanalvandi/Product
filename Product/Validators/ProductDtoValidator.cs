using FluentValidation;
using Product.DTOs;
public class ProductDtoValidator : AbstractValidator<ProductDto>
{
    public ProductDtoValidator()
    {
        RuleFor(p => p.Name).NotEmpty().WithMessage("Name is required.");
        RuleFor(p => p.ManufactureEmail)
            .NotEmpty().WithMessage("Manufacture Email is required.")
            .EmailAddress().WithMessage("Manufacture Email must be a valid email.");
        RuleFor(p => p.ManufacturePhone).NotEmpty().WithMessage("Manufacture Phone is required.");
        RuleFor(p => p.ProduceDate).NotEmpty().WithMessage("Produce Date is required.");
    }
}