using Flixer.Catalog.UnitTest.Application.UseCases.Category.Common;

namespace Flixer.Catalog.UnitTest.Application.UseCases.Category.DeleteCategoryUseCase;

[CollectionDefinition(nameof(DeleteCategoryUseCaseTestFixture))]
public class DeleteCategoryUseCaseTestFixtureCollection : ICollectionFixture<DeleteCategoryUseCaseTestFixture>
{

}

public class DeleteCategoryUseCaseTestFixture : CategoryUseCasesBaseFixture
{
}