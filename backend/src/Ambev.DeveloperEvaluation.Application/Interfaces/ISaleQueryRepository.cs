using Ambev.DeveloperEvaluation.Application.Commom;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Interfaces
{
    /// <summary>
    /// Interface for querying Sale entities.
    /// </summary>
    public interface ISaleQueryRepository
    {
        /// <summary>
        /// Queries the Sale entities based on the provided parameters.
        /// </summary>
        /// <param name="parameters">The query parameters to filter and sort the results.</param>
        /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
        /// <returns>A collection of Sale entities matching the query parameters.</returns>
        Task<IEnumerable<Sale>> QueryAsync(QueryParams<Sale> parameters, CancellationToken cancellationToken = default);
    }
}
