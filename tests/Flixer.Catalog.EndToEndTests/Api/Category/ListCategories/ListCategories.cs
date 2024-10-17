// using Xunit;
// using System.Net;
// using FluentAssertions;
// using Microsoft.AspNetCore.Http;
// using Flixer.Catalog.EndToEndTests.ApiModels;
// using Flixer.Catalog.EndToEndTests.Extensions;
// using Flixer.Catalog.EndToEndTests.Fixtures.Category;
// using Flixer.Catalog.Application.Common.Input.Category;
// using Flixer.Catalog.Application.Common.Output.Category;
// using Flixer.Catalog.Domain.Enums;
// using Newtonsoft.Json;
//
// namespace Flixer.Catalog.EndToEndTests.Api.Category.ListCategories;
//
// [Collection(nameof(CategoryFixture))]
// public class ListCategories : IDisposable
// {
//     private readonly CategoryFixture _fixture;
//
//     public ListCategories(CategoryFixture fixture)
//     {
//         _fixture = fixture;
//     }
//
//     [Fact]
//     [Trait("EndToEnd/API", "Category/List - Endpoints")]
//     public async Task EndToEnd_ShouldListCategoriesAndTotalByDefault_WhenCalledHttpMethod()
//     {
//         var defaultPerPage = 15;
//         var exampleCategoryList = _fixture.DataGenerator.GetExampleCategoriesList(20);
//         await _fixture.Persistence.InsertList(exampleCategoryList);
//
//         var (response, output) = await
//             _fixture.ApiClient.Get<TestApiResponseList<CategoryOutput>>(
//                 $"/categories"
//             );
//
//         response.Should().NotBeNull();
//         response!.StatusCode.Should().Be(HttpStatusCode.OK);
//         
//         output.Should().NotBeNull();
//         output!.Meta.Should().NotBeNull();
//         output!.Data.Should().NotBeNull();
//         output.Meta!.Page.Should().Be(defaultPerPage);
//         output!.Data.Should().HaveCount(defaultPerPage);
//         output.Meta.CurrentPage.Should().Be(1);
//         output.Meta!.Total.Should().Be(exampleCategoryList.Count);
//
//         foreach (var outputItem in output.Data!)
//         {
//             var expectedItem = exampleCategoryList
//                 .FirstOrDefault(x => x.Id == outputItem.Id);
//
//             expectedItem.Should().NotBeNull();
//             outputItem.Id.Should().Be(expectedItem!.Id);
//             outputItem.Name.Should().Be(expectedItem.Name);
//             outputItem.Description.Should().Be(expectedItem.Description);
//             outputItem.IsActive.Should().Be(expectedItem.IsActive);
//             outputItem.CreatedAt.TrimMillisseconds().Should().Be(expectedItem.CreatedAt.TrimMillisseconds());
//         }
//     }
//     
//     [Fact]
//     [Trait("EndToEnd/API", "Category/List - Endpoints")]
//     public async Task EndToEnd_ShouldListItemsEmptyWhenPersistenceEmpty_WhenCalledHttpMethod()
//     {
//         var (response, output) = await _fixture.ApiClient
//             .Get<TestApiResponseList<CategoryOutput>>("/categories");
//     
//         response.Should().NotBeNull();
//         response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
//     
//         output.Should().NotBeNull();
//         output!.Meta!.Total.Should().Be(0);
//         output.Data.Should().HaveCount(0);
//     }
//     
//     [Fact]
//     [Trait("EndToEnd/API", "Category/List - Endpoints")]
//     public async void EndToEnd_ShouldListCategoriesAndTotal_WhenCalledHttpMethod()
//     {
//         var exampleCategoriesList = _fixture.DataGenerator.GetExampleCategoriesList(20);
//     
//         await _fixture.Persistence.InsertList(exampleCategoriesList);
//     
//         var input = new ListCategoriesInput(page: 1, perPage: 5);
//     
//         var (response, output) = await _fixture.ApiClient
//             .Get<TestApiResponseList<CategoryOutput>>("/categories", input);
//     
//         response.Should().NotBeNull();
//         response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
//     
//         output.Should().NotBeNull();
//         output!.Meta!.Page.Should().Be(input.PerPage);
//         output.Meta.CurrentPage.Should().Be(input.Page);
//         output.Data.Should().HaveCount(input.PerPage);
//         output.Meta.Total.Should().Be(exampleCategoriesList.Count);
//     
//         foreach (var outputItem in output.Data!)
//         {
//             var exampleItem = exampleCategoriesList
//                 .FirstOrDefault(x => x.Id == outputItem.Id);
//     
//             exampleItem.Should().NotBeNull();
//             outputItem.Name.Should().Be(exampleItem!.Name);
//             outputItem.IsActive.Should().Be(exampleItem.IsActive);
//             outputItem.CreatedAt.TrimMillisseconds().Should().Be(exampleItem.CreatedAt.TrimMillisseconds());
//             outputItem.Description.Should().Be(exampleItem.Description);
//         }
//     }
//     
//     [Theory]
//     [Trait("EndToEnd/API", "Category/List - Endpoints")]
//     [InlineData(10, 1, 5, 5)]
//     [InlineData(10, 2, 5, 5)]
//     [InlineData(7, 2, 5, 2)]
//     [InlineData(7, 3, 5, 0)]
//     public async Task EndToEnd_ShouldListPaginated_WhenCalledHttpMethod(
//         int quantityCategoriesToGenerate,
//         int page,
//         int perPage,
//         int expectedQuantityItems
//     )
//     {
//         var exampleCategoriesList = _fixture.DataGenerator
//             .GetExampleCategoriesList(quantityCategoriesToGenerate);
//     
//         await _fixture.Persistence.InsertList(exampleCategoriesList);
//     
//         var input = new ListCategoriesInput(page, perPage);
//     
//         var (response, output) = await _fixture.ApiClient
//             .Get<TestApiResponseList<CategoryOutput>>("/categories", input);
//     
//         response.Should().NotBeNull();
//         response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
//     
//         output.Should().NotBeNull();
//         output!.Meta!.CurrentPage.Should().Be(input.Page);
//         output.Meta.Page.Should().Be(input.PerPage);
//         output.Meta.Total.Should().Be(exampleCategoriesList.Count);
//         output.Data.Should().HaveCount(expectedQuantityItems);
//     
//         foreach (var outputItem in output.Data!)
//         {
//             var exampleItem = exampleCategoriesList
//                 .FirstOrDefault(x => x.Id == outputItem.Id);
//     
//             exampleItem.Should().NotBeNull();
//             outputItem.Name.Should().Be(exampleItem!.Name);
//             outputItem.IsActive.Should().Be(exampleItem.IsActive);
//             outputItem.CreatedAt.TrimMillisseconds().Should().Be(exampleItem.CreatedAt.TrimMillisseconds());
//             outputItem.Description.Should().Be(exampleItem.Description);
//         }
//     }
//     
//     
//     [Theory]
//     [Trait("EndToEnd/API", "Category/List - Endpoints")]
//     [InlineData("Action", 1, 5, 1, 1)]
//     [InlineData("Horror", 1, 5, 3, 3)]
//     [InlineData("Horror", 2, 5, 0, 3)]
//     [InlineData("Sci-fi", 1, 5, 4, 4)]
//     [InlineData("Sci-fi", 1, 2, 2, 4)]
//     [InlineData("Sci-fi", 2, 3, 1, 4)]
//     [InlineData("Sci-fi Other", 1, 3, 0, 0)]
//     [InlineData("Robots", 1, 5, 2, 2)]
//     public async Task EndToEnd_ShouldSearchByText_WhenCalledHttpMethod(
//         string search,
//         int page,
//         int perPage,
//         int expectedQuantityItemsReturned,
//         int expectedQuantityTotalItems
//     )
//     {
//         var categoryNamesList = new List<string>() {
//             "Action",
//             "Horror",
//             "Horror - Robots",
//             "Horror - Based on Real Facts",
//             "Drama",
//             "Sci-fi IA",
//             "Sci-fi Space",
//             "Sci-fi Robots",
//             "Sci-fi Future"
//         };
//     
//         var exampleCategoriesList = _fixture.DataGenerator
//             .GetExampleCategoriesListWithNames(categoryNamesList);
//     
//         await _fixture.Persistence.InsertList(exampleCategoriesList);
//     
//         var input = new ListCategoriesInput(page, perPage, search);
//     
//         var (response, output) = await _fixture.ApiClient
//             .Get<TestApiResponseList<CategoryOutput>>("/categories", input);
//     
//         response.Should().NotBeNull();
//         response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
//     
//         output.Should().NotBeNull();
//         output!.Meta!.CurrentPage.Should().Be(input.Page);
//         output.Meta.Page.Should().Be(input.PerPage);
//         output.Meta.Total.Should().Be(expectedQuantityTotalItems);
//         output.Data.Should().HaveCount(expectedQuantityItemsReturned);
//     
//         foreach (var outputItem in output.Data!)
//         {
//             var exampleItem = exampleCategoriesList
//                 .FirstOrDefault(x => x.Id == outputItem.Id);
//     
//             exampleItem.Should().NotBeNull();
//             outputItem.Name.Should().Be(exampleItem!.Name);
//             outputItem.IsActive.Should().Be(exampleItem.IsActive);
//             outputItem.CreatedAt.TrimMillisseconds().Should().Be(exampleItem.CreatedAt.TrimMillisseconds());
//             outputItem.Description.Should().Be(exampleItem.Description);
//         }
//     }
//     
//     [Theory]
//     [Trait("EndToEnd/API", "Category/List - Endpoints")]
//     [InlineData("name", "asc")]
//     [InlineData("name", "desc")]
//     [InlineData("id", "asc")]
//     [InlineData("id", "desc")]
//     [InlineData("", "asc")]
//     public async Task EndToEnd_ShouldListOrdered_WhenCalledHttpMethod(string orderBy, string order)
//     {
//         var exampleCategoriesList = _fixture.DataGenerator.GetExampleCategoriesList();
//     
//         await _fixture.Persistence.InsertList(exampleCategoriesList);
//     
//         var inputOrder = order == "asc" ? SearchOrder.Asc : SearchOrder.Desc;
//     
//         var input = new ListCategoriesInput(
//             page: 1,
//             perPage: 20,
//             sort: orderBy,
//             dir: inputOrder
//         );
//     
//         var (response, output) = await _fixture.ApiClient
//             .Get<TestApiResponseList<CategoryOutput>>("/categories", input);
//     
//         response.Should().NotBeNull();
//         response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
//     
//         output.Should().NotBeNull();
//         output!.Meta!.CurrentPage.Should().Be(input.Page);
//         output.Meta.Page.Should().Be(input.PerPage);
//         output.Meta.Total.Should().Be(exampleCategoriesList.Count);
//         output.Data.Should().HaveCount(exampleCategoriesList.Count);
//     
//         var expectedOrderedList = _fixture.DataGenerator.CloneCategoriesListOrdered(
//             exampleCategoriesList,
//             input.Sort,
//             input.Dir
//         );
//     
//         for (var indice = 0; indice < expectedOrderedList.Count; indice++)
//         {
//             var outputItem = output.Data![indice];
//             var exampleItem = expectedOrderedList[indice];
//     
//             outputItem.Should().NotBeNull();
//             exampleItem.Should().NotBeNull();
//             outputItem.Id.Should().Be(exampleItem.Id);
//             outputItem.Name.Should().Be(exampleItem!.Name);
//             outputItem.IsActive.Should().Be(exampleItem.IsActive);
//             outputItem.Description.Should().Be(exampleItem.Description);
//             outputItem.CreatedAt.TrimMillisseconds().Should().Be(exampleItem.CreatedAt.TrimMillisseconds());
//         }
//     }
//
//     public void Dispose()
//         => _fixture.CleanPersistence();
// }