using Ambev.DeveloperEvaluation.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Interfaces
{
    /// <summary>
    /// Interface for storing created User entities in Redis.
    /// </summary>
    public interface IUserCreateRepository
    {
        /// <summary>
        /// Stores a created user in Redis.
        /// </summary>
        /// <param name="user">The user to store.</param>
        /// <param name="cacheKey">The cache key for the user.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task StoreUserInCacheAsync(User user, string cacheKey, CancellationToken cancellationToken = default);
    }
}
