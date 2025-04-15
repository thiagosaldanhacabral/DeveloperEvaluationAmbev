using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

/// <summary>
/// Defines the repository interface for Product entity.
/// </summary>
public interface IExternalProductRepository
{
    Task<ExternalProduct> CreateAsync(ExternalProduct product, CancellationToken cancellationToken = default);
    Task<ExternalProduct?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<ExternalProduct>> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ExternalProduct> UpdateAsync(ExternalProduct product, CancellationToken cancellationToken = default);
}

