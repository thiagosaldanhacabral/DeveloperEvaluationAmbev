using System.Linq.Expressions;

namespace Ambev.DeveloperEvaluation.Application.Commom
{
    public class QueryParams<T>
    {
        public Expression<Func<T, bool>>? Filter { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public Func<IQueryable<T>, IOrderedQueryable<T>>? OrderBy { get; set; }

        public string CacheKey => GenerateCacheKey();

        private string GenerateCacheKey()
        {
            var filterString = Filter?.ToString() ?? "nofilter";
            var paging = $"page:{Page ?? 1},size:{PageSize ?? 10}";
            var ordering = OrderBy?.Method.Name ?? "noorder";
            return $"{typeof(T).Name}:{filterString}:{paging}:{ordering}".ToLowerInvariant();
        }
    }
}
