using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

public record GetSaleResult
(
    Guid Id = default,
    string? SaleNumber = default,
    DateTime SaleDate = default,
    ExternalCustomer? Customer = default,
    ExternalBranch? Branch = default,
    decimal TotalAmount = default,
    List<SaleItem>? Products = default
);