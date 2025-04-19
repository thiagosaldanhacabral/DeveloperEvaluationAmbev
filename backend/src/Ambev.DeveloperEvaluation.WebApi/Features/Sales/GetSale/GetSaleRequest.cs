namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;

/// <summary>
/// Request class for retrieving an existing sale.
/// </summary>
public record GetSaleRequest(Guid Id = default);
