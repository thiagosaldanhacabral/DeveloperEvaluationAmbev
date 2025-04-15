using Ambev.DeveloperEvaluation.Common.Validation;

namespace Ambev.DeveloperEvaluation.Domain.Common;

/// <summary>
/// Represents the base class for all entities in the domain.
/// Provides basic functionality such as ID management and validation.
/// </summary>
public abstract class BaseEntity : IComparable<BaseEntity>
{
    /// <summary>
    /// Gets the unique identifier of the entity.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Performs validation on the entity using domain-specific rules.
    /// </summary>
    /// <returns>A collection of validation errors, if any.</returns>
    public Task<IEnumerable<ValidationErrorDetail>> ValidateAsync()
    {
        return Validator.ValidateAsync(this);
    }

    /// <summary>
    /// Compares the current entity with another entity based on their IDs.
    /// </summary>
    /// <param name="other">The other entity to compare.</param>
    /// <returns>An integer indicating the comparison result.</returns>
    public int CompareTo(BaseEntity? other)
    {
        return other == null ? 1 : other.Id.CompareTo(Id);
    }
    
    /// <summary>
    /// Gets the date and time when the user was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets the date and time of the last update to the user's information.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}
