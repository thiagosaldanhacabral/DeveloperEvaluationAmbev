using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Validation;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents an item in a sales transaction.
/// Includes product details, quantity, and discount calculations.
/// </summary>
public class SaleItem : BaseEntity
{
    /// <summary>
    /// Gets the product associated with the sale item.
    /// </summary>
    public ExternalProduct Product { get; private set; }

    /// <summary>
    /// Gets the productId associated with the sale item.
    /// </summary>
    public Guid ProductId { get; private set; }

    /// <summary>
    /// Gets the sale associated with the sale item.
    /// </summary>
    public Sale? Sale { get; private set; }

    /// <summary>
    /// Gets the saleId associated with the sale item.
    /// </summary>
    public Guid SaleId { get; private set; }

    /// <summary>
    /// Gets the quantity of the product purchased.
    /// </summary>
    public int Quantity { get; private set; }

    /// <summary>
    /// Gets the discount applied to the item.
    /// </summary>
    public decimal Discount { get; private set; }

    /// <summary>
    /// Gets the total amount for the item after discount.
    /// </summary>
    public decimal TotalAmount { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the item has been canceled.
    /// </summary>
    public bool IsCancelled { get; private set; }

    public SaleItem()
    {

    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SaleItem"/> class.
    /// </summary>
    /// <param name="saleId">the id of sale.</param>
    /// <param name="quantity">The quantity of the product.</param>
    /// <param name="product">The product </param>
    public SaleItem(Guid saleId, int quantity, ExternalProduct product)
    {
        ProductId = product.Id;
        SaleId = saleId;
        Quantity = quantity;
        Product = product;
        CreatedAt = DateTime.UtcNow;
        Discount = CalculateDiscount(quantity);
        TotalAmount = CalculateTotalAmount();
    }

    /// <summary>
    /// Cancels the sale item.
    /// </summary>
    public void CancelItem()
    {
        IsCancelled = true;
    }

    /// <summary>
    /// Performs validation of the sale item entity using the SaleItemValidator rules.
    /// </summary>
    /// <returns>
    /// A <see cref="ValidationResultDetail"/> containing:
    /// - IsValid: Indicates whether all validation rules passed
    /// - Errors: Collection of validation errors if any rules failed
    /// </returns>
    /// <remarks>
    /// <listheader>The validation includes checking:</listheader>
    /// <list type="bullet">Presence of a valid product ID</list>
    /// <list type="bullet">Quantity is greater than zero</list>
    /// <list type="bullet">Quantity does not exceed allowed limits</list>
    /// </remarks>
    public ValidationResultDetail Validate()
    {
        var validator = new SaleItemValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }

    /// <summary>
    /// Calculates the discount based on quantity.
    /// </summary>
    /// <param name="quantity">The quantity of the product.</param>
    /// <returns>The discount percentage.</returns>
    private decimal CalculateDiscount(int quantity)
    {
        if (quantity < 4) return 0m;
        if (quantity >= 4 && quantity < 10) return 0.1m;
        if (quantity >= 10 && quantity <= 20) return 0.2m;
        throw new InvalidOperationException("Cannot sell more than 20 identical items.");
    }
    /// <summary>
    /// Calculates the total amount based on quantity and value.
    /// </summary>
    /// <returns>The total amount.</returns>
    private decimal CalculateTotalAmount()
    {
        return (Quantity * Product.Price) * (1 - Discount);
    }
}
