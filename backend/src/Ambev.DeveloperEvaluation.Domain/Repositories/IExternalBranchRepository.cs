using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

/// <summary>
/// Defines the repository interface for ExternalBranch entity.
/// </summary>
public interface IExternalBranchRepository
{
    Task<ExternalBranch> CreateAsync(ExternalBranch branch, CancellationToken cancellationToken = default);
    Task<ExternalBranch?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<ExternalBranch>> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ExternalBranch> UpdateAsync(ExternalBranch branch, CancellationToken cancellationToken = default);
    Task<ExternalBranch?> GetByIdFromMongoAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<ExternalBranch>> GetByNameFromMongoAsync(string name, CancellationToken cancellationToken = default);
    Task<IEnumerable<ExternalBranch>> GetAllFromMongoAsync(CancellationToken cancellationToken = default);
}