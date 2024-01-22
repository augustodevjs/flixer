using Flixer.Catalog.EndToEndTests.Api.Category.Common;

namespace Flixer.Catalog.EndToEndTests.Api.Category.DeleteCategory;

[CollectionDefinition(nameof(DeleteCategoryTestFixture))]
public class DeleteCategoryApiTestFixtureCollection : ICollectionFixture<DeleteCategoryTestFixture>
{

}

public class DeleteCategoryTestFixture : CategoryBaseFixture
{
}
