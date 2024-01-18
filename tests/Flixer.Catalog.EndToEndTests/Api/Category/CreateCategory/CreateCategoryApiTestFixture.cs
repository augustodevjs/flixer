using Flixer.Catalog.Application.Dtos.InputModel.Category;
using Flixer.Catalog.EndToEndTests.Api.Category.Common;

namespace Flixer.Catalog.EndToEndTests.Api.Category.CreateCategory;

[CollectionDefinition(nameof(CreateCategoryApiTestFixture))]
public class CreateCategoryApiTestFixtureCollection : ICollectionFixture<CreateCategoryApiTestFixture>
{
}

public class CreateCategoryApiTestFixture : CategoryBaseFixture
{
    public CreateCategoryInputModel getExampleInput()
        => new(
            GetValidCategoryName(),
            GetValidCategoryDescription(),
            GetRandomBoolean()
        );

}