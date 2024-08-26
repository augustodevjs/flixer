// using Flixer.Catalog.EndToEndTests.Base;
// using Flixer.Catalog.Common.Tests.Fixture.Category;
//
// namespace Flixer.Catalog.EndToEndTests.Api.Category.Common;
//
// [CollectionDefinition(nameof(CategoryFixture))]
// public class CategoryFixtureCollection : ICollectionFixture<CategoryFixture>
// {
// }
//
// public class CategoryFixture : BaseFixture
// {
//     public CategoryPersistence Persistence;
//     public CategoryTestFixture CategoryTest;
//
//     public CategoryFixture() : base()
//     {
//         CategoryTest = new CategoryTestFixture();
//         Persistence = new CategoryPersistence(CreateDbContext());
//     }
// }