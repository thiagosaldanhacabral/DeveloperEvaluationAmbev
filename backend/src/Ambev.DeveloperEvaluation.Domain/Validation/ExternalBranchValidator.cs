using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

/// <summary>
/// Validator for the <see cref="ExternalBranch"/> entity.
/// </summary>
public class ExternalBranchValidator : AbstractValidator<ExternalBranch>
{
    public ExternalBranchValidator()
    {
        RuleFor(branch => branch.BranchName)
            .NotEmpty()
            .WithMessage("Branch name must not be empty.")
            .MaximumLength(150)
            .WithMessage("Branch name cannot exceed 150 characters.");

        RuleFor(branch => branch.Location)
            .NotEmpty()
            .WithMessage("Branch location must not be empty.")
            .MaximumLength(250)
            .WithMessage("Branch location cannot exceed 250 characters.");
    }
}