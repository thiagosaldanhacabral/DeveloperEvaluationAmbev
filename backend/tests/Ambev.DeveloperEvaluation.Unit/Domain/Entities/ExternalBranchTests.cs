using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Contains unit tests for the ExternalBranch entity class.
/// </summary>
public class ExternalBranchTests
{
    [Fact(DisplayName = "Validation should pass for valid ExternalBranch data")]
    public void Given_ValidExternalBranchData_When_Validated_Then_ShouldReturnValid()
    {
        // Arrange
        var branch = ExternalBranchTestData.GenerateValidExternalBranch();

        // Act
        var result = branch.Validate();

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact(DisplayName = "Validation should fail for invalid ExternalBranch data")]
    public void Given_InvalidExternalBranchData_When_Validated_Then_ShouldReturnInvalid()
    {
        // Arrange
        var branch = ExternalBranchTestData.GenerateInvalidExternalBranch();

        // Act
        var result = branch.Validate();

        // Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
    }
    

    [Fact(DisplayName = "ExternalBranch status should change to inactive.")]
    public void Given_Inactive_Then_StatusShouldBeInactive()
    {
        // Arrange
        var externalBranch = ExternalBranchTestData.GenerateValidExternalBranch();

        // Act
        externalBranch.Inactivate();

        // Assert
        
        Assert.False(externalBranch.IsActive);
    }
    
    [Fact(DisplayName = "ExternalBranch status should change to active.")]
    public void Given_Active_Then_StatusShouldBeActive()
    {
        // Arrange
        var externalBranch = ExternalBranchTestData.GenerateValidExternalBranch();

        // Act
        externalBranch.Activate();

        // Assert
        
        Assert.True(externalBranch.IsActive);
    }
}