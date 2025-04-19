using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;

/// <summary>
/// Response class for the result of retrieving a sale.
/// </summary>
public record GetSaleResponse(
    Guid Id = default,
    string? SaleNumber = default,
    DateTime SaleDate = default,
    ExternalCustomer? Customer = default,
    ExternalBranch? Branch = default,
    decimal TotalAmount = default,
    List<SaleItem>? Products = default);
