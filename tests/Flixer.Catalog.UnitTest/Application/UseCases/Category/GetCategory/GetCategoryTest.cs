using Flixer.Catalog.Application.Exceptions;
using Flixer.Catalog.Common.Tests.Fixture.Category;
using Flixer.Catalog.Application.Dtos.InputModel.Category;
using UseCase = Flixer.Catalog.Application.UseCases.Category;

namespace Flixer.Catalog.UnitTest.Application.UseCases.Category.GetCategory;

[Collection(nameof(CategoryTestFixture))]
public class GetCategoryTest
{
    private readonly CategoryTestFixture _fixture;

    public GetCategoryTest(CategoryTestFixture fixture) => _fixture = fixture;

    [Fact]
    [Trait("Application", "GetCategory - Use Cases")]
    public async Task UseCase_ShouldGetCategory_WhenMethodHandleIsCalled()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var exampleCategory = _fixture.GetValidCategory();

        repositoryMock.Setup(x => x.Get(
            It.IsAny<Guid>(),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(exampleCategory);

        var input = new GetCategoryInputModel(exampleCategory.Id);
        var useCase = new UseCase.GetCategory(repositoryMock.Object);

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

    [Fact]
    [Trait("Application", "GetCategory - Use Cases")]
    public async Task UseCase_ShouldThrowException_WhenCategoryDoesntExist()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var exampleGuid = Guid.NewGuid();

        var input = new GetCategoryInputModel(exampleGuid);
        var useCase = new UseCase.GetCategory(repositoryMock.Object);

        var task = async () => await useCase.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<NotFoundException>().WithMessage($"Category '{exampleGuid}' not found.");
        repositoryMock.Verify(x => x.Get(
            It.IsAny<Guid>(),
            It.IsAny<CancellationToken>()
        ), Times.Once);
    }
}
