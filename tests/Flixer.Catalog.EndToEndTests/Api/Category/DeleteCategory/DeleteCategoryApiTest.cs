using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Flixer.Catalog.EndToEndTests.Api.Category.Common;

namespace Flixer.Catalog.EndToEndTests.Api.Category.DeleteCategory;

[Collection(nameof(CategoryFixture))]
public class DeleteCategoryApiTest : IDisposable
{
    private readonly CategoryFixture _fixture;

    public DeleteCategoryApiTest(CategoryFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    [Trait("EndToEnd/API", "Category/Delete - Endpoints")]
    public async void EndToEnd_ShouldDeleteCategory_WhenCalledHttpDeleteMethod()
    {
        var exampleCategory = _fixture.GetExampleCategory();

        await _fixture.Persistence.Insert(exampleCategory);

        var (response, output) = await _fixture.ApiClient.Delete<object>($"/categories/{exampleCategory.Id}");
        var persistenceCategory = await _fixture.Persistence.GetById(exampleCategory.Id);

        response!.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status204NoContent);

        output.Should().BeNull();
        persistenceCategory.Should().BeNull();
    }

    [Fact]
    [Trait("EndToEnd/API", "Category/Delete - Endpoints")]
    public async Task EndToEnd_ThrowError_WhenDoesntExistCategory()
    {
        var exampleCategory = _fixture.GetExampleCategory();
        await _fixture.Persistence.Insert(exampleCategory);

        var randomGuid = Guid.NewGuid();

        var (response, output) = await _fixture.ApiClient.Delete<ProblemDetails>($"/categories/{randomGuid}");

        response!.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status404NotFound);

        output.Should().NotBeNull();
        output!.Type.Should().Be("NotFound");
        output!.Title.Should().Be("Not Found");
        output.Status.Should().Be((int)StatusCodes.Status404NotFound);
        output!.Detail.Should().Be($"Category '{randomGuid}' not found.");
    }

    public void Dispose()
        => _fixture.CleanPersistence();
}
