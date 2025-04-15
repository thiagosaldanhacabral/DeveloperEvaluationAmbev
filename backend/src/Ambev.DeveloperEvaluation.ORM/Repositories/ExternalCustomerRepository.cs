using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

/// <summary>
/// Implementation of IExternalCustomerRepository using Entity Framework Core.
/// </summary>
public class ExternalCustomerRepository : IExternalCustomerRepository
{
    private readonly DefaultContext _context;
    private readonly IMongoCollection<ExternalCustomer> _mongoCollection;

    public ExternalCustomerRepository(DefaultContext context, IMongoCollection<ExternalCustomer> mongoCollection)
    {
        _context = context;
        _mongoCollection = mongoCollection;
    }

    public async Task<ExternalCustomer> CreateAsync(ExternalCustomer customer, CancellationToken cancellationToken = default)
    {
        await _context.ExternalCustomers.AddAsync(customer, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        await _mongoCollection.InsertOneAsync(customer, cancellationToken: cancellationToken);
        return customer;
    }

    public async Task<ExternalCustomer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.ExternalCustomers.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<ExternalCustomer>> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _context.ExternalCustomers
            .Where(c => c.CustomerName.Contains(name))
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var customer = await GetByIdAsync(id, cancellationToken);
        if (customer == null)
            return false;

        _context.ExternalCustomers.Remove(customer);
        await _context.SaveChangesAsync(cancellationToken);

        var deleteResult = await _mongoCollection.DeleteOneAsync(c => c.Id == id, cancellationToken);
        return deleteResult.DeletedCount > 0;
    }

    public async Task<ExternalCustomer> UpdateAsync(ExternalCustomer customer, CancellationToken cancellationToken = default)
    {
        _context.ExternalCustomers.Update(customer);
        await _context.SaveChangesAsync(cancellationToken);

        await _mongoCollection.ReplaceOneAsync(c => c.Id == customer.Id, customer, cancellationToken: cancellationToken);
        return customer;
    }

    public async Task<ExternalCustomer?> GetByIdFromMongoAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _mongoCollection.Find(c => c.Id == id).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<ExternalCustomer>> GetByNameFromMongoAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _mongoCollection.Find(c => c.CustomerName.Contains(name)).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ExternalCustomer>> GetAllFromMongoAsync(CancellationToken cancellationToken = default)
    {
        return await _mongoCollection.Find(_ => true).ToListAsync(cancellationToken);
    }
}