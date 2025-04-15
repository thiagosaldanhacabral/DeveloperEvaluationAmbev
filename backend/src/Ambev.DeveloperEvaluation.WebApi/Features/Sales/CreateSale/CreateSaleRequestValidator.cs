using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

/// <summary>
/// Validator for CreateSaleRequest that defines validation rules for creating a sale.
/// </summary>
public class CreateSaleRequestValidator : AbstractValidator<CreateSaleRequest>
{
    /// <summary>
    /// Initializes a new instance of the CreateSaleValidator with defined validation rules.
    /// </summary>
    /// <remarks>
    /// Validation rules include:
    /// - SaleNumber: Required, maximum length of 50 characters
    /// - SaleDate: Must be a valid date and cannot be in the future
    /// - Customer: Required and validated using ExternalCustomerValidator
    /// - Branch: Required and validated using ExternalBranchValidator
    /// - Products: Must contain at least one product, each validated using SaleItemValidator
    /// </remarks>
    public CreateSaleRequestValidator()
    {
        RuleFor(sale => sale.SaleNumber)
            .NotEmpty().WithMessage("Sale number is required.")
            .MaximumLength(50).WithMessage("Sale number cannot exceed 50 characters.");

        RuleFor(sale => sale.SaleDate)
            .NotEmpty().WithMessage("Sale date is required.")
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Sale date cannot be in the future.");

        RuleFor(sale => sale.Customer)
            .NotNull().WithMessage("Customer is required.")
            .SetValidator(new CreateExternalCustomerValidator()!);

        RuleFor(sale => sale.Branch)
            .NotNull().WithMessage("Branch is required.")
            .SetValidator(new CreateExternalBranchValidator()!);

        RuleFor(sale => sale.Products)
            .NotEmpty().WithMessage("At least one product is required.")
            .SetValidator(new CreateExternalProductListValidator()!);
    }
}