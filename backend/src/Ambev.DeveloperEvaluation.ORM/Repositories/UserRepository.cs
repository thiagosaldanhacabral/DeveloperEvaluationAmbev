using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

/// <summary>
/// Implementation of IUserRepository using Entity Framework Core
/// </summary>
public class UserRepository : IUserRepository
{
    private readonly DefaultContext _context;
    private readonly IMongoCollection<User> _mongoCollection;

    /// <summary>
    /// Initializes a new instance of UserRepository
    /// </summary>
    /// <param name="context">The database context</param>
    public UserRepository(DefaultContext context, IMongoCollection<User> mongoCollection)
    {
        _context = context;
        _mongoCollection = mongoCollection;
    }

    /// <summary>
    /// Creates a new user in the database
    /// </summary>
    /// <param name="user">The user to create</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created user</returns>
    public async Task<User> CreateAsync(User user, CancellationToken cancellationToken = default)
    {
        await _context.Users.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        await _mongoCollection.InsertOneAsync(user, cancellationToken: cancellationToken);

        return user;
    }

    /// <summary>
    /// Retrieves a user by their unique identifier
    /// </summary>
    /// <param name="id">The unique identifier of the user</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The user if found, null otherwise</returns>
    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Users.FirstOrDefaultAsync(o=> o.Id == id, cancellationToken);
    }

    /// <summary>
    /// Retrieves a user by their email address
    /// </summary>
    /// <param name="email">The email address to search for</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The user if found, null otherwise</returns>
    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    /// <summary>
    /// Deletes a user from the database
    /// </summary>
    /// <param name="id">The unique identifier of the user to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the user was deleted, false if not found</returns>
    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await GetByIdAsync(id, cancellationToken);
        if (user == null)
            return false;

        _context.Users.Remove(user);
        await _context.SaveChangesAsync(cancellationToken);

        var deleteResult = await _mongoCollection.DeleteOneAsync(u => u.Id == id, cancellationToken);

        return deleteResult.DeletedCount > 0;
    }

    /// <summary>
    /// Retrieves a user by their unique identifier from MongoDB.
    /// </summary>
    /// <param name="id">The unique identifier of the user.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The user if found, null otherwise.</returns>
    public async Task<User?> GetByIdFromMongoAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _mongoCollection.Find(u => u.Id == id).FirstOrDefaultAsync(cancellationToken);
    }

    /// <summary>
    /// Retrieves a user by their email address from MongoDB.
    /// </summary>
    /// <param name="email">The email address to search for.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The user if found, null otherwise.</returns>
    public async Task<User?> GetByEmailFromMongoAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _mongoCollection.Find(u => u.Email == email).FirstOrDefaultAsync(cancellationToken);
    }

    /// <summary>
    /// Retrieves all users from MongoDB.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A list of all users.</returns>
    public async Task<IEnumerable<User>> GetAllFromMongoAsync(CancellationToken cancellationToken = default)
    {
        return await _mongoCollection.Find(_ => true).ToListAsync(cancellationToken);
    }
}
