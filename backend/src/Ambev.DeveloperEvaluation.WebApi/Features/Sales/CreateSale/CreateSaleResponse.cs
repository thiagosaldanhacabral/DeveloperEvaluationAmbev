namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

/// <summary>
/// Response class for the result of creating a sale.
/// </summary>
public abstract record CreateSaleResponse(Guid Id, string SaleNumber, decimal TotalAmount);
