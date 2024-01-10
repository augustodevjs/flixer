using Flixer.Catalog.Application.Exceptions;
using Flixer.Catalog.Application.UseCases.Category.GetCategory;

namespace Flixer.Catalog.UnitTest.Application.UseCases.Category.GetCategoryUseCase;

[Collection(nameof(GetCategoryUseCaseTestFixture))]
public class GetCategoryUseCaseTest
{
    private readonly GetCategoryUseCaseTestFixture _fixture;

    public GetCategoryUseCaseTest(GetCategoryUseCaseTestFixture fixture) => _fixture = fixture;

    [Fact(DisplayName = nameof(GetCategory))]
    [Trait("Application", "GetCategory - Use Cases")]
    public async Task GetCategory()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var exampleCategory = _fixture.GetExampleCategory();

        repositoryMock.Setup(x => x.Get(
            It.IsAny<Guid>(),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(exampleCategory);

        var input = new GetCategoryInput(exampleCategory.Id);
        var useCase = new GetCategory(repositoryMock.Object);

        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Id.Should().Be(exampleCategory.Id);
        output.Name.Should().Be(exampleCategory.Name);
        output.IsActive.Should().Be(exampleCategory.IsActive);
        output.CreatedAt.Should().Be(exampleCategory.CreatedAt);
        output.Description.Should().Be(exampleCategory.Description);

        repositoryMock.Verify(x => x.Get(
            It.IsAny<Guid>(),
            It.IsAny<CancellationToken>()
        ), Times.Once);
    }

    [Fact(DisplayName = nameof(NotFoundExceptionWhenCategoryDoesntExist))]
    [Trait("Application", "GetCategory - Use Cases")]
    public async Task NotFoundExceptionWhenCategoryDoesntExist()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var exampleGuid = Guid.NewGuid();

        repositoryMock.Setup(x => x.Get(
            It.IsAny<Guid>(),
            It.IsAny<CancellationToken>()
        )).ThrowsAsync(
            new NotFoundException($"Category '{exampleGuid}' not found")
        );

        var input = new GetCategoryInput(exampleGuid);
        var useCase = new GetCategory(repositoryMock.Object);

        var task = async () => await useCase.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<NotFoundException>();
        repositoryMock.Verify(x => x.Get(
            It.IsAny<Guid>(),
            It.IsAny<CancellationToken>()
        ), Times.Once);
    }
}
