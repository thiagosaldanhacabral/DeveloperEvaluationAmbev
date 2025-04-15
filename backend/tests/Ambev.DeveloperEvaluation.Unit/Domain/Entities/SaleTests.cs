using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Contains unit tests for the Sale entity class.
/// Tests cover status changes and validation scenarios.
/// </summary>
public class SaleTests
{
    /// <summary>
    /// Tests that when a sale is marked as cancelled, the status is updated accordingly.
    /// </summary>
    [Fact(DisplayName = "Sale status should change to Cancelled when cancelled")]
    public void Given_ActiveSale_When_Cancelled_Then_StatusShouldBeCancelled()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();

        // Act
        sale.Cancel();

        // Assert

        Assert.True(sale.IsCancelled);
    }

    /// <summary>
    /// Tests that the total amount of a sale is calculated correctly based on its items.
    /// </summary>
    [Fact(DisplayName = "Sale total amount should be calculated correctly")]
    public void Given_SaleWithItems_When_TotalAmountCalculated_Then_ShouldBeCorrect()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();

        // Act
        sale.RecalculateTotalAmount();
        
        // Assert
        Assert.Equal(35, sale.TotalAmount);
    }

    /// <summary>
    /// Tests that validation passes when all sale properties are valid.
    /// </summary>
    [Fact(DisplayName = "Validation should pass for valid sale data")]
    public void Given_ValidSaleData_When_Validated_Then_ShouldReturnValid()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();

        // Act
        sale.RecalculateTotalAmount();
        var result = sale.Validate();
        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    /// <summary>
    /// Tests that validation fails when sale properties are invalid.
    /// </summary>
    [Fact(DisplayName = "Validation should fail for invalid sale data")]
    public void Given_InvalidSaleData_When_Validated_Then_ShouldReturnInvalid()
    {
        // Arrange
        var sale = new Sale(Guid.Empty, Guid.Empty, "", default,
            ExternalCustomerTestData.GenerateInvalidExternalCustomer(),
            ExternalBranchTestData.GenerateInvalidExternalBranch());

        // Act
        var result = sale.Validate();

        // Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
    }
}