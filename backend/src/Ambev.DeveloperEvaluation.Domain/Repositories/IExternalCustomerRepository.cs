using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

/// <summary>
/// Defines the repository interface for ExternalCustomer entity.
/// </summary>
public interface IExternalCustomerRepository
{
    Task<ExternalCustomer> CreateAsync(ExternalCustomer customer, CancellationToken cancellationToken = default);
    Task<ExternalCustomer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<ExternalCustomer>> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ExternalCustomer> UpdateAsync(ExternalCustomer customer, CancellationToken cancellationToken = default);
}