// using Xunit;
// using System.Net;
// using Flixer.Catalog.Api.ApiModels.Response;
// using FluentAssertions;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Http;
// using Flixer.Catalog.EndToEndTests.Fixtures.Category;
// using Flixer.Catalog.Application.Common.Input.Category;
// using Flixer.Catalog.Application.Common.Output.Category;
// using Flixer.Catalog.UnitTest.Fixture.Application.Category.UpdateCategory;
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
//         var exampleCategoriesList = _fixture.DataGenerator.GetExampleCategoriesList(20);
//         await _fixture.Persistence.InsertList(exampleCategoriesList);
//
//         var exampleCategory = exampleCategoriesList[10];
//
//         var input = _fixture.DataGenerator.GetInputUpdate(exampleCategory.Id);
//
//         var (response, output) = await _fixture.ApiClient
//             .Put<ApiResponse<CategoryOutput>>(
//                 $"/categories/{exampleCategory.Id}",
//                 input
//             );
//
//         var dbCategory = await _fixture.Persistence.GetById(exampleCategory.Id);
//
//         response.Should().NotBeNull();
//         response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
//
//         output.Should().NotBeNull();
//         output!.Data.Name.Should().Be(input.Name);
//         output!.Data.Id.Should().Be(exampleCategory.Id);
//         output.Data.Description.Should().Be(input.Description);
//         output.Data.IsActive.Should().Be((bool)input.IsActive!);
//
//         dbCategory.Should().NotBeNull();
//         dbCategory!.Name.Should().Be(input.Name);
//         dbCategory.Description.Should().Be(input.Description);
//         dbCategory.IsActive.Should().Be((bool)input.IsActive!);
//     }
//
//     [Fact]
//     [Trait("EndToEnd/API", "Category/Update - Endpoints")]
//     public async void EndToEnd_ShouldUpdateCategoryNameAndDescription_WhenCalledHttpPutMethod()
//     {
//         var exampleCategoriesList = _fixture.DataGenerator.GetExampleCategoriesList(20);
//         
//         await _fixture.Persistence.InsertList(exampleCategoriesList);
//
//         var exampleCategory = exampleCategoriesList[10];
//
//         var input = new UpdateCategoryInput(
//             exampleCategory.Id,
//             _fixture.DataGenerator.GetValidCategoryName(),
//             _fixture.DataGenerator.GetValidCategoryDescription(),
//             null
//         );
//
//         var (response, output) = await _fixture.ApiClient
//             .Put<ApiResponse<CategoryOutput>>(
//                 $"/categories/{exampleCategory.Id}",
//                 input
//             );
//
//         var dbCategory = await _fixture.Persistence.GetById(exampleCategory.Id);
//
//         response.Should().NotBeNull();
//         response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
//
//         output.Should().NotBeNull();
//         output!.Data.Name.Should().Be(input.Name);
//         output!.Data.Id.Should().Be(exampleCategory.Id);
//         output.Data.Description.Should().Be(input.Description);
//         output.Data.IsActive.Should().Be(exampleCategory.IsActive);
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
//         var exampleCategoriesList = _fixture.DataGenerator.GetExampleCategoriesList(20);
//         await _fixture.Persistence.InsertList(exampleCategoriesList);
//
//         var randomGuid = Guid.NewGuid();
//
//         var input = _fixture.DataGenerator.GetInputUpdate(randomGuid);
//
//         var (response, output) = await _fixture.ApiClient
//             .Put<ProblemDetails>(
//                 $"/categories/{randomGuid}",
//                 input
//             );
//
//         response.Should().NotBeNull();
//         response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status404NotFound);
//
//         output.Should().NotBeNull();
//         output!.Type.Should().Be("NotFound");
//         output!.Title.Should().Be("Not Found");
//         output.Status.Should().Be(StatusCodes.Status404NotFound);
//         output.Detail.Should().Be($"Category '{randomGuid}' not found.");
//     }
//
//     [Theory(DisplayName = nameof(ErrorWhenCantInstantiateAggregate))]
//     [Trait("EndToEnd/API", "Category/Update - Endpoints")]
//     [MemberData(
//         nameof(DataGenerator.GetInvalidUpdateInputs),
//         parameters: 12,
//         MemberType = typeof(DataGenerator)
//     )]
//     public async void ErrorWhenCantInstantiateAggregate(UpdateCategoryInput input)
//     {
//         var exampleCategoriesList = _fixture.DataGenerator.GetExampleCategoriesList(20);
//
//         await _fixture.Persistence.InsertList(exampleCategoriesList);
//
//         var exampleCategory = exampleCategoriesList[10];
//         input.Id = exampleCategory.Id;
//
//         var (response, output) = await _fixture.ApiClient
//             .Put<ProblemDetails>(
//                 $"/categories/{exampleCategory.Id}",
//                 input
//             );
//
//         response.Should().NotBeNull();
//         response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status422UnprocessableEntity);
//
//         output.Should().NotBeNull();
//         output!.Extensions.Should().ContainKey("Errors");
//         output!.Type.Should().Be("UnprocessableEntity");
//         output.Detail.Should().Be("Category is invalid");
//         output.Title.Should().Be("One or more validation errors occurred");
//         output.Status.Should().Be((int)StatusCodes.Status422UnprocessableEntity);
//     }
//
//     public void Dispose()
//     => _fixture.CleanPersistence();
// }