using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

/// <summary>
/// Provides methods for generating test data for the Sale entity.
/// </summary>
public static class SaleTestData
{
    private static readonly Guid _saleId = new Faker().Random.Guid();

    private static readonly Faker _faker = new Faker();
    
    private static readonly ExternalCustomer _externalCustomer =
        ExternalCustomerTestData.GenerateValidExternalCustomer();

    private static readonly ExternalBranch _externalBranch = ExternalBranchTestData.GenerateValidExternalBranch();
    private static readonly ExternalProduct product1 = new ExternalProduct(new Faker().Commerce.ProductName(), 10)
    {
        Id = Guid.NewGuid()
    };
    private static readonly ExternalProduct product2 = new ExternalProduct(new Faker().Commerce.ProductName(), 15)
    {
        Id = Guid.NewGuid()
    };
    /// <summary>
    /// Configures the Faker to generate valid Sale entities.
    /// </summary>
    private static readonly Faker<Sale> SaleFaker = new Faker<Sale>()
        .RuleFor(s => s.SaleNumber, f => f.Random.AlphaNumeric(10))
        .RuleFor(s => s.SaleDate, f => f.Date.Recent())
        .RuleFor(s => s.Customer, _externalCustomer)
        .RuleFor(s => s.Branch, _externalBranch)
        .RuleFor(s => s.Items, f => new()
        {
            new SaleItem(_saleId,2, product1),
            new SaleItem(_saleId,1, product2),
        });

    /// <summary>
    /// Generates a valid Sale entity with randomized data.
    /// </summary>
    /// <returns>A valid Sale entity.</returns>
    public static Sale GenerateValidSale()
    {
        var sale = new Sale(_externalCustomer.Id, _externalBranch.Id, _faker.Random.AlphaNumeric(10), _faker.Date.Recent(), _externalCustomer, _externalBranch)
        {
            Id = _saleId,
        };
        sale.AddItem(new SaleItem(_saleId,2, product1));
        sale.AddItem(new SaleItem(_saleId,1, product2));
        sale.RecalculateTotalAmount();
        return sale;
    }
}