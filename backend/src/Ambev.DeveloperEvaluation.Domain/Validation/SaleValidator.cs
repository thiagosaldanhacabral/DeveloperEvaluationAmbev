using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

public class SaleValidator : AbstractValidator<Sale>
{
    public SaleValidator()
    {
        RuleFor(sale => sale.SaleNumber)
            .NotEmpty().WithMessage("Sale number cannot be empty.")
            .MaximumLength(50).WithMessage("Sale number cannot be longer than 50 characters.");

        RuleFor(sale => sale.SaleDate)
            .NotEmpty().WithMessage("Sale date must be provided.")
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Sale date cannot be in the future.");

        RuleFor(sale => sale.Customer)
            .NotNull().WithMessage("Customer details must be provided.");

        RuleFor(sale => sale.Branch)
            .NotNull().WithMessage("Branch details must be provided.");

        RuleFor(sale => sale.Items)
            .NotEmpty().WithMessage("At least one product must be included in the sale.")
            .Must(items => items.All(item => item.Quantity > 0))
            .WithMessage("All products must have a quantity greater than 0.");

        RuleFor(sale => sale.TotalAmount)
            .GreaterThan(0).WithMessage("Total amount must be greater than 0.")
            .Must((sale, total) => 
                Math.Round(total, 2) == Math.Round(sale.Items.Sum(item => item.Product.Price * item.Quantity), 2))
            .WithMessage("Total amount does not match the sum of sale items.");
    }
}