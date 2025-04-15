using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

/// <summary>
/// Validator for the <see cref="ExternalCustomer"/> entity.
/// </summary>
public class ExternalCustomerValidator : AbstractValidator<ExternalCustomer>
{
    public ExternalCustomerValidator()
    {
        RuleFor(customer => customer.CustomerName)
            .NotEmpty()
            .WithMessage("Customer name must not be empty.")
            .MaximumLength(200)
            .WithMessage("Customer name cannot exceed 200 characters.");

        RuleFor(customer => customer.Email)
            .NotEmpty()
            .WithMessage("Email must not be empty.")
            .EmailAddress()
            .WithMessage("Invalid email format.");

        RuleFor(customer => customer.Phone)
            .MinimumLength(11)
            .MaximumLength(15)
            .WithMessage("Phone number must have 11-15 digits.");
    }
}