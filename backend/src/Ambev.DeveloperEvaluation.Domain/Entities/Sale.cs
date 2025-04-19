using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Validation;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a sales transaction in the system.
/// Includes customer details, sale items, and business rules validation.
/// </summary>
public class Sale : BaseEntity
{
    public Sale() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Sale"/> class.
    /// </summary>
    public Sale(Guid customerId, Guid branchId, string saleNumber, DateTime saleDate, ExternalCustomer customer, ExternalBranch branch)
    {
        CustomerId = customerId;
        BranchId = branchId;
        SaleNumber = saleNumber;
        SaleDate = saleDate.Kind == DateTimeKind.Utc ? saleDate : saleDate.ToUniversalTime();
        Customer = customer;
        Branch = branch;
        CreatedAt = DateTime.UtcNow;
        TotalAmount = 0;
    }

    /// <summary>
    /// Gets the unique number of the sale.
    /// </summary>
    public string SaleNumber { get; private set; }

    /// <summary>
    /// Gets the date the sale was made.
    /// </summary>
    public DateTime SaleDate { get; private set; }

    /// <summary>
    /// Gets the customer who made the purchase.
    /// </summary>
    public ExternalCustomer Customer { get; private set; }

    public Guid CustomerId { get; private set; }

    public ExternalBranch Branch { get; private set; }

    public Guid BranchId { get; private set; }

    public List<SaleItem> Items { get; private set; } = [];

    public decimal TotalAmount { get; private set; }

    public bool IsCancelled { get; private set; }

    /// <summary>
    /// Cancel the sale and update timestamp.
    /// </summary>
    public void Cancel()
    {
        IsCancelled = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AddItem(SaleItem item)
    {
        if (IsCancelled)
            throw new InvalidOperationException("Cannot add items to a cancelled sale.");

        Items.Add(item);
        TotalAmount += item.TotalAmount;
    }

    public void RecalculateTotalAmount()
    {
        TotalAmount = Items.Sum(x => x.TotalAmount);
    }

    public void CancelSale()
    {
        IsCancelled = true;
        foreach (var item in Items)
            item.CancelItem();
    }

    public ValidationResultDetail Validate()
    {
        var validator = new SaleValidator();
        var result = validator.Validate(this);

        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }
}
