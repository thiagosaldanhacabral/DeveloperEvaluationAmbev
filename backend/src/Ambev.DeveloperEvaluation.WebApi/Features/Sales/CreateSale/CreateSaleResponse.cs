namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

/// <summary>
/// Response class for the result of creating a sale.
/// </summary>
public record CreateSaleResponse(Guid Id = default, string? SaleNumber = default, decimal TotalAmount = default);