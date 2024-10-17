// using Xunit;
// using System.Net;
// using Flixer.Catalog.Api.ApiModels.Response;
// using FluentAssertions;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Http;
// using Flixer.Catalog.EndToEndTests.Fixtures.Category;
// using Flixer.Catalog.Application.Common.Input.Category;
// using Flixer.Catalog.Application.Common.Output.Category;
// using Flixer.Catalog.UnitTest.Fixture.Application.Category.CreateCategory;
//
// namespace Flixer.Catalog.EndToEndTests.Api.Category.CreateCategory;
//
// [Collection(nameof(CategoryFixture))]
// public class CreateCategoryApiTest : IDisposable
// {
//     private readonly CategoryFixture _fixture;
//
//     public CreateCategoryApiTest(CategoryFixture fixture)
//     {
//         _fixture = fixture;
//     }
//
//     [Fact]
//     [Trait("EndToEnd/API", "Category/Create - Endpoints")]
//     public async Task EndToEnd_ShouldCreateCategory_WhenCalledHttpPostMethod()
//     {
//         var input = _fixture.DataGenerator.GetInputCreate();
//
//         var (response, output) = await _fixture.ApiClient
//             .Post<ApiResponse<CategoryOutput>>("/categories", input);
//         
//         var dbCategory = await _fixture.Persistence.GetById(output!.Data.Id);
//
//         response!.Should().NotBeNull();
//         response!.StatusCode.Should()
//             .Be((HttpStatusCode) StatusCodes.Status201Created);
//         
//         output.Should().NotBeNull();
//         output!.Data.Id.Should().NotBeEmpty();
//         output.Data.Name.Should().Be(input.Name);
//         output.Data.IsActive.Should().Be(input.IsActive);
//         output.Data.Description.Should().Be(input.Description);
//         output.Data.CreatedAt.Should().NotBeSameDateAs(default);
//         
//         dbCategory.Should().NotBeNull();
//         dbCategory!.Id.Should().NotBeEmpty();
//         dbCategory.Name.Should().Be(input.Name);
//         dbCategory.IsActive.Should().Be(input.IsActive);
//         dbCategory.Description.Should().Be(input.Description);
//         dbCategory.CreatedAt.Should().NotBeSameDateAs(default);
//     }
//
//     [Theory]
//     [Trait("EndToEnd/API", "Category/Create - Endpoints")]
//     [MemberData(
//         nameof(DataGenerator.GetInvalidCreateInputs),
//         parameters: 12,
//         MemberType = typeof(DataGenerator)
//     )]
//     public async Task EndToEnd_ThrowError_WhenCantInstantiateAggregate(
//         CreateCategoryInput input
//     )
//     {
//         var (response, output) = await _fixture.ApiClient
//             .Post<ProblemDetails>("/categories", input);
//     
//         response!.Should().NotBeNull();
//         response!.StatusCode.Should()
//             .Be((HttpStatusCode) StatusCodes.Status422UnprocessableEntity);
//     
//         output.Should().NotBeNull();
//         output!.Extensions.Should().ContainKey("Errors");
//         output!.Type.Should().Be("UnprocessableEntity");
//         output.Detail.Should().Be("Category is invalid");
//         output.Title.Should().Be("One or more validation errors occurred");
//         output.Status.Should().Be(StatusCodes.Status422UnprocessableEntity);
//     }
//     
//     public void Dispose()
//         => _fixture.CleanPersistence();
// }