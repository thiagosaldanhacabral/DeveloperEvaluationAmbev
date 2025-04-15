namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;

/// <summary>
/// Response class for the result of updating a sale.
/// </summary>
public abstract record UpdateSaleResponse(Guid Id, string SaleNumber, decimal TotalAmount);