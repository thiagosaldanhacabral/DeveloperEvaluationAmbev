using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

/// <summary>
/// Implementation of IExternalCustomerRepository using Entity Framework Core.
/// </summary>
public class ExternalCustomerRepository : IExternalCustomerRepository
{
    private readonly DefaultContext _context;

    public ExternalCustomerRepository(DefaultContext context)
    {
        _context = context;
    }

    public async Task<ExternalCustomer> CreateAsync(ExternalCustomer customer, CancellationToken cancellationToken = default)
    {
        await _context.ExternalCustomers.AddAsync(customer, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
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
        return true;
    }

    public async Task<ExternalCustomer> UpdateAsync(ExternalCustomer customer, CancellationToken cancellationToken = default)
    {
        _context.ExternalCustomers.Update(customer);
        await _context.SaveChangesAsync(cancellationToken);
        return customer;
    }
}