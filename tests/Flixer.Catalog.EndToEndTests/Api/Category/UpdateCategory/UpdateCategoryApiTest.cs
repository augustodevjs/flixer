// using System.Net;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Http;
// using Flixer.Catalog.EndToEndTests.Api.Category.Common;
// using Flixer.Catalog.Application.Dtos.ViewModel.Category;
// using Flixer.Catalog.Application.Dtos.InputModel.Category;
// using CommonTests = Flixer.Catalog.Common.Tests.Fixture.Category;
//
// namespace Flixer.Catalog.EndToEndTests.Api.Category.UpdateCategory;
//
// [Collection(nameof(CategoryFixture))]
// public class UpdateCategoryApiTest : IDisposable
// {
//     private readonly CategoryFixture _fixture;
//
//     public UpdateCategoryApiTest(CategoryFixture fixture)
//         => _fixture = fixture;
//
//     [Fact]
//     [Trait("EndToEnd/API", "Category/Update - Endpoints")]
//     public async void EndToEnd_ShouldUpdateCategory_WhenCalledHttpPutMethod()
//     {
//         var exampleCategoriesList = _fixture.CategoryTest.GetExampleCategoriesList(20);
//         await _fixture.Persistence.InsertList(exampleCategoriesList);
//
//         var exampleCategory = exampleCategoriesList[10];
//
//         var input = _fixture.CategoryTest.GetInputUpdate(exampleCategory.Id);
//
//         var (response, output) = await _fixture.ApiClient.Put<CategoryViewModel>(
//             $"/categories/{exampleCategory.Id}",
//             input
//         );
//
//         var dbCategory = await _fixture.Persistence.GetById(exampleCategory.Id);
//
//         response.Should().NotBeNull();
//         response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
//
//         output.Should().NotBeNull();
//         output!.Name.Should().Be(input.Name);
//         output!.Id.Should().Be(exampleCategory.Id);
//         output.Description.Should().Be(input.Description);
//         output.IsActive.Should().Be((bool)input.IsActive!);
//
//         dbCategory.Should().NotBeNull();
//         dbCategory!.Name.Should().Be(input.Name);
//         dbCategory.Description.Should().Be(input.Description);
//         dbCategory.IsActive.Should().Be((bool)input.IsActive!);
//     }
//
//     [Fact]
//     [Trait("EndToEnd/API", "Category/Update - Endpoints")]
//     public async void EndToEnd_ShouldUpdateCategoryOnlyName_WhenCalledHttpPutMethod()
//     {
//         var exampleCategoriesList = _fixture.CategoryTest.GetExampleCategoriesList(20);
//         await _fixture.Persistence.InsertList(exampleCategoriesList);
//
//         var exampleCategory = exampleCategoriesList[10];
//
//         var input = new UpdateCategoryInputModel(exampleCategory.Id, _fixture.CategoryTest.GetValidCategoryName());
//
//         var (response, output) = await _fixture.ApiClient.Put<CategoryViewModel>(
//             $"/categories/{exampleCategory.Id}",
//             input
//         );
//
//         var dbCategory = await _fixture.Persistence.GetById(exampleCategory.Id);
//
//         response.Should().NotBeNull();
//         response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
//
//         output.Should().NotBeNull();
//         output!.Name.Should().Be(input.Name);
//         output!.Id.Should().Be(exampleCategory.Id);
//         output.IsActive.Should().Be(exampleCategory.IsActive);
//         output.Description.Should().Be(exampleCategory.Description);
//
//         dbCategory.Should().NotBeNull();
//         dbCategory!.Name.Should().Be(input.Name);
//         dbCategory.IsActive.Should().Be(exampleCategory.IsActive);
//         dbCategory.Description.Should().Be(exampleCategory.Description);
//     }
//
//     [Fact]
//     [Trait("EndToEnd/API", "Category/Update - Endpoints")]
//     public async void EndToEnd_ShouldUpdateCategoryNameAndDescription_WhenCalledHttpPutMethod()
//     {
//         var exampleCategoriesList = _fixture.CategoryTest.GetExampleCategoriesList(20);
//         await _fixture.Persistence.InsertList(exampleCategoriesList);
//
//         var exampleCategory = exampleCategoriesList[10];
//
//         var input = new UpdateCategoryInputModel(
//             exampleCategory.Id,
//             _fixture.CategoryTest.GetValidCategoryName(),
//             null,
//             _fixture.CategoryTest.GetValidCategoryDescription()
//         );
//
//         var (response, output) = await _fixture.ApiClient.Put<CategoryViewModel>(
//             $"/categories/{exampleCategory.Id}",
//             input
//         );
//
//         var dbCategory = await _fixture.Persistence.GetById(exampleCategory.Id);
//
//         response.Should().NotBeNull();
//         response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
//
//         output.Should().NotBeNull();
//         output!.Name.Should().Be(input.Name);
//         output!.Id.Should().Be(exampleCategory.Id);
//         output.Description.Should().Be(input.Description);
//         output.IsActive.Should().Be(exampleCategory.IsActive);
//
//         dbCategory.Should().NotBeNull();
//         dbCategory!.Name.Should().Be(input.Name);
//         dbCategory.Description.Should().Be(input.Description);
//         dbCategory.IsActive.Should().Be(exampleCategory.IsActive);
//     }
//
//
//     [Fact]
//     [Trait("EndToEnd/API", "Category/Update - Endpoints")]
//     public async void EndToEnd_ShouldThrowError_WhenCategoryNotFound()
//     {
//         var exampleCategoriesList = _fixture.CategoryTest.GetExampleCategoriesList(20);
//         await _fixture.Persistence.InsertList(exampleCategoriesList);
//
//         var randomGuid = Guid.NewGuid();
//
//         var input = _fixture.CategoryTest.GetInputUpdate(randomGuid);
//
//         var (response, output) = await _fixture.ApiClient.Put<ProblemDetails>(
//             $"/categories/{randomGuid}",
//             input
//         );
//
//         response.Should().NotBeNull();
//         response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status404NotFound);
//
//         output.Should().NotBeNull();
//         output!.Type.Should().Be("NotFound");
//         output!.Title.Should().Be("Not Found");
//         output.Status.Should().Be((int)StatusCodes.Status404NotFound);
//         output.Detail.Should().Be($"Category '{randomGuid}' not found.");
//     }
//
//     [Theory(DisplayName = nameof(ErrorWhenCantInstantiateAggregate))]
//     [Trait("EndToEnd/API", "Category/Update - Endpoints")]
//     [MemberData(
//         nameof(CommonTests.DataGenerator.GetInvalidUpdateInputs),
//         parameters: 12,
//         MemberType = typeof(CommonTests.DataGenerator)
//     )]
//     public async void ErrorWhenCantInstantiateAggregate(UpdateCategoryInputModel input, string expectedDetail)
//     {
//         var exampleCategoriesList = _fixture.CategoryTest.GetExampleCategoriesList(20);
//
//         await _fixture.Persistence.InsertList(exampleCategoriesList);
//
//         var exampleCategory = exampleCategoriesList[10];
//         input.Id = exampleCategory.Id;
//
//         var (response, output) = await _fixture.ApiClient.Put<ProblemDetails>(
//             $"/categories/{exampleCategory.Id}",
//             input
//         );
//
//         response.Should().NotBeNull();
//         response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status422UnprocessableEntity);
//
//         output.Should().NotBeNull();
//         output!.Detail.Should().Be(expectedDetail);
//         output.Type.Should().Be("UnprocessableEntity");
//         output!.Title.Should().Be("One or more validation errors ocurred");
//         output.Status.Should().Be((int)StatusCodes.Status422UnprocessableEntity);
//     }
//
//     public void Dispose()
//     => _fixture.CleanPersistence();
// }