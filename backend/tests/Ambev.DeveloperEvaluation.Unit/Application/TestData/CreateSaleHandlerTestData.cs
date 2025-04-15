using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData;

/// <summary>
/// Provides methods for generating test data for CreateSaleHandler tests.
/// </summary>
public static class CreateSaleHandlerTestData
{
    private static Guid saleId { get; } = new Faker().Random.Guid();
    private static readonly ExternalCustomer _externalCustomer = ExternalCustomerTestData.GenerateValidExternalCustomer();
    private static readonly ExternalBranch _externalBranch = ExternalBranchTestData.GenerateValidExternalBranch();
    private static readonly ExternalProduct _externalProduct = ExternalProductTestData.GenerateValidExternalProduct();
    private static readonly Faker<CreateSaleCommand> createSaleHandlerFaker = new Faker<CreateSaleCommand>()
        .RuleFor(s => s.SaleNumber, f => f.Commerce.Ean13())
        .RuleFor(s => s.SaleDate, f => f.Date.Past())
        .RuleFor(s => s.Customer, _externalCustomer)
        .RuleFor(s => s.Branch, _externalBranch)
        .RuleFor(s => s.SaleItems, f => new List<SaleItem>
        {
            new SaleItem(saleId, f.Random.Int(1, 10), _externalProduct)
        });

    /// <summary>
    /// Generates a valid CreateSaleCommand.
    /// </summary>
    public static CreateSaleCommand GenerateValidCommand()
    {
        return createSaleHandlerFaker.Generate();
    }
}