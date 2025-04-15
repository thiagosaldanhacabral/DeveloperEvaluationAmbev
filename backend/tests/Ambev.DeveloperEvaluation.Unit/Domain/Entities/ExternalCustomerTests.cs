using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Contains unit tests for the ExternalCustomer entity class.
/// </summary>
public class ExternalCustomerTests
{
    [Fact(DisplayName = "Validation should pass for valid ExternalCustomer data")]
    public void Given_ValidExternalCustomerData_When_Validated_Then_ShouldReturnValid()
    {
        // Arrange
        var customer = ExternalCustomerTestData.GenerateValidExternalCustomer();

        // Act
        var result = customer.Validate();

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact(DisplayName = "Validation should fail for invalid ExternalCustomer data")]
    public void Given_InvalidExternalCustomerData_When_Validated_Then_ShouldReturnInvalid()
    {
        // Arrange
        var customer = ExternalCustomerTestData.GenerateInvalidExternalCustomer();

        // Act
        var result = customer.Validate();

        // Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
    }
    
    [Fact(DisplayName = "ExternalCustomer status should change to inactive.")]
    public void Given_Inactive_Then_StatusShouldBeInactive()
    {
        // Arrange
        var externalCustomer = ExternalCustomerTestData.GenerateValidExternalCustomer();

        // Act
        externalCustomer.Inactivate();

        // Assert
        
        Assert.False(externalCustomer.IsActive);
    }
    
    [Fact(DisplayName = "ExternalCustomer status should change to active.")]
    public void Given_Active_Then_StatusShouldBeActive()
    {
        // Arrange
        var externalCustomer = ExternalCustomerTestData.GenerateValidExternalCustomer();

        // Act
        externalCustomer.Activate();

        // Assert
        
        Assert.True(externalCustomer.IsActive);
    }
}