using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

/// <summary>
/// Validator for the <see cref="ExternalProduct"/> entity.
/// </summary>
public class ExternalProductValidator : AbstractValidator<ExternalProduct>
{
    public ExternalProductValidator()
    {
        RuleFor(product => product.ProductName)
            .NotEmpty()
            .WithMessage("Product name must not be empty.")
            .MaximumLength(100)
            .WithMessage("Product name cannot exceed 100 characters.");

        RuleFor(product => product.Price)
            .GreaterThan(0)
            .WithMessage("Product price must be greater than zero.");
    }
}