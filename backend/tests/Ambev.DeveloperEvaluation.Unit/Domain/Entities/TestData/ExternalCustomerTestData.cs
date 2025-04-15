using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

/// <summary>
/// Provides test data generation for the ExternalCustomer entity.
/// </summary>
public static class ExternalCustomerTestData
{
    private static readonly Faker _faker = new Faker();

    public static ExternalCustomer GenerateValidExternalCustomer()
    {
        return new ExternalCustomer(_faker.Name.FullName(), _faker.Internet.Email(), _faker.Phone.PhoneNumberFormat())
        {
            Id = Guid.NewGuid()
        };
    }

    public static ExternalCustomer GenerateInvalidExternalCustomer()
    {
        return new ExternalCustomer("", "invalid-email", "invalid-phone")
        {
            Id = Guid.NewGuid()
        };
    }
}