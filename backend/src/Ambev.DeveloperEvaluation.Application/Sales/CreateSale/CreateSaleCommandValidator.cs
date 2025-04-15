using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Validator for CreateSaleCommand that defines validation rules for sale creation command.
/// </summary>
public class CreateSaleCommandValidator : AbstractValidator<CreateSaleCommand>
{
    /// <summary>
    /// Initializes a new instance of the CreateSaleCommandValidator with defined validation rules.
    /// </summary>
    /// <remarks>
    /// Validation rules include:
    /// - SaleNumber: Required and must be alphanumeric with a length between 5 and 20 characters.
    /// - SaleDate: Required and must be in the past or present.
    /// - Customer: Must be a valid customer.
    /// - Branch: Must be a valid branch.
    /// - SaleItems: Must contain at least one item and all items must have valid data.
    /// - TotalAmount: Must be greater than 0.
    /// - IsCancelled: Can be true or false, but should not affect validation.
    /// </remarks>
    public CreateSaleCommandValidator()
    {
        RuleFor(sale => sale.SaleNumber)
            .NotEmpty().WithMessage("Sale number is required.")
            .Matches("^[a-zA-Z0-9]*$").WithMessage("Sale number must be alphanumeric.")
            .Length(5, 20).WithMessage("Sale number must be between 5 and 20 characters.");

        RuleFor(sale => sale.SaleDate)
            .NotEmpty().WithMessage("Sale date is required.")
            .Must(date => date <= DateTime.Now).WithMessage("Sale date must be in the past or present.");

        RuleFor(sale => sale.Customer)
            .NotNull().WithMessage("Customer is required.");

        RuleFor(sale => sale.Branch)
            .NotNull().WithMessage("Branch is required.");

        RuleFor(sale => sale.SaleItems)
            .NotEmpty().WithMessage("At least one product must be included in the sale.")
            .Must(items => items.All(item => item is { Quantity: > 0, Product.Price: > 0 }))
            .WithMessage("Each item must have a positive quantity and unit price.")
            .Must(items => items.All(item => item.Quantity <= 20)).WithMessage("You cannot have more than 20 of a quantity for each product.");
    }
}