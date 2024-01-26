using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Flixer.Catalog.Application.Dtos.ViewModel.Category;

namespace Flixer.Catalog.EndToEndTests.Api.Category.GetCategory;

[Collection(nameof(GetCategoryApiTestFixture))]
public class GetCategoryApiTest : IDisposable
{
    private readonly GetCategoryApiTestFixture _fixture;

    public GetCategoryApiTest(GetCategoryApiTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    [Trait("EndToEnd/API", "Category/Get - Endpoints")]
    public async Task EndToEnd_ShouldGetCategory_WhenCalledHttpGetByIdMethod()
    {
        var exampleCategory = _fixture.GetExampleCategory();

        await _fixture.Persistence.Insert(exampleCategory);

        var (response, output) = await _fixture.ApiClient.Get<CategoryViewModel>($"/categories/{exampleCategory.Id}");

        response!.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode) StatusCodes.Status200OK);

        output.Should().NotBeNull();
        output!.Id.Should().NotBeEmpty();
        output.Name.Should().Be(exampleCategory.Name);
        output.CreatedAt.Should().NotBeSameDateAs(default);
        output.IsActive.Should().Be(exampleCategory.IsActive);
        output.Description.Should().Be(exampleCategory.Description);
    }

    [Fact]
    [Trait("EndToEnd/API", "Category/Get - Endpoints")]
    public async Task EndToEnd_ThrowError_WhenDoesntExistCategory()
    {
        var exampleCategory = _fixture.GetExampleCategory();
        await _fixture.Persistence.Insert(exampleCategory);

        var randomGuid = Guid.NewGuid();

        var (response, output) = await _fixture.ApiClient.Get<ProblemDetails>($"/categories/{randomGuid}");

        response!.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode) StatusCodes.Status404NotFound);

        output.Should().NotBeNull();
        output!.Type.Should().Be("NotFound");
        output!.Title.Should().Be("Not Found");
        output.Status.Should().Be((int)StatusCodes.Status404NotFound);
        output!.Detail.Should().Be($"Category '{randomGuid}' not found.");
    }

    public void Dispose()
        => _fixture.CleanPersistence();
}
