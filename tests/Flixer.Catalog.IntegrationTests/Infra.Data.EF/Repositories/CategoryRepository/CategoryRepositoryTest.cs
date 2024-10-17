// using Xunit;
// using FluentAssertions;
// using Flixer.Catalog.Domain.Enums;
// using Flixer.Catalog.Domain.SeedWork.SearchableRepository;
// using Flixer.Catalog.IntegrationTests.Fixtures.Repository;
//
// namespace Flixer.Catalog.IntegrationTests.Infra.Data.EF.Repositories.CategoryRepository;
//
// [Collection(nameof(CategoryRepositoryFixture))]
// public class CategoryRepositoryTest
// {
//     private readonly CategoryRepositoryFixture _fixture;
//     private const string NameDbContext = "integration-tests-repository";
//
//     public CategoryRepositoryTest(CategoryRepositoryFixture fixture)
//     {
//         _fixture = fixture;
//     }
//
//     [Fact]
//     [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
//     public async Task CategoryRepository_Create()
//     {
//         var dbContext = _fixture.CreateDbContext(NameDbContext);
//         var exampleCategory = _fixture.DataGenerator.GetValidCategory();
//         var categoryRepository = new Catalog.Infra.Data.EF.Repositories.CategoryRepository(dbContext);
//
//         categoryRepository.Create(exampleCategory);
//         await dbContext.SaveChangesAsync(CancellationToken.None);
//
//         var dbCategory = await _fixture.CreateDbContext(NameDbContext, true)
//             .Categories.FindAsync(exampleCategory.Id);
//         
//         dbCategory.Should().NotBeNull();
//         dbCategory?.Name.Should().Be(exampleCategory.Name);
//         dbCategory?.IsActive.Should().Be(exampleCategory.IsActive);
//         dbCategory?.CreatedAt.Should().Be(exampleCategory.CreatedAt);
//         dbCategory?.Description.Should().Be(exampleCategory.Description);
//     }
//     
//     [Fact]
//     [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
//     public async Task CategoryRepository_GetById()
//     {
//         var dbContext = _fixture.CreateDbContext(NameDbContext);
//         var exampleCategory = _fixture.DataGenerator.GetValidCategory();
//
//         await dbContext.Categories.AddAsync(exampleCategory);
//         await dbContext.SaveChangesAsync(CancellationToken.None);
//         
//         var categoryRepository = new Catalog.Infra.Data.EF.Repositories.CategoryRepository(
//             _fixture.CreateDbContext(NameDbContext, true)
//         );
//
//         var dbCategory = await categoryRepository.GetById(exampleCategory.Id);
//         
//         dbCategory.Should().NotBeNull();
//         dbCategory?.Id.Should().Be(exampleCategory.Id);
//         dbCategory?.Name.Should().Be(exampleCategory.Name);
//         dbCategory?.IsActive.Should().Be(exampleCategory.IsActive);
//         dbCategory?.CreatedAt.Should().Be(exampleCategory.CreatedAt);
//         dbCategory?.Description.Should().Be(exampleCategory.Description);
//     }
//     
//     [Fact]
//     [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
//     public async Task CategoryRepository_GetIdsListByIds()
//     {
//         var dbContext = _fixture.CreateDbContext(NameDbContext);
//         var examplesCategoriesList = _fixture.DataGenerator.GetExampleCategoriesList();
//         var exampleCategoriesGuids = examplesCategoriesList.Select(x => x.Id).ToList();
//
//         await dbContext.Categories.AddRangeAsync(examplesCategoriesList);
//         await dbContext.SaveChangesAsync(CancellationToken.None);
//         
//         var categoryRepository = new Catalog.Infra.Data.EF.Repositories.CategoryRepository(
//             _fixture.CreateDbContext(NameDbContext, true)
//         );
//
//         var dbCategory = await categoryRepository.GetIdsListByIds(exampleCategoriesGuids);
//     
//         dbCategory.Should().NotBeNull();
//         dbCategory.Count.Should().BeGreaterOrEqualTo(exampleCategoriesGuids.Count);
//     }
//     
//     [Fact]
//     [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
//     public async Task CategoryRepository_GetListByIdsAsync()
//     {
//         var dbContext = _fixture.CreateDbContext(NameDbContext);
//         var examplesCategoriesList = _fixture.DataGenerator.GetExampleCategoriesList();
//         var exampleCategoriesGuids = examplesCategoriesList.Select(x => x.Id).ToList();
//
//         await dbContext.Categories.AddRangeAsync(examplesCategoriesList);
//         await dbContext.SaveChangesAsync(CancellationToken.None);
//         
//         var categoryRepository = new Catalog.Infra.Data.EF.Repositories.CategoryRepository(
//             _fixture.CreateDbContext(NameDbContext, true)
//         );
//
//         var dbCategory = await categoryRepository.GetListByIdsAsync(exampleCategoriesGuids);
//     
//         dbCategory.Should().NotBeNull();
//         dbCategory.Should().BeEquivalentTo(examplesCategoriesList);
//     }
//     
//     [Fact]
//     [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
//     public async Task CategoryRepository_Update()
//     {
//         var dbContext = _fixture.CreateDbContext(NameDbContext);
//         var exampleCategory = _fixture.DataGenerator.GetValidCategory();
//         var newExampleCategory = _fixture.DataGenerator.GetValidCategory();
//
//         await dbContext.Categories.AddAsync(exampleCategory);
//         await dbContext.SaveChangesAsync(CancellationToken.None);
//         
//         var categoryRepository = new Catalog.Infra.Data.EF.Repositories.CategoryRepository(dbContext);
//         
//         exampleCategory.Update(newExampleCategory.Name, newExampleCategory.Description);
//
//         categoryRepository.Update(exampleCategory);
//         await dbContext.SaveChangesAsync();
//         
//         var dbCategory = await _fixture.CreateDbContext(NameDbContext, true)
//             .Categories.FindAsync(exampleCategory.Id);
//         
//         dbCategory.Should().NotBeNull();
//         dbCategory?.Id.Should().Be(exampleCategory.Id);
//         dbCategory?.Name.Should().Be(exampleCategory.Name);
//         dbCategory?.IsActive.Should().Be(exampleCategory.IsActive);
//         dbCategory?.CreatedAt.Should().Be(exampleCategory.CreatedAt);
//         dbCategory?.Description.Should().Be(exampleCategory.Description);
//     }
//     
//     [Fact]
//     [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
//     public async Task CategoryRepository_Delete()
//     {
//         var dbContext = _fixture.CreateDbContext(NameDbContext);
//         var exampleCategory = _fixture.DataGenerator.GetValidCategory();
//
//         await dbContext.Categories.AddAsync(exampleCategory);
//         await dbContext.SaveChangesAsync(CancellationToken.None);
//         
//         var categoryRepository = new Catalog.Infra.Data.EF.Repositories.CategoryRepository(dbContext);
//         
//         categoryRepository.Delete(exampleCategory);
//         await dbContext.SaveChangesAsync();
//         
//         var dbCategory = await _fixture.CreateDbContext(NameDbContext, true)
//             .Categories.FindAsync(exampleCategory.Id);
//         
//         dbCategory.Should().BeNull();
//     }
//     
//     [Fact]
//     [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
//     public async Task CategoryRepository_SearchReturnsListAndTotal()
//     {
//         var dbContext = _fixture.CreateDbContext(NameDbContext);
//         var exampleCategoriesList = _fixture.DataGenerator.GetExampleCategoriesList(15);
//
//         foreach (var category in exampleCategoriesList)
//         {
//             await dbContext.Categories.AddAsync(category);
//         }
//         
//         await dbContext.SaveChangesAsync(CancellationToken.None);
//         
//         var categoryRepository = new Catalog.Infra.Data.EF.Repositories.CategoryRepository(dbContext);
//
//         var searchInput = new SearchInput(1, 20, "", "", SearchOrder.Asc);
//         
//         var output = await categoryRepository.Search(searchInput);
//
//         output.Should().NotBeNull();
//         output.Items.Should().NotBeNull();
//         output.PerPage.Should().Be(searchInput.PerPage);
//         output.CurrentPage.Should().Be(searchInput.Page);
//         output.Total.Should().Be(exampleCategoriesList.Count);
//         output.Items.Should().HaveCount(exampleCategoriesList.Count);
//
//         foreach (var outputItem in output.Items)
//         {
//             var exampleItem = exampleCategoriesList.Find(
//                 category => category.Id == outputItem.Id
//             );
//             
//             exampleItem.Should().NotBeNull();
//             outputItem.Name.Should().Be(exampleItem!.Name);
//             outputItem.Description.Should().Be(exampleItem.Description);
//             outputItem.IsActive.Should().Be(exampleItem.IsActive);
//             outputItem.CreatedAt.Should().Be(exampleItem.CreatedAt);
//         }
//     }
//     
//     [Fact]
//     [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
//     public async Task CategoryRepository_SearchRetursEmptyWhenPersistenceIsEmpty()
//     {
//         var dbContext = _fixture.CreateDbContext(NameDbContext);
//         var categoryRepository = new Catalog.Infra.Data.EF.Repositories.CategoryRepository(dbContext);
//         var searchInput = new SearchInput(1, 20, "", "", SearchOrder.Asc);
//
//         var output = await categoryRepository.Search(searchInput);
//
//         output.Should().NotBeNull();
//         output.Items.Should().NotBeNull();
//         output.CurrentPage.Should().Be(searchInput.Page);
//         output.PerPage.Should().Be(searchInput.PerPage);
//         output.Total.Should().Be(0);
//         output.Items.Should().HaveCount(0);
//     }
//     
//     [Theory]
//     [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
//     [InlineData(10, 1, 5, 5)]
//     [InlineData(10, 2, 5, 5)]
//     [InlineData(7, 2, 5, 2)]
//     [InlineData(7, 3, 5, 0)]
//     public async Task CategoryRepository_SearchRetursPaginated(
//         int quantityCategoriesToGenerate,
//         int page,
//         int perPage,
//         int expectedQuantityItems
//     )
//     {
//         var dbContext = _fixture.CreateDbContext(NameDbContext);
//         var exampleCategoriesList = 
//             _fixture.DataGenerator.GetExampleCategoriesList(quantityCategoriesToGenerate);
//         
//         await dbContext.AddRangeAsync(exampleCategoriesList);
//         await dbContext.SaveChangesAsync(CancellationToken.None);
//         
//         var categoryRepository = new Catalog.Infra.Data.EF.Repositories.CategoryRepository(dbContext);
//         var searchInput = new SearchInput(page, perPage, "", "", SearchOrder.Asc);
//
//         var output = await categoryRepository.Search(searchInput);
//
//         output.Should().NotBeNull();
//         output.Items.Should().NotBeNull();
//         output.CurrentPage.Should().Be(searchInput.Page);
//         output.PerPage.Should().Be(searchInput.PerPage);
//         output.Total.Should().Be(quantityCategoriesToGenerate);
//         output.Items.Should().HaveCount(expectedQuantityItems);
//         foreach (var outputItem in output.Items)
//         {
//             var exampleItem = exampleCategoriesList.Find(
//                 category => category.Id == outputItem.Id
//             );
//             exampleItem.Should().NotBeNull();
//             outputItem.Name.Should().Be(exampleItem!.Name);
//             outputItem.Description.Should().Be(exampleItem.Description);
//             outputItem.IsActive.Should().Be(exampleItem.IsActive);
//             outputItem.CreatedAt.Should().Be(exampleItem.CreatedAt);
//         }
//     }
//     
//     [Theory]
//     [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
//     [InlineData("Action", 1, 5, 1, 1)]
//     [InlineData("Horror", 1, 5, 3, 3)]
//     [InlineData("Horror", 2, 5, 0, 3)]
//     [InlineData("Sci-fi", 1, 5, 4, 4)]
//     [InlineData("Sci-fi", 1, 2, 2, 4)]
//     [InlineData("Sci-fi", 2, 3, 1, 4)]
//     [InlineData("Sci-fi Other", 1, 3, 0, 0)]
//     [InlineData("Robots", 1, 5, 2, 2)]
//     public async Task CategoryRepository_SearchByText(
//         string search,
//         int page,
//         int perPage,
//         int expectedQuantityItemsReturned,
//         int expectedQuantityTotalItems
//     )
//     {
//         var dbContext = _fixture.CreateDbContext(NameDbContext);
//         
//         var exampleCategoriesList =
//             _fixture.DataGenerator.GetExampleCategoriesListWithNames(new List<string>() { 
//                 "Action",
//                 "Horror",
//                 "Horror - Robots",
//                 "Horror - Based on Real Facts",
//                 "Drama",
//                 "Sci-fi IA",
//                 "Sci-fi Space",
//                 "Sci-fi Robots",
//                 "Sci-fi Future"
//             });
//         await dbContext.AddRangeAsync(exampleCategoriesList);
//         await dbContext.SaveChangesAsync(CancellationToken.None);
//         
//         var categoryRepository = new Catalog.Infra.Data.EF.Repositories.CategoryRepository(dbContext);
//         var searchInput = new SearchInput(page, perPage, search, "", SearchOrder.Asc);
//
//         var output = await categoryRepository.Search(searchInput);
//
//         output.Should().NotBeNull();
//         output.Items.Should().NotBeNull();
//         output.CurrentPage.Should().Be(searchInput.Page);
//         output.PerPage.Should().Be(searchInput.PerPage);
//         output.Total.Should().Be(expectedQuantityTotalItems);
//         output.Items.Should().HaveCount(expectedQuantityItemsReturned);
//         foreach (var outputItem in output.Items)
//         {
//             var exampleItem = exampleCategoriesList.Find(
//                 category => category.Id == outputItem.Id
//             );
//             exampleItem.Should().NotBeNull();
//             outputItem.Name.Should().Be(exampleItem!.Name);
//             outputItem.Description.Should().Be(exampleItem.Description);
//             outputItem.IsActive.Should().Be(exampleItem.IsActive);
//             outputItem.CreatedAt.Should().Be(exampleItem.CreatedAt);
//         }
//     }
//     
//     [Theory]
//     [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
//     [InlineData("name", "asc")]
//     [InlineData("name", "desc")]
//     [InlineData("id", "asc")]
//     [InlineData("id", "desc")]
//     [InlineData("createdAt", "asc")]
//     [InlineData("createdAt", "desc")]
//     [InlineData("", "asc")]
//     public async Task CategoryRepository_SearchOrdered(
//         string orderBy,
//         string order
//     )
//     {
//         var dbContext = _fixture.CreateDbContext(NameDbContext);
//         var exampleCategoriesList = _fixture.DataGenerator.GetExampleCategoriesList();
//         
//         await dbContext.AddRangeAsync(exampleCategoriesList);
//         await dbContext.SaveChangesAsync(CancellationToken.None);
//         
//         var categoryRepository = new Catalog.Infra.Data.EF.Repositories.CategoryRepository(dbContext);
//         
//         var searchOrder = order.ToLower() == "asc" ? SearchOrder.Asc : SearchOrder.Desc;
//         var searchInput = new SearchInput(1, 20, "", orderBy, searchOrder);
//
//         var output = await categoryRepository.Search(searchInput);
//
//         var expectedOrderedList = _fixture.DataGenerator.CloneCategoriesListOrdered(
//             exampleCategoriesList,
//             orderBy,
//             searchOrder
//         );
//         
//         output.Should().NotBeNull();
//         output.Items.Should().NotBeNull();
//         output.CurrentPage.Should().Be(searchInput.Page);
//         output.PerPage.Should().Be(searchInput.PerPage);
//         output.Total.Should().Be(exampleCategoriesList.Count);
//         output.Items.Should().HaveCount(exampleCategoriesList.Count);
//         
//         for(var index = 0; index < expectedOrderedList.Count; index++)
//         {
//             var expectedItem = expectedOrderedList[index];
//             var outputItem = output.Items[index];
//             expectedItem.Should().NotBeNull();
//             outputItem.Should().NotBeNull();
//             outputItem.Name.Should().Be(expectedItem!.Name);
//             outputItem.Id.Should().Be(expectedItem.Id);
//             outputItem.Description.Should().Be(expectedItem.Description);
//             outputItem.IsActive.Should().Be(expectedItem.IsActive);
//             outputItem.CreatedAt.Should().Be(expectedItem.CreatedAt);
//         }
//     }
// }