using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;

/// <summary>
/// Validator for validating ExternalBranch data.
/// </summary>
public class UpdateExternalBranchValidator : AbstractValidator<UpdateSaleBranchRequest>
{
    /// <summary>
    /// Initializes a new instance of ExternalBranchValidator with defined validation rules.
    /// </summary>
    public UpdateExternalBranchValidator()
    {
        RuleFor(x => x.BranchName).NotEmpty().WithMessage("Branch Name must not be empty.")
            .Length(3, 100).WithMessage("Branch Name must be between 3 and 100 characters.");
        RuleFor(x => x.Location).NotEmpty().WithMessage("Branch Location must not be empty.")
            .Length(3, 150).WithMessage("Branch Location must be between 3 and 150 characters.");
    }
}