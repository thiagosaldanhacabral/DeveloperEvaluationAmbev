using Ambev.DeveloperEvaluation.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Interfaces
{
    /// <summary>
    /// Interface for storing created Sale entities in Redis.
    /// </summary>
    public interface ISaleCreateRepository
    {
        /// <summary>
        /// Stores a created sale in Redis.
        /// </summary>
        /// <param name="sale">The sale to store.</param>
        /// <param name="cacheKey">The cache key for the sale.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task StoreSaleInCacheAsync(Sale sale, string cacheKey, CancellationToken cancellationToken = default);
    }
}
