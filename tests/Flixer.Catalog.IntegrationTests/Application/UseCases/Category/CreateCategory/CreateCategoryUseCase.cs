using Flixer.Catalog.Infra.Data.EF.Context;
using Flixer.Catalog.Infra.Data.EF.Repositories;
using Flixer.Catalog.Common.Tests.Fixture.Category;
using Flixer.Catalog.Application.Dtos.InputModel.Category;
using ApplicationUseCase = Flixer.Catalog.Application.UseCases.Category;

namespace Flixer.Catalog.IntegrationTests.Application.UseCases.Category.CreateCategory;

[Collection(nameof(CategoryTestFixture))]
public class CreateCategoryUseCase
{
    private readonly CategoryTestFixture _fixture;
    public const string nameDbContext = "integration-tests-db";

    public CreateCategoryUseCase(CategoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(CreateCategory))]
    [Trait("Integration/Application", "CreateCategory - Use Cases")]
    public async void CreateCategory()
    {
        var input = _fixture.GetInputCreate();
        var dbContext = _fixture.CreateDbContext(nameDbContext);

        var unityOfWork = new UnityOfWork(dbContext);
        var repository = new CategoryRepository(dbContext);

        var useCase = new ApplicationUseCase.CreateCategory(repository, unityOfWork);

        var output = await useCase.Handle(input, CancellationToken.None);

        var dbCategory = await (_fixture.CreateDbContext(nameDbContext, true)).Categories.FindAsync(output.Id);

        dbCategory.Should().NotBeNull();
        dbCategory!.Name.Should().Be(input.Name);
        dbCategory.IsActive.Should().Be(input.IsActive);
        dbCategory.CreatedAt.Should().Be(output.CreatedAt);
        dbCategory.Description.Should().Be(input.Description);

        output.Should().NotBeNull();
        output.Id.Should().NotBeEmpty();
        output.Name.Should().Be(input.Name);
        output.IsActive.Should().Be(input.IsActive);
        output.Description.Should().Be(input.Description);
        output.CreatedAt.Should().NotBeSameDateAs(default);
    }

    [Fact(DisplayName = nameof(CreateCategoryOnlyWithName))]
    [Trait("Integration/Application", "CreateCategory - Use Cases")]
    public async void CreateCategoryOnlyWithName()
    {
        var dbContext = _fixture.CreateDbContext(nameDbContext);

        var repository = new CategoryRepository(dbContext);
        var unityOfWork = new UnityOfWork(dbContext);

        var useCase = new ApplicationUseCase.CreateCategory(repository, unityOfWork);

        var input = new CreateCategoryInputModel(_fixture.GetValidCategory().Name);

        var output = await useCase.Handle(input, CancellationToken.None);

        var dbCategory = await (_fixture.CreateDbContext(nameDbContext, true)).Categories.FindAsync(output.Id);

        dbCategory.Should().NotBeNull();
        dbCategory!.IsActive.Should().Be(true);
        dbCategory.Description.Should().Be("");
        dbCategory!.Name.Should().Be(input.Name);
        dbCategory.CreatedAt.Should().Be(output.CreatedAt);

        output.Should().NotBeNull();
        output.Id.Should().NotBeEmpty();
        output.IsActive.Should().Be(true);
        output.Description.Should().Be("");
        output.Name.Should().Be(input.Name);
        output.CreatedAt.Should().NotBeSameDateAs(default);
    }

    [Fact(DisplayName = nameof(CreateCategoryOnlyWithNameAndDescription))]
    [Trait("Integration/Application", "CreateCategory - Use Cases")]
    public async void CreateCategoryOnlyWithNameAndDescription()
    {
        var exampleInput = _fixture.GetInputCreate();
        var dbContext = _fixture.CreateDbContext(nameDbContext);

        var unityOfWork = new UnityOfWork(dbContext);
        var repository = new CategoryRepository(dbContext);

        var useCase = new ApplicationUseCase.CreateCategory(repository, unityOfWork);

        var input = new CreateCategoryInputModel(exampleInput.Name, exampleInput.Description);

        var output = await useCase.Handle(input, CancellationToken.None);

        var dbCategory = await (_fixture.CreateDbContext(nameDbContext, true)).Categories.FindAsync(output.Id);

        dbCategory.Should().NotBeNull();
        dbCategory!.IsActive.Should().Be(true);
        dbCategory!.Name.Should().Be(input.Name);
        dbCategory.CreatedAt.Should().Be(output.CreatedAt);
        dbCategory.Description.Should().Be(input.Description);

        output.Should().NotBeNull();
        output.Id.Should().NotBeEmpty();
        output.IsActive.Should().Be(true);
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.CreatedAt.Should().NotBeSameDateAs(default);
    }
}
