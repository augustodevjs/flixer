using Xunit;
using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Flixer.Catalog.Api.Response;
using Flixer.Catalog.EndToEndTests.Fixtures.Category;
using Flixer.Catalog.Application.Common.Output.Category;

namespace Flixer.Catalog.EndToEndTests.Api.Category.GetCategory;

[Collection(nameof(CategoryFixture))]
public class GetCategoryApiTest : IDisposable
{
    private readonly CategoryFixture _fixture;
    
    public GetCategoryApiTest(CategoryFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    [Trait("EndToEnd/API", "Category/Get - Endpoints")]
    public async Task EndToEnd_ShouldGetCategory_WhenCalledHttpGetByIdMethod()
    {
        var exampleCategory = _fixture.DataGenerator.GetValidCategory();

        await _fixture.Persistence.Insert(exampleCategory);

        var (response, output) = await _fixture.ApiClient
            .Get<ApiResponse<CategoryOutput>>($"/categories/{exampleCategory.Id}");

        response!.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode) StatusCodes.Status200OK);

        output.Should().NotBeNull();
        output!.Data.Id.Should().NotBeEmpty();
        output.Data.Name.Should().Be(exampleCategory.Name);
        output.Data.CreatedAt.Should().NotBeSameDateAs(default);
        output.Data.IsActive.Should().Be(exampleCategory.IsActive);
        output.Data.Description.Should().Be(exampleCategory.Description);
    }

    [Fact]
    [Trait("EndToEnd/API", "Category/Get - Endpoints")]
    public async Task EndToEnd_ThrowError_WhenDoesntExistCategory()
    {
        var exampleCategory = _fixture.DataGenerator.GetValidCategory();
        await _fixture.Persistence.Insert(exampleCategory);

        var randomGuid = Guid.NewGuid();

        var (response, output) = await _fixture.ApiClient
            .Get<ProblemDetails>($"/categories/{randomGuid}");

        response!.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode) StatusCodes.Status404NotFound);

        output.Should().NotBeNull();
        output!.Type.Should().Be("NotFound");
        output!.Title.Should().Be("Not Found");
        output.Status.Should().Be(StatusCodes.Status404NotFound);
        output!.Detail.Should().Be($"Category '{randomGuid}' not found.");
    }

    public void Dispose()
        => _fixture.CleanPersistence();
}
