// using Xunit;
// using Flixer.Catalog.EndToEndTests.Persistence;
// using Domain_CategoryFixture = Flixer.Catalog.UnitTest.Fixture.Domain.CategoryFixture;
//
// namespace Flixer.Catalog.EndToEndTests.Fixtures.Category;
//
// [CollectionDefinition(nameof(CategoryFixture))]
// public class CategoryFixtureCollection : ICollectionFixture<CategoryFixture>
// {
//     
// }
//
// public class CategoryFixture : BaseFixture
// {
//     public CategoryPersistence Persistence;
//     public Domain_CategoryFixture DomainCategoryFixture;
//
//     public CategoryFixture(CategoryPersistence persistence, Domain_CategoryFixture domainCategoryFixture)
//     {
//         Persistence = new CategoryPersistence(CreateDbContext("e2e-tests-db"));
//         DomainCategoryFixture = new Domain_CategoryFixture();
//     }
// }