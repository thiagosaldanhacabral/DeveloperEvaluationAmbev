using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

/// <summary>
/// Query for retrieving a sale by its ID
/// </summary>
public record GetSaleQuery : IRequest<GetSaleResult>
{
    /// <summary>
    /// The unique identifier of the sale to retrieve
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Filter for sales containing a specific product name
    /// </summary>
    public string ProductNameContains { get; set; } = string.Empty;

    /// <summary>
    /// The page number for paginated results
    /// </summary>
    public int? Page { get; set; }

    /// <summary>
    /// The number of results per page for paginated results
    /// </summary>
    public int? PageSize { get; set; }

    /// <summary>
    /// Initializes a new instance of GetSaleQuery
    /// </summary>
    /// <param name="id">The ID of the sale to retrieve</param>
    public GetSaleQuery(Guid id)
    {
        Id = id;
    }
}
