using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

/// <summary>
/// Provides test data generation for the SaleItem entity.
/// </summary>
public static class SaleItemTestData
{
    private static readonly ExternalProduct fakeProduct = ExternalProductTestData.GenerateValidExternalProduct();
    
    public static SaleItem GenerateValidSaleItem() => new SaleItem(Guid.NewGuid(), new Faker().Random.Int(1,20), fakeProduct);

    public static SaleItem GenerateInvalidSaleItem() => new SaleItem(Guid.Empty, 0, ExternalProductTestData.GenerateInvalidExternalProduct());
}