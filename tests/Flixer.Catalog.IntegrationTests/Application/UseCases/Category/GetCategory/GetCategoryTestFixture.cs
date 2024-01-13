using Flixer.Catalog.IntegrationTests.Application.UseCases.Category.Common;

namespace Flixer.Catalog.IntegrationTests.Application.UseCases.Category.GetCategory;

[CollectionDefinition(nameof(GetCategoryTestFixture))]
public class GetCategoryTestFixtureCollection : ICollectionFixture<GetCategoryTestFixture>
{ 
}

public class GetCategoryTestFixture : CategoryUseCaseBaseFixture
{
}