using Ambev.DeveloperEvaluation.Application.Commom;
using Ambev.DeveloperEvaluation.Application.Interfaces;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.ORM;
using Microsoft.Extensions.Caching.Distributed;

namespace Ambev.DeveloperEvaluation.Application.Sales
{
    /// <summary>
    /// Repository for querying Sale entities with caching support.
    /// </summary>
    public class SaleQueryRepository : CacheableQueryRepository<Sale>, ISaleQueryRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SaleQueryRepository"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="cache">The distributed cache instance.</param>
        public SaleQueryRepository(DefaultContext context, IDistributedCache cache)
            : base(context, cache) { }
    }
}
