using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

/// <summary>
/// Provides test data generation for the ExternalBranch entity.
/// </summary>
public static class ExternalBranchTestData
{
    private static readonly Faker _faker = new Faker();
    public static ExternalBranch GenerateValidExternalBranch()
    {
        var externalBranch = new ExternalBranch(_faker.Company.CompanyName(), _faker.Address.FullAddress())
        {
            Id = Guid.NewGuid()
        };
        return externalBranch;
    }


    public static ExternalBranch GenerateInvalidExternalBranch()
    {
        {
            var externalBranch = new ExternalBranch(string.Empty, string.Empty)
            {
                Id = Guid.Empty
            };
            return externalBranch;
        }
    }
}