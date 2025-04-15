using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

/// <summary>
/// Implementation of IExternalBranchRepository using Entity Framework Core and MongoDB.
/// </summary>
public class ExternalBranchRepository : IExternalBranchRepository
{
    private readonly DefaultContext _context;
    private readonly IMongoCollection<ExternalBranch> _mongoCollection;

    /// <summary>
    /// Initializes a new instance of ExternalBranchRepository.
    /// </summary>
    /// <param name="context">The database context.</param>
    /// <param name="mongoDbContext">The MongoDB context.</param>
    public ExternalBranchRepository(DefaultContext context, MongoDbContext mongoDbContext)
    {
        _context = context;
        _mongoCollection = mongoDbContext.GetCollection<ExternalBranch>("ExternalBranches");
    }

    /// <summary>
    /// Creates a new branch in the database and MongoDB.
    /// </summary>
    /// <param name="branch">The branch to create.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The created branch.</returns>
    public async Task<ExternalBranch> CreateAsync(ExternalBranch branch, CancellationToken cancellationToken = default)
    {
        await _context.ExternalBranches.AddAsync(branch, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        await _mongoCollection.InsertOneAsync(branch, cancellationToken: cancellationToken);

        return branch;
    }

    /// <summary>
    /// Retrieves a branch by its unique identifier from PostgreSQL.
    /// </summary>
    /// <param name="id">The unique identifier of the branch.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The branch if found, null otherwise.</returns>
    public async Task<ExternalBranch?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.ExternalBranches.FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
    }

    /// <summary>
    /// Retrieves a branch by its unique identifier from MongoDB.
    /// </summary>
    /// <param name="id">The unique identifier of the branch.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The branch if found, null otherwise.</returns>
    public async Task<ExternalBranch?> GetByIdFromMongoAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _mongoCollection.Find(b => b.Id == id).FirstOrDefaultAsync(cancellationToken);
    }

    /// <summary>
    /// Retrieves branches by their name from PostgreSQL.
    /// </summary>
    /// <param name="name">The name of the branch to search for.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A list of branches that match the name.</returns>
    public async Task<IEnumerable<ExternalBranch>> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _context.ExternalBranches
            .Where(b => b.BranchName.Contains(name))
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Retrieves all branches from MongoDB.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A list of all branches.</returns>
    public async Task<IEnumerable<ExternalBranch>> GetAllFromMongoAsync(CancellationToken cancellationToken = default)
    {
        return await _mongoCollection.Find(_ => true).ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Deletes a branch from the database and MongoDB.
    /// </summary>
    /// <param name="id">The unique identifier of the branch to delete.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>True if the branch was deleted, false if not found.</returns>
    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var branch = await GetByIdAsync(id, cancellationToken);
        if (branch == null)
            return false;

        _context.ExternalBranches.Remove(branch);
        await _context.SaveChangesAsync(cancellationToken);

        var deleteResult = await _mongoCollection.DeleteOneAsync(b => b.Id == id, cancellationToken);
        return deleteResult.DeletedCount > 0;
    }

    /// <summary>
    /// Updates an existing branch in the database and MongoDB.
    /// </summary>
    /// <param name="branch">The branch to update.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The updated branch.</returns>
    public async Task<ExternalBranch> UpdateAsync(ExternalBranch branch, CancellationToken cancellationToken = default)
    {
        _context.ExternalBranches.Update(branch);
        await _context.SaveChangesAsync(cancellationToken);

        await _mongoCollection.ReplaceOneAsync(b => b.Id == branch.Id, branch, cancellationToken: cancellationToken);

        return branch;
    }
}
