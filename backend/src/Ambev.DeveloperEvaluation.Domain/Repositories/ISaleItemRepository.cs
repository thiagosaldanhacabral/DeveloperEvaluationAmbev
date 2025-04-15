using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

/// <summary>
/// Defines the repository interface for SaleItem entity.
/// </summary>
public interface ISaleItemRepository
{
    Task<SaleItem> CreateAsync(SaleItem saleItem, CancellationToken cancellationToken = default);
    Task<SaleItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<SaleItem>> GetBySaleIdAsync(Guid saleId, CancellationToken cancellationToken = default);
    Task<IEnumerable<SaleItem>> GetByProductNameAsync(string productName, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<SaleItem> UpdateAsync(SaleItem saleItem, CancellationToken cancellationToken = default);
}