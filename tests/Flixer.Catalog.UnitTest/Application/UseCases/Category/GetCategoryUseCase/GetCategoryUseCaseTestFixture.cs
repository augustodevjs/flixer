using Flixer.Catalog.UnitTest.Application.UseCases.Category.Common;

namespace Flixer.Catalog.UnitTest.Application.UseCases.Category.GetCategoryUseCase;

[CollectionDefinition(nameof(GetCategoryUseCaseTestFixture))]
public class GetCategoryTestFixtureCollection :
    ICollectionFixture<GetCategoryUseCaseTestFixture>
{ }

public class GetCategoryUseCaseTestFixture : CategoryUseCasesBaseFixture
{ }
