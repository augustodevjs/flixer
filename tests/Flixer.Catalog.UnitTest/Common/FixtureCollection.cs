using Flixer.Catalog.Common.Tests.Fixture.Genre;
using Flixer.Catalog.Common.Tests.Fixture.Category;

namespace Flixer.Catalog.UnitTest.Common;

[CollectionDefinition(nameof(CategoryTestFixture))]
public class CategoryFixtureCollection : ICollectionFixture<CategoryTestFixture>
{

}

[CollectionDefinition(nameof(GenreTestFixture))]
public class GenreTestFixtureCollection : ICollectionFixture<GenreTestFixture>
{
}