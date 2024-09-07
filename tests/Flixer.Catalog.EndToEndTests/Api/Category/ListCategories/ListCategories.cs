// using System.Net;
// using Flixer.Catalog.Api.Response;
// using Flixer.Catalog.Application.Common.Output.Category;
// using Flixer.Catalog.EndToEndTests.Fixtures.Category;
// using FluentAssertions;
// using Newtonsoft.Json;
// using Xunit.Abstractions;
// using Microsoft.AspNetCore.Http;
// using Xunit;
//
// namespace Flixer.Catalog.EndToEndTests.Api.Category.ListCategories;
//
// [Collection(nameof(CategoryFixture))]
// public class ListCategories : IDisposable
// {
//     private readonly ITestOutputHelper _output;
//     private readonly CategoryFixture _fixture;
//
//     public ListCategories(ITestOutputHelper output, CategoryFixture fixture)
//     {
//         _output = output;
//         _fixture = fixture;
//     }
//
//     [Fact]
//     [Trait("EndToEnd/API", "Category/List - Endpoints")]
//     public async Task EndToEnd_ShouldListCategoriesAndTotalByDefault_WhenCalledHttpMethod()
//     {
//         var defaultPerPage = 15;
//
//         var exampleCategoriesList = _fixture.DataGenerator.GetExampleCategoriesList(20);
//
//         await _fixture.Persistence.InsertList(exampleCategoriesList);
//
//         var (response, output) = await _fixture.ApiClient
//             .Get<ApiResponseList<ListCategoriesOutput>>("/categories");
//
//         response.Should().NotBeNull();
//         response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
//
//         output.Should().NotBeNull();
//         output!.Meta.Page.Should().Be(1);
//         output.Meta.CurrentPage.Should().Be(defaultPerPage);
//         output.Data.Should().HaveCount(defaultPerPage);
//         output!.Meta.Total.Should().Be(exampleCategoriesList.Count);
//
//         foreach (var outputItem in output.Data.Select(x => x.Items))
//         {
//             var exampleItem = exampleCategoriesList.FirstOrDefault(x => x.Id == outputItem.);
//
//             exampleItem.Should().NotBeNull();
//             outputItem.Name.Should().Be(exampleItem!.Name);
//             outputItem.IsActive.Should().Be(exampleItem.IsActive);
//             outputItem.CreatedAt.TrimMilisseconds().Should().Be(exampleItem.CreatedAt.TrimMilisseconds());
//             outputItem.Description.Should().Be(exampleItem.Description);
//         }
//     }
//
//     [Fact]
//     [Trait("EndToEnd/API", "Category/List - Endpoints")]
//     public async Task EndToEnd_ShouldListItemsEmptyWhenPersistenceEmpty_WhenCalledHttpMethod()
//     {
//         var (response, output) = await _fixture.ApiClient.Get<ListCategoriesViewModel>("/categories");
//
//         response.Should().NotBeNull();
//         response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
//
//         output.Should().NotBeNull();
//         output!.Total.Should().Be(0);
//         output.Items.Should().HaveCount(0);
//     }
//
//     [Fact]
//     [Trait("EndToEnd/API", "Category/List - Endpoints")]
//     public async void EndToEnd_ShouldListCategoriesAndTotal_WhenCalledHttpMethod()
//     {
//         var exampleCategoriesList = _fixture.CategoryTest.GetExampleCategoriesList(20);
//
//         await _fixture.Persistence.InsertList(exampleCategoriesList);
//
//         var input = new ListCategoriesInputModel(page: 1, perPage: 5);
//
//         var (response, output) = await _fixture.ApiClient.Get<ListCategoriesViewModel>("/categories", input);
//
//         response.Should().NotBeNull();
//         response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
//
//         output.Should().NotBeNull();
//         output!.Page.Should().Be(input.Page);
//         output.PerPage.Should().Be(input.PerPage);
//         output.Items.Should().HaveCount(input.PerPage);
//         output.Total.Should().Be(exampleCategoriesList.Count);
//
//         foreach (CategoryViewModel outputItem in output.Items)
//         {
//             var exampleItem = exampleCategoriesList.FirstOrDefault(x => x.Id == outputItem.Id);
//
//             exampleItem.Should().NotBeNull();
//             outputItem.Name.Should().Be(exampleItem!.Name);
//             outputItem.IsActive.Should().Be(exampleItem.IsActive);
//             outputItem.CreatedAt.TrimMilisseconds().Should().Be(exampleItem.CreatedAt.TrimMilisseconds());
//             outputItem.Description.Should().Be(exampleItem.Description);
//         }
//     }
//
//     [Theory(DisplayName = nameof(ListPaginated))]
//     [Trait("EndToEnd/API", "Category/List - Endpoints")]
//     [InlineData(10, 1, 5, 5)]
//     [InlineData(10, 2, 5, 5)]
//     [InlineData(7, 2, 5, 2)]
//     [InlineData(7, 3, 5, 0)]
//     public async Task ListPaginated(
//         int quantityCategoriesToGenerate,
//         int page,
//         int perPage,
//         int expectedQuantityItems
//     )
//     {
//         var exampleCategoriesList = _fixture.CategoryTest.GetExampleCategoriesList(quantityCategoriesToGenerate);
//
//         await _fixture.Persistence.InsertList(exampleCategoriesList);
//
//         var input = new ListCategoriesInputModel(page, perPage);
//
//         var (response, output) = await _fixture.ApiClient.Get<ListCategoriesViewModel>("/categories", input);
//
//         response.Should().NotBeNull();
//         response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
//
//         output.Should().NotBeNull();
//         output!.Page.Should().Be(input.Page);
//         output.PerPage.Should().Be(input.PerPage);
//         output.Total.Should().Be(exampleCategoriesList.Count);
//         output.Items.Should().HaveCount(expectedQuantityItems);
//
//         foreach (CategoryViewModel outputItem in output.Items)
//         {
//             var exampleItem = exampleCategoriesList.FirstOrDefault(x => x.Id == outputItem.Id);
//
//             exampleItem.Should().NotBeNull();
//             outputItem.Name.Should().Be(exampleItem!.Name);
//             outputItem.IsActive.Should().Be(exampleItem.IsActive);
//             outputItem.CreatedAt.TrimMilisseconds().Should().Be(exampleItem.CreatedAt.TrimMilisseconds());
//             outputItem.Description.Should().Be(exampleItem.Description);
//         }
//     }
//
//     [Theory(DisplayName = nameof(SearchByText))]
//     [Trait("EndToEnd/API", "Category/List - Endpoints")]
//     [InlineData("Action", 1, 5, 1, 1)]
//     [InlineData("Horror", 1, 5, 3, 3)]
//     [InlineData("Horror", 2, 5, 0, 3)]
//     [InlineData("Sci-fi", 1, 5, 4, 4)]
//     [InlineData("Sci-fi", 1, 2, 2, 4)]
//     [InlineData("Sci-fi", 2, 3, 1, 4)]
//     [InlineData("Sci-fi Other", 1, 3, 0, 0)]
//     [InlineData("Robots", 1, 5, 2, 2)]
//     public async Task SearchByText(
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
//         var exampleCategoriesList = _fixture.CategoryTest.GetExampleCategoriesListWithNames(categoryNamesList);
//
//         await _fixture.Persistence.InsertList(exampleCategoriesList);
//
//         var input = new ListCategoriesInputModel(page, perPage, search);
//
//         var (response, output) = await _fixture.ApiClient.Get<ListCategoriesViewModel>("/categories", input);
//
//         response.Should().NotBeNull();
//         response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
//
//         output.Should().NotBeNull();
//         output!.Page.Should().Be(input.Page);
//         output.PerPage.Should().Be(input.PerPage);
//         output.Total.Should().Be(expectedQuantityTotalItems);
//         output.Items.Should().HaveCount(expectedQuantityItemsReturned);
//
//         foreach (CategoryViewModel outputItem in output.Items)
//         {
//             var exampleItem = exampleCategoriesList.FirstOrDefault(x => x.Id == outputItem.Id);
//
//             exampleItem.Should().NotBeNull();
//             outputItem.Name.Should().Be(exampleItem!.Name);
//             outputItem.IsActive.Should().Be(exampleItem.IsActive);
//             outputItem.CreatedAt.TrimMilisseconds().Should().Be(exampleItem.CreatedAt.TrimMilisseconds());
//             outputItem.Description.Should().Be(exampleItem.Description);
//         }
//     }
//
//     [Theory(DisplayName = nameof(ListOrdered))]
//     [Trait("EndToEnd/API", "Category/List - Endpoints")]
//     [InlineData("name", "asc")]
//     [InlineData("name", "desc")]
//     [InlineData("id", "asc")]
//     [InlineData("id", "desc")]
//     [InlineData("", "asc")]
//     public async Task ListOrdered(string orderBy, string order)
//     {
//         var exampleCategoriesList = _fixture.CategoryTest.GetExampleCategoriesList(10);
//
//         await _fixture.Persistence.InsertList(exampleCategoriesList);
//
//         var inputOrder = order == "asc" ? SearchOrder.Asc : SearchOrder.Desc;
//
//         var input = new ListCategoriesInputModel(
//             page: 1,
//             perPage: 20,
//             sort: orderBy,
//             dir: inputOrder
//         );
//
//         var (response, output) = await _fixture.ApiClient.Get<ListCategoriesViewModel>("/categories", input);
//
//         response.Should().NotBeNull();
//         response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
//
//         output.Should().NotBeNull();
//         output!.Page.Should().Be(input.Page);
//         output.PerPage.Should().Be(input.PerPage);
//         output.Total.Should().Be(exampleCategoriesList.Count);
//         output.Items.Should().HaveCount(exampleCategoriesList.Count);
//
//         var expectedOrderedList = _fixture.CategoryTest.CloneCategoriesListOrdered(
//             exampleCategoriesList,
//             input.Sort,
//             input.Dir
//         );
//
//         var count = 0;
//         var expectedArr = expectedOrderedList.Select(x => $"{++count} {x.Name} {x.CreatedAt} {JsonConvert.SerializeObject(x)}");
//         count = 0;
//         var outputArr = output.Items.Select(x => $"{++count} {x.Name} {x.CreatedAt} {JsonConvert.SerializeObject(x)}");
//
//         _output.WriteLine("Expecteds...");
//         _output.WriteLine(String.Join('\n', expectedArr));
//         _output.WriteLine("Outputs...");
//         _output.WriteLine(String.Join('\n', outputArr));
//
//         for (int indice = 0; indice < expectedOrderedList.Count; indice++)
//         {
//             var outputItem = output.Items[indice];
//             var exampleItem = expectedOrderedList[indice];
//
//             outputItem.Should().NotBeNull();
//             exampleItem.Should().NotBeNull();
//             outputItem.Id.Should().Be(exampleItem.Id);
//             outputItem.Name.Should().Be(exampleItem!.Name);
//             outputItem.IsActive.Should().Be(exampleItem.IsActive);
//             outputItem.Description.Should().Be(exampleItem.Description);
//             outputItem.CreatedAt.TrimMilisseconds().Should().Be(exampleItem.CreatedAt.TrimMilisseconds());
//         }
//     }
//
//     public void Dispose()
//         => _fixture.CleanPersistence();
// }