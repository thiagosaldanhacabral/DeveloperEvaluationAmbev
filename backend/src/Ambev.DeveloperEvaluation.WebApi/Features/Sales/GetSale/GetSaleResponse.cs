using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;

/// <summary>
/// Response class for the result of retrieving a sale.
/// </summary>
public abstract record GetSaleResponse(
    Guid Id,
    string SaleNumber,
    DateTime SaleDate,
    ExternalCustomer Customer,
    ExternalBranch Branch,
    decimal TotalAmount,
    List<Domain.Entities.SaleItem> Products);
