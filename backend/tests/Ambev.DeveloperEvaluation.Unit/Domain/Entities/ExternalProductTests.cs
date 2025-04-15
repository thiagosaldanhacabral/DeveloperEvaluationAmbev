using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Contains unit tests for the ExternalProduct entity class.
/// </summary>
public class ExternalProductTests
{
    [Fact(DisplayName = "Validation should pass for valid ExternalProduct data")]
    public void Given_ValidExternalProductData_When_Validated_Then_ShouldReturnValid()
    {
        // Arrange
        var product = ExternalProductTestData.GenerateValidExternalProduct();

        // Act
        var result = product.Validate();

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact(DisplayName = "Validation should fail for invalid ExternalProduct data")]
    public void Given_InvalidExternalProductData_When_Validated_Then_ShouldReturnInvalid()
    {
        // Arrange
        var product = ExternalProductTestData.GenerateInvalidExternalProduct();

        // Act
        var result = product.Validate();

        // Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
    }
    
    [Fact(DisplayName = "ExternalProduct status should change to inactive.")]
    public void Given_Inactive_Then_StatusShouldBeInactive()
    {
        // Arrange
        var externalProduct = ExternalProductTestData.GenerateValidExternalProduct();

        // Act
        externalProduct.Inactivate();

        // Assert
        
        Assert.False(externalProduct.IsActive);
    }
    
    [Fact(DisplayName = "ExternalProduct status should change to active.")]
    public void Given_Active_Then_StatusShouldBeActive()
    {
        // Arrange
        var externalProduct = ExternalProductTestData.GenerateValidExternalProduct();

        // Act
        externalProduct.Activate();

        // Assert
        
        Assert.True(externalProduct.IsActive);
    }
}