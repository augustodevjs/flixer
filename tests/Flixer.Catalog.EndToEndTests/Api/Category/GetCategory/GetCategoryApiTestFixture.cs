using Flixer.Catalog.EndToEndTests.Api.Category.Common;

namespace Flixer.Catalog.EndToEndTests.Api.Category.GetCategory;

[CollectionDefinition(nameof(GetCategoryApiTestFixture))]
public class GetCategoryApiTestFixtureCollection : ICollectionFixture<GetCategoryApiTestFixture>
{

}

public class GetCategoryApiTestFixture : CategoryBaseFixture
{
}
