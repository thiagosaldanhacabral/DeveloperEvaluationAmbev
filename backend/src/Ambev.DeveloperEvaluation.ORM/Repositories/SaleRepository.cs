using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

/// <summary>
/// Implementation of ISaleRepository using Entity Framework Core.
/// </summary>
public class SaleRepository(DefaultContext context, IMongoCollection<Sale> mongoCollection) : ISaleRepository
{
    public async Task<Sale> CreateAsync(Sale sale, CancellationToken cancellationToken = default)
    {
        await context.Sales.AddAsync(sale, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        await mongoCollection.InsertOneAsync(sale, cancellationToken: cancellationToken);
        return sale;
    }

    public async Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Sales
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Sale>> GetByCustomerNameAsync(string customerName, CancellationToken cancellationToken = default)
    {
        return await context.Sales
            .Where(s => s.Customer.CustomerName.Contains(customerName))
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var sale = await GetByIdAsync(id, cancellationToken);
        if (sale == null)
            return false;

        context.Sales.Remove(sale);
        await context.SaveChangesAsync(cancellationToken);

        var deleteResult = await mongoCollection.DeleteOneAsync(s => s.Id == id, cancellationToken);
        return deleteResult.DeletedCount > 0;
    }

    public async Task<Sale> UpdateAsync(Sale sale, CancellationToken cancellationToken = default)
    {
        context.Sales.Update(sale);
        await context.SaveChangesAsync(cancellationToken);

        await mongoCollection.ReplaceOneAsync(s => s.Id == sale.Id, sale, cancellationToken: cancellationToken);
        return sale;
    }
}