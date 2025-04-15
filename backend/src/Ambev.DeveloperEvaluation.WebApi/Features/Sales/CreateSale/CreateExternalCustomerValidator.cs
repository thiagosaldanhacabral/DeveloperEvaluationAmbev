using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

/// <summary>
/// Validator for validating ExternalCustomer data.
/// </summary>
public class CreateExternalCustomerValidator : AbstractValidator<CreateSaleCustomerRequest>
{
    /// <summary>
    /// Initializes a new instance of ExternalCustomerValidator with defined validation rules.
    /// </summary>
    public CreateExternalCustomerValidator()
    {
        RuleFor(x => x.CustomerName).NotEmpty().WithMessage("Customer Name must not be empty.")
            .Length(3, 100).WithMessage("Customer Name must be between 3 and 100 characters.");
        RuleFor(x => x.Email).NotEmpty().WithMessage("Customer Email must not be empty.")
            .EmailAddress().WithMessage("Customer Email must be a valid email address.");
        RuleFor(x => x.Phone).Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Customer Phone must match international format (+X XXXXXXXXXX).");
    }
}