using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;

/// <summary>
/// Validator for validating SaleItem data.
/// </summary>
public class UpdateExternalProductListValidator : AbstractValidator<List<UpdateSaleProductsRequest>>
{
    /// <summary>
    /// Initializes a new instance of SaleItemValidator with defined validation rules.
    /// </summary>
    public UpdateExternalProductListValidator()
    {
        // Ensures that the list has at least one Product
        RuleFor(items => items)
            .NotEmpty().WithMessage("At least one product must be added to the sale.");

        // Validates each SaleItem in the list using the ProductValidator
        RuleForEach(items => items)
            .SetValidator(new UpdateExternalProductValidator())
            .WithMessage("Each product in the sale must be valid.");

        // Ensure that the total quantity does not exceed the limit (e.g., 20 items)
        RuleFor(items => items.Sum(item => item.Quantity))
            .LessThanOrEqualTo(100).WithMessage("Total quantity of products cannot exceed 100.");
    }
}

public class UpdateExternalProductValidator : AbstractValidator<UpdateSaleProductsRequest>
{
    public UpdateExternalProductValidator()
    {
        RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than zero.")
            .LessThanOrEqualTo(20).WithMessage("Quantity cannot exceed 20 items.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Unit Price must be greater than zero.")
            .LessThanOrEqualTo(100000).WithMessage("Unit Price cannot exceed 100,000.")
            .PrecisionScale(10, 2, true).WithMessage("Unit Price must have at most two decimal places.");
    }
}