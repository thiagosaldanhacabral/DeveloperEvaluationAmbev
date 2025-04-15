using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Contains unit tests for the SaleItem entity class.
/// </summary>
public class SaleItemTests
{
    [Fact(DisplayName = "Validation should pass for valid SaleItem data")]
    public void Given_ValidSaleItemData_When_Validated_Then_ShouldReturnValid()
    {
        // Arrange
        var saleItem = SaleItemTestData.GenerateValidSaleItem();

        // Act
        var result = saleItem.Validate();

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact(DisplayName = "Validation should fail for invalid SaleItem data")]
    public void Given_InvalidSaleItemData_When_Validated_Then_ShouldReturnInvalid()
    {
        // Arrange
        var saleItem = SaleItemTestData.GenerateInvalidSaleItem();

        // Act
        var result = saleItem.Validate();

        // Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
    }
    
    [Fact(DisplayName = "SaleItem status should change to Cancelled when cancelled")]
    public void Given_ActiveSale_When_Cancelled_Then_StatusShouldBeCancelled()
    {
        // Arrange
        var saleItem = SaleItemTestData.GenerateValidSaleItem();

        // Act
        saleItem.CancelItem();

        // Assert
        
        Assert.True(saleItem.IsCancelled);
    }
}