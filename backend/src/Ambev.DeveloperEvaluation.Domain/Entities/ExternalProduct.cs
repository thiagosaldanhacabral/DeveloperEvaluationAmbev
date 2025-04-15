using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Validation;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a product in the system.
/// Includes details such as name and price.
/// </summary>
public class ExternalProduct : BaseEntity
{
    /// <summary>
    /// Gets the name of the product.
    /// </summary>
    public string ProductName { get; private set; }

    /// <summary>
    /// Gets the price of the product.
    /// </summary>
    public decimal Price { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ExternalProduct"/> class.
    /// </summary>
    public ExternalProduct(string productName, decimal price)
    {
        ProductName = productName;
        Price = price;
        CreatedAt = DateTime.UtcNow;
        IsActive = true;
    }
    
    /// <summary>
    /// Gets a value indicating whether the item has been active.
    /// </summary>
    public bool IsActive { get; private set; }
    
    /// <summary>
    /// Performs validation of the external product entity using the ExternalProductValidator rules.
    /// </summary>
    /// <returns>
    /// A <see cref="ValidationResultDetail"/> containing:
    /// - IsValid: Indicates whether all validation rules passed
    /// - Errors: Collection of validation errors if any rules failed
    /// </returns>
    /// <remarks>
    /// <listheader>The validation includes checking:</listheader>
    /// <list type="bullet">Product name is not empty</list>
    /// <list type="bullet">Product name length does not exceed maximum</list>
    /// <list type="bullet">Product price is a positive value</list>
    /// </remarks>
    public ValidationResultDetail Validate()
    {
        var validator = new ExternalProductValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }
    
    /// <summary>
    /// Inactive the product.
    /// </summary>
    public void Inactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }
    
    /// <summary>
    /// Active the product.
    /// </summary>
    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }
}
