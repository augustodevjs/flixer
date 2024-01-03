using Flixer.Catalog.Domain.Entities;
using Flixer.Catalog.Domain.Repository;
using UseCases = Flixer.Catalog.UnitTest.Application.UseCases.CreateCategory;

namespace Flixer.Catalog.UnitTest.Application.UseCases.CreateCategoryUseCase;
public class CreateCategoryUseCaseTest
{
    [Fact(DisplayName = nameof(CreateCategoryUseCaseTest))]
    [Trait("Application", "CreateCategory - Use Cases")]
    public async void CreateCategory()
    {
        var unitOfWorkMock = new Mock<IUnityOfWork>();
        var repositoryMock = new Mock<ICategoryRepository>();
        var useCase = new UseCases.CreateCategory(repositoryMock.Object, unitOfWorkMock.Object);

        var input = new CreateCategoryInput(
            "Category Name",
            "Category Description",
            true
        );

        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Name.Should.Be("Category Name");
        output.Description.Should.Be("Category Description");
        output.IsActive.Should.Be(true);
        (output.Id != null && output.Id != Guid.Empty).Should().BeTrue();
        (output.CreatedAt != null && output.CreatedAt != default(DateTime)).Should().BeTrue();

        unitOfWorkMock.Verify(uow => uow.Commit(It.IsAny<CancellationToken>), Times.Once);
        repositoryMock.Verify(repository => repository.Insert(It.IsAny<Category>, It.IsAny<CancellationToken>), Times.Once);
    }
}
