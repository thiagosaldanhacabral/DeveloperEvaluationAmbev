using Ambev.DeveloperEvaluation.Application.Commom;
using Ambev.DeveloperEvaluation.Application.Interfaces;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.ORM;
using Microsoft.Extensions.Caching.Distributed;

namespace Ambev.DeveloperEvaluation.Application.Sales
{
    /// <summary>
    /// Repository for storing created Sale entities in Redis.
    /// </summary>
    public class SaleCreateRepository : CacheableQueryRepository<Sale>, ISaleCreateRepository
    {
        public SaleCreateRepository(DefaultContext context, IDistributedCache cache)
            : base(context, cache) { }

        /// <summary>
        /// Stores a created sale in Redis.
        /// </summary>
        /// <param name="sale">The sale to store.</param>
        /// <param name="cacheKey">The cache key for the sale.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        public async Task StoreSaleInCacheAsync(Sale sale, string cacheKey, CancellationToken cancellationToken = default)
        {
            await SetCacheAsync(cacheKey, sale, cancellationToken);
        }
    }
}