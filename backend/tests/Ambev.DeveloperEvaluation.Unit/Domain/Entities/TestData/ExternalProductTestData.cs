using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

/// <summary>
/// Provides test data generation for the ExternalProduct entity.
/// </summary>
public static class ExternalProductTestData
{
    private static readonly Faker _faker = new Faker();
    
    public static ExternalProduct GenerateValidExternalProduct()
    {
        return new ExternalProduct(_faker.Commerce.ProductName(), _faker.Random.Decimal(1, 10000))
        {
            Id = Guid.NewGuid()
        };
    }
        
    public static ExternalProduct GenerateInvalidExternalProduct()
    {
        return new ExternalProduct(string.Empty, -10)
        {
            Id = Guid.Empty
        };
    }
}