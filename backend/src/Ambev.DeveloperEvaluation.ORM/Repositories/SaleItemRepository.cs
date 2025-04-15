using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

/// <summary>
/// Implementation of ISaleItemRepository using Entity Framework Core.
/// </summary>
public class SaleItemRepository : ISaleItemRepository
{
    private readonly DefaultContext _context;
    private readonly IMongoCollection<SaleItem> _mongoCollection;

    /// <summary>
    /// Initializes a new instance of SaleItemRepository.
    /// </summary>
    /// <param name="context">The database context.</param>
    public SaleItemRepository(DefaultContext context, IMongoCollection<SaleItem> mongoCollection)
    {
        _context = context;
        _mongoCollection = mongoCollection;
    }

    /// <summary>
    /// Creates a new sale item in the database.
    /// </summary>
    /// <param name="saleItem">The sale item to create.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The created sale item.</returns>
    public async Task<SaleItem> CreateAsync(SaleItem saleItem, CancellationToken cancellationToken = default)
    {
        await _context.SaleItems.AddAsync(saleItem, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        await _mongoCollection.InsertOneAsync(saleItem, cancellationToken: cancellationToken);
        return saleItem;
    }

    /// <summary>
    /// Retrieves a sale item by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the sale item.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The sale item if found, null otherwise.</returns>
    public async Task<SaleItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.SaleItems.FirstOrDefaultAsync(si => si.Id == id, cancellationToken);
    }

    /// <summary>
    /// Retrieves sale items associated with a specific sale ID.
    /// </summary>
    /// <param name="saleId">The sale ID to search for.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A list of sale items for the given sale ID.</returns>
    public async Task<IEnumerable<SaleItem>> GetBySaleIdAsync(Guid saleId, CancellationToken cancellationToken = default)
    {
        return await _context.SaleItems
            .Where(si => si.SaleId == saleId)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Retrieves sale items based on product name.
    /// </summary>
    /// <param name="productName">The product name to search for.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A list of sale items matching the product name.</returns>
    public async Task<IEnumerable<SaleItem>> GetByProductNameAsync(string productName, CancellationToken cancellationToken = default)
    {
        return await _context.SaleItems
            .Where(si => si.Product.ProductName.Contains(productName))
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Deletes a sale item from the database.
    /// </summary>
    /// <param name="id">The unique identifier of the sale item to delete.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>True if the sale item was deleted, false if not found.</returns>
    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var saleItem = await GetByIdAsync(id, cancellationToken);
        if (saleItem == null)
            return false;

        _context.SaleItems.Remove(saleItem);
        await _context.SaveChangesAsync(cancellationToken);

        var deleteResult = await _mongoCollection.DeleteOneAsync(si => si.Id == id, cancellationToken);
        return deleteResult.DeletedCount > 0;
    }

    /// <summary>
    /// Updates an existing sale item in the database.
    /// </summary>
    /// <param name="saleItem">The sale item to update.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The updated sale item.</returns>
    public async Task<SaleItem> UpdateAsync(SaleItem saleItem, CancellationToken cancellationToken = default)
    {
        _context.SaleItems.Update(saleItem);
        await _context.SaveChangesAsync(cancellationToken);

        await _mongoCollection.ReplaceOneAsync(si => si.Id == saleItem.Id, saleItem, cancellationToken: cancellationToken);
        return saleItem;
    }
}