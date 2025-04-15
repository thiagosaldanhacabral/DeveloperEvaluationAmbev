using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Entities;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Command for creating a new sale.
/// </summary>
/// <remarks>
/// This command captures the necessary data for creating a sale, 
/// including sale number, date, customer, branch, and sale items (products).
/// It implements <see cref="IRequest"/> to initiate a request 
/// that returns a <see cref="CreateSaleResult"/>.
///
/// The data provided in this command is validated using the 
/// <see cref="CreateSaleCommandValidator"/> to ensure that the fields are correctly 
/// populated and follow the required rules.
/// </remarks>
public class CreateSaleCommand : IRequest<CreateSaleResult>
{
    /// <summary>
    /// Gets or sets the unique sale number.
    /// </summary>
    public string SaleNumber { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets the date when the sale was made.
    /// </summary>
    public DateTime SaleDate { get; init; }

    /// <summary>
    /// Gets or sets the customer associated with the sale.
    /// </summary>
    public ExternalCustomer Customer { get; init; }

    /// <summary>
    /// Gets or sets the branch where the sale was made.
    /// </summary>
    public ExternalBranch Branch { get; init; }

    /// <summary>
    /// Gets or sets the list of sale items (products) in the sale.
    /// </summary>
    public List<SaleItem> SaleItems { get; init; }

    /// <summary>
    /// Gets or sets the total amount of the sale.
    /// </summary>
    public decimal TotalAmount { get; init; }

    /// <summary>
    /// Gets or sets the status of the sale (whether it's cancelled or not).
    /// </summary>
    public bool IsCancelled { get; init; }

    /// <summary>
    /// Validates the command data using the appropriate validator.
    /// </summary>
    /// <returns>Validation result details</returns>
    public ValidationResultDetail Validate()
    {
        var validator = new CreateSaleCommandValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }
}