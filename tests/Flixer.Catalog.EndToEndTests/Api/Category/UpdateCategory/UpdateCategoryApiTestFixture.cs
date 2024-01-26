using Flixer.Catalog.EndToEndTests.Api.Category.Common;
using Flixer.Catalog.Application.Dtos.InputModel.Category;

namespace Flixer.Catalog.EndToEndTests.Api.Category.UpdateCategory;

[CollectionDefinition(nameof(UpdateCategoryApiTestFixture))]
public class UpdateCategoryApiTestFixtureCollection : ICollectionFixture<UpdateCategoryApiTestFixture>
{ 

}

public class UpdateCategoryApiTestFixture : CategoryBaseFixture
{
    public UpdateCategoryInputModel GetExampleInput(Guid? id = null)
        => new(
            id ?? Guid.NewGuid(),
            GetValidCategoryName(),
            GetRandomBoolean(),
            GetValidCategoryDescription()
        );
}