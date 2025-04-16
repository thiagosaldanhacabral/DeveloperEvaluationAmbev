using Ambev.DeveloperEvaluation.Application.Commom;
using Ambev.DeveloperEvaluation.Application.Interfaces;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.ORM;
using Microsoft.Extensions.Caching.Distributed;

namespace Ambev.DeveloperEvaluation.Application.Users
{
    /// <summary>
    /// Repository for storing created User entities in Redis.
    /// </summary>
    public class UserCreateRepository : CacheableQueryRepository<User>, IUserCreateRepository
    {
        public UserCreateRepository(DefaultContext context, IDistributedCache cache)
            : base(context, cache) { }

        /// <summary>
        /// Stores a created user in Redis.
        /// </summary>
        /// <param name="user">The user to store.</param>
        /// <param name="cacheKey">The cache key for the user.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        public async Task StoreUserInCacheAsync(User user, string cacheKey, CancellationToken cancellationToken = default)
        {
            await SetCacheAsync(cacheKey, user, cancellationToken);
        }
    }
}
