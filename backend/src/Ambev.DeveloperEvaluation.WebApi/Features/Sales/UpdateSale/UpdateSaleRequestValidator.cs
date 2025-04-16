using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;

/// <summary>
/// Validator for UpdateSaleRequest that defines validation rules for updating an existing sale.
/// </summary>
public class UpdateSaleRequestValidator : AbstractValidator<UpdateSaleRequest>
{
    /// <summary>
    /// Initializes a new instance of the UpdateSaleValidator with defined validation rules.
    /// </summary>
    /// <remarks>
    /// Validation rules include:
    /// - Id: Required, to identify the sale being updated
    /// - SaleNumber: Required, maximum length of 50 characters
    /// - SaleDate: Must be a valid date and cannot be in the future
    /// - Customer: Required and validated using ExternalCustomerValidator
    /// - Branch: Required and validated using ExternalBranchValidator
    /// - Products: Must contain at least one product, each validated using SaleItemValidator
    /// </remarks>
    public UpdateSaleRequestValidator()
    {
        RuleFor(sale => sale.Id)
            .NotEmpty().WithMessage("Sale ID is required.");

        RuleFor(sale => sale.SaleNumber)
            .NotEmpty().WithMessage("Sale number is required.")
            .MaximumLength(50).WithMessage("Sale number cannot exceed 50 characters.");

        RuleFor(sale => sale.SaleDate)
            .NotEmpty().WithMessage("Sale date is required.")
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Sale date cannot be in the future.");

        RuleFor(sale => sale.Customer)
            .NotNull().WithMessage("Customer is required.")
            .SetValidator(new UpdateExternalCustomerValidator()!);

        RuleFor(sale => sale.Branch)
            .NotNull().WithMessage("Branch is required.")
            .SetValidator(new UpdateExternalBranchValidator()!);

        RuleFor(sale => sale.Products)
            .NotEmpty().WithMessage("At least one product is required.")
            .ForEach(product => product.SetValidator(new UpdateExternalProductValidator()));
    }
}