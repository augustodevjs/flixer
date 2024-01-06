using Flixer.Catalog.UnitTest.Application.Common;

namespace Flixer.Catalog.UnitTest.Application.UseCases.GetCategoryUseCase;

[CollectionDefinition(nameof(GetCategoryUseCaseTestFixture))]
public class GetCategoryTestFixtureCollection :
    ICollectionFixture<GetCategoryUseCaseTestFixture>
{}

public class GetCategoryUseCaseTestFixture : CategoryUseCasesBaseFixture
{}
