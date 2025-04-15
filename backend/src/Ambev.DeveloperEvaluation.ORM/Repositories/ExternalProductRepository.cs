using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

/// <summary>
/// Implementation of IExternalProductRepository using Entity Framework Core.
/// </summary>
public class ExternalProductRepository(DefaultContext context) : IExternalProductRepository
{
    public async Task<ExternalProduct> CreateAsync(ExternalProduct product, CancellationToken cancellationToken = default)
    {
        await context.ExternalProducts.AddAsync(product, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
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
        return true;
    }

    public async Task<ExternalProduct> UpdateAsync(ExternalProduct product, CancellationToken cancellationToken = default)
    {
        context.ExternalProducts.Update(product);
        await context.SaveChangesAsync(cancellationToken);
        return product;
    }
}