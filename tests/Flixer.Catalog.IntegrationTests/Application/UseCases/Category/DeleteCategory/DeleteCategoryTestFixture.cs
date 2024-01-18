using Flixer.Catalog.IntegrationTests.Application.UseCases.Category.Common;

namespace Flixer.Catalog.IntegrationTests.Application.UseCases.Category.DeleteCategory;

[CollectionDefinition(nameof(DeleteCategoryTestFixture))]
public class DeleteCategoryTestFixtureCollection : ICollectionFixture<DeleteCategoryTestFixture>
{
}

public class DeleteCategoryTestFixture : CategoryUseCaseBaseFixture
{
}