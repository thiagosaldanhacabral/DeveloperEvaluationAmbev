namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.DeleteSale;

/// <summary>
/// Response class for the result of deleting a sale.
/// </summary>
public class DeleteSaleResponse
{
    /// <summary>
    /// Gets or sets the message indicating the result of the deletion.
    /// </summary>
    public required string Message { get; set; }
}