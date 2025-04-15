using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

/// <summary>
/// Implementation of IExternalProductRepository using Entity Framework Core.
/// </summary>
public class ExternalProductRepository(DefaultContext context, IMongoCollection<ExternalProduct> mongoCollection) : IExternalProductRepository
{
    public async Task<ExternalProduct> CreateAsync(ExternalProduct product, CancellationToken cancellationToken = default)
    {
        await context.ExternalProducts.AddAsync(product, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        await mongoCollection.InsertOneAsync(product, cancellationToken: cancellationToken);
        return product;
    }

    public async Task<ExternalProduct?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.ExternalProducts.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<ExternalProduct>> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await context.ExternalProducts
            .Where(p => p.ProductName.Contains(name))
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await GetByIdAsync(id, cancellationToken);
        if (product == null)
            return false;

        context.ExternalProducts.Remove(product);
        await context.SaveChangesAsync(cancellationToken);

        var deleteResult = await mongoCollection.DeleteOneAsync(p => p.Id == id, cancellationToken);
        return deleteResult.DeletedCount > 0;
    }

    public async Task<ExternalProduct> UpdateAsync(ExternalProduct product, CancellationToken cancellationToken = default)
    {
        context.ExternalProducts.Update(product);
        await context.SaveChangesAsync(cancellationToken);

        await mongoCollection.ReplaceOneAsync(p => p.Id == product.Id, product, cancellationToken: cancellationToken);
        return product;
    }
}