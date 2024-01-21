using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Flixer.Catalog.Application.Dtos.InputModel.Category;
using Flixer.Catalog.Application.Dtos.ViewModel.Category;

namespace Flixer.Catalog.EndToEndTests.Api.Category.CreateCategory;

[Collection(nameof(CreateCategoryApiTestFixture))]
public class CreateCategoryApiTest
{
    private readonly CreateCategoryApiTestFixture _fixture;

    public CreateCategoryApiTest(CreateCategoryApiTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    [Trait("EndToEnd/API", "Category/Create - Endpoints")]
    public async Task EndToEnd_ShouldCreateCategory_WhenCalledHttpPostMethod()
    {
        var input = _fixture.GetExampleInput();

        var (response, output) = await _fixture.ApiClient.Post<CategoryViewModel>("/categories", input);

        var dbCategory = await _fixture.Persistence.GetById(output!.Id);

        response!.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.Created);

        output.Should().NotBeNull();
        output.Id.Should().NotBeEmpty();
        output.Name.Should().Be(input.Name);
        output.IsActive.Should().Be(input.IsActive);
        output.Description.Should().Be(input.Description);
        output.CreatedAt.Should().NotBeSameDateAs(default);

        dbCategory.Should().NotBeNull();
        dbCategory!.Id.Should().NotBeEmpty();
        dbCategory.Name.Should().Be(input.Name);
        dbCategory.IsActive.Should().Be(input.IsActive);
        dbCategory.Description.Should().Be(input.Description);
        dbCategory.CreatedAt.Should().NotBeSameDateAs(default);
    }

    [Theory]
    [Trait("EndToEnd/API", "Category/Create - Endpoints")]
    [MemberData(
        nameof(CreateCategoryApiTestDataGenerator.GetInvalidInputs),
        MemberType = typeof(CreateCategoryApiTestDataGenerator)
    )]
    public async Task EndToEnd_ThrowError_WhenCantInstantieteAggregate(
        CreateCategoryInputModel input,
        string expectedDetails
    )
    {
        var (response, output) = await _fixture.ApiClient.Post<ProblemDetails>("/categories", input);

        response!.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);

        output.Should().NotBeNull();
        output!.Detail.Should().Be(expectedDetails);
        output.Type.Should().Be("UnprocessableEntity");
        output!.Title.Should().Be("One or more validation errors ocurred");
        output.Status.Should().Be((int)StatusCodes.Status422UnprocessableEntity);
    }
}