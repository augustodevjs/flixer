﻿using Flixer.Catalog.Application.Dtos.ViewModel.Category;
using System.Net;

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
    [Trait("EndToEnd/API", "Category - Endpoints")]
    public async Task EndToEnd_ShouldCreateCategory_WhenCalledHttpPostMethod()
    {
        var input = _fixture.getExampleInput();

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
}
