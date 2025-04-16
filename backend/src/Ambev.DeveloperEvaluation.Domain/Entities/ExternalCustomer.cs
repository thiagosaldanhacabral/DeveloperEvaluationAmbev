using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Validation;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a customer who makes purchases.
/// Includes customer details such as name and contact information.
/// </summary>
public class ExternalCustomer : BaseEntity
{
    /// <summary>
    /// Gets the name of the customer.
    /// </summary>
    public string CustomerName { get; private set; }

    /// <summary>
    /// Gets the contact email of the customer.
    /// </summary>
    public string Email { get; private set; }

    /// <summary>
    /// Gets the contact phone of the customer.
    /// </summary>
    public string Phone { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the item has been active.
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Inactive the customer.
    /// </summary>
    public void Inactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Active the customer.
    /// </summary>
    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public ExternalCustomer()
    {

    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ExternalCustomer"/> class.
    /// </summary>
    public ExternalCustomer(string customerName, string email, string phone)
    {
        CustomerName = customerName;
        Email = email;
        Phone = phone;
        CreatedAt = DateTime.UtcNow;
        IsActive = true;
    }

    /// <summary>
    /// Performs validation of the external customer entity using the ExternalCustomerValidator rules.
    /// </summary>
    /// <returns>
    /// A <see cref="ValidationResultDetail"/> containing:
    /// - IsValid: Indicates whether all validation rules passed
    /// - Errors: Collection of validation errors if any rules failed
    /// </returns>
    /// <remarks>
    /// <listheader>The validation includes checking:</listheader>
    /// <list type="bullet">Customer name is not empty</list>
    /// <list type="bullet">Customer name length does not exceed maximum</list>
    /// <list type="bullet">Email format is valid</list>
    /// <list type="bullet">Phone number format is valid</list>
    /// </remarks>
    public ValidationResultDetail Validate()
    {
        var validator = new ExternalCustomerValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }
}
