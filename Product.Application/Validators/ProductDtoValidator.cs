using System;
using FluentValidation;
using Product.Application.Commands;

namespace Product.Application.Validators
{
    public class ProductDtoValidator : AbstractValidator<CreateProductCommand>
    {
        public ProductDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(x => x.ManufactureEmail).NotEmpty().EmailAddress();
            RuleFor(x => x.ProduceDate).LessThanOrEqualTo(DateTime.Today);
        }
    }
}