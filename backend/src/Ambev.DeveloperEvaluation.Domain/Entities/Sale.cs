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
        /// <summary>
        /// Initializes a new instance of the <see cref="Sale"/> class.
        /// </summary>
        public Sale(Guid customerId, Guid branchId, string saleNumber, DateTime saleDate, ExternalCustomer customer, ExternalBranch branch)
        {
            CustomerId = customerId;
            BranchId = branchId;
            SaleNumber = saleNumber;
            SaleDate = saleDate;
            Customer = customer;
            Branch = branch;
            CreatedAt = DateTime.UtcNow;
            TotalAmount = 0;
        }

        /// <summary>
        /// Performs validation of the sale entity using the SaleValidator rules.
        /// </summary>
        /// <returns>
        /// A <see cref="ValidationResultDetail"/> containing:
        /// - IsValid: Indicates whether all validation rules passed
        /// - Errors: Collection of validation errors if any rules failed
        /// </returns>
        /// <remarks>
        /// <listheader>The validation includes checking:</listheader>
        /// <list type="bullet">Sale number format and non-emptiness</list>
        /// <list type="bullet">Sale date validity</list>
        /// <list type="bullet">Presence of customer and branch details</list>
        /// <list type="bullet">Presence of at least one product in the sale</list>
        /// <list type="bullet">Total amount calculation consistency</list>
        /// </remarks>
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
        /// <summary>
        /// Cancel the sale.
        /// Changes the user's status to Inactive.
        /// </summary>
        public void Cancel()
        {
            IsCancelled = true;
            UpdatedAt = DateTime.UtcNow;
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
        
        /// <summary>
        /// Gets the customerId who made the purchase.
        /// </summary>
        public Guid CustomerId { get; private set; }
        
        /// <summary>
        /// Gets the branch where the sale occurred.
        /// </summary>
        public ExternalBranch Branch { get; private set; }
        
        /// <summary>
        /// Gets the branchId where the sale occurred.
        /// </summary>
        public Guid BranchId { get; private set; }

        /// <summary>
        /// Gets the collection of items included in the sale.
        /// </summary>
        public List<SaleItem> Items { get; private set; } = [];

        /// <summary>
        /// Gets the total amount for the sale, including discounts.
        /// </summary>
        public decimal TotalAmount { get; private set; } 

        /// <summary>
        /// Gets a value indicating whether the sale has been canceled.
        /// </summary>
        public bool IsCancelled { get; private set; }
        
        /// <summary>
        /// Adds an item to the sale.
        /// </summary>
        /// <param name="item">The item to add.</param>
        public void AddItem(SaleItem item)
        {
            if (IsCancelled)
                throw new InvalidOperationException("Cannot add items to a cancelled sale.");
            Items.Add(item);
            TotalAmount += item.TotalAmount;
        }
        
        /// <summary>
        /// Recalculate TotalAmount of products
        /// </summary>
        public void RecalculateTotalAmount()
        {
            TotalAmount = Items.Sum(x => x.TotalAmount);
        }


        /// <summary>
        /// Cancels the entire sale.
        /// </summary>
        public void CancelSale()
        {
            IsCancelled = true;
            foreach (var item in Items)
                item.CancelItem();
        }
    }