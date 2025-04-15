namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;

/// <summary>
/// Request class for updating an existing sale.
/// </summary>
public class UpdateSaleRequest
{
    /// <summary>
    /// Gets or sets the unique identifier of the sale to update.
    /// </summary>
    public Guid? Id { get; set; }

    /// <summary>
    /// Gets or sets the sale number (e.g., invoice number).
    /// </summary>
    public string? SaleNumber { get; set; }

    /// <summary>
    /// Gets or sets the date when the sale was made.
    /// </summary>
    public DateTime? SaleDate { get; set; }

    /// <summary>
    /// Gets or sets the customer associated with the sale.
    /// </summary>
    public UpdateSaleCustomerRequest? Customer { get; set; }

    /// <summary>
    /// Gets or sets the branch where the sale was made.
    /// </summary>
    public UpdateSaleBranchRequest? Branch { get; set; }

    /// <summary>
    /// Gets or sets the list of products in the sale.
    /// </summary>
    public List<UpdateSaleProductsRequest>? Products { get; set; } 
}

public abstract class UpdateSaleProductsRequest
{
    /// <summary>
    /// Gets the id of the product.
    /// </summary>
    public Guid? ProductId { get; set; }
    /// <summary>
    /// Gets the name of the product.
    /// </summary>
    public string? ProductName { get; set; }

    /// <summary>
    /// Gets the price of the product.
    /// </summary>
    public decimal? Price { get; set; }
    
    /// <summary>
    /// Gets the quantity of the product.
    /// </summary>
    public int? Quantity { get; set; }
}

public abstract class UpdateSaleCustomerRequest
{
    // <summary>
    /// Gets the id of the customer.
    /// </summary>
    public string? CustomerId { get; set; }
    
    // <summary>
    /// Gets the name of the customer.
    /// </summary>
    public string? CustomerName { get; set; }

    /// <summary>
    /// Gets the contact email of the customer.
    /// </summary>
    public string? Email { get; set; }
        
    /// <summary>
    /// Gets the contact phone of the customer.
    /// </summary>
    public string? Phone { get; set; }
    
}

public abstract class UpdateSaleBranchRequest
{
    /// <summary>
    /// Gets the id of the branch.
    /// </summary>
    public Guid? BranchId { get; set; }
    
    /// <summary>
    /// Gets the name of the branch.
    /// </summary>
    public string? BranchName { get; set; }

    /// <summary>
    /// Gets the location of the branch.
    /// </summary>
    public string? Location { get; set; }
}