using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ambev.DeveloperEvaluation.Application.Commom
{
    public abstract class CacheableQueryRepository<T> where T : class
    {
        private readonly IDistributedCache _cache;
        protected readonly DbSet<T> _dbSet;

        protected CacheableQueryRepository(DbContext context, IDistributedCache cache)
        {
            _cache = cache;
            _dbSet = context.Set<T>();
        }

        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public async Task<IEnumerable<T>> QueryAsync(QueryParams<T> parameters, CancellationToken cancellationToken = default)
        {
            var cacheKey = parameters.CacheKey;

            var cached = await _cache.GetStringAsync(cacheKey, cancellationToken);
            if (!string.IsNullOrWhiteSpace(cached))
                return JsonSerializer.Deserialize<List<T>>(cached)!;

            IQueryable<T> query = _dbSet;

            if (parameters.Filter != null)
                query = query.Where(parameters.Filter);

            if (parameters.OrderBy != null)
                query = parameters.OrderBy(query);

            if (parameters.Page.HasValue && parameters.PageSize.HasValue)
            {
                query = query
                    .Skip((parameters.Page.Value - 1) * parameters.PageSize.Value)
                    .Take(parameters.PageSize.Value);
            }

            var result = await query.ToListAsync(cancellationToken);

            await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(result), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            }, cancellationToken);

            return result;
        }

        protected async Task SetCacheAsync<TValue>(string cacheKey, TValue value, CancellationToken cancellationToken = default)
        {
            if (EqualityComparer<TValue>.Default.Equals(value, default))
                throw new ArgumentNullException(nameof(value));

            var serializedValue = JsonSerializer.Serialize(value, _jsonOptions);

            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1)
            };

            await _cache.SetStringAsync(cacheKey, serializedValue, cacheOptions, cancellationToken);
        }
    }
}
