using Flixer.Catalog.Domain.Entities;
using Flixer.Catalog.Domain.Exceptions;
using Flixer.Catalog.Application.UseCases.Category.CreateCategory;

namespace Flixer.Catalog.UnitTest.Application.UseCases.CreateCategoryUseCase;

[Collection(nameof(CreateCategoryUseCaseTestFixture))]
public class CreateCategoryUseCaseTest
{
    private readonly CreateCategoryUseCaseTestFixture _fixture;

    public CreateCategoryUseCaseTest(CreateCategoryUseCaseTestFixture fixture) =>
        _fixture = fixture;

    [Fact(DisplayName = nameof(CreateCategory))]
    [Trait("Application", "CreateCategory - Use Cases")]
    public async void CreateCategory()
    {
        var unitOfWorkMock = _fixture.GetUnityOfWorkMock();
        var repositoryMock = _fixture.GetCategoryRepositoryMock();

        var useCase = new CreateCategory(repositoryMock.Object, unitOfWorkMock.Object);

        var input = _fixture.GetInput();

        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Id.Should().NotBeEmpty();
        output.Name.Should().Be(input.Name);
        output.IsActive.Should().Be(input.IsActive);
        output.Description.Should().Be(input.Description);
        output.CreatedAt.Should().NotBeSameDateAs(default);

        unitOfWorkMock.Verify(uow => uow.Commit(It.IsAny<CancellationToken>()), Times.Once);
        repositoryMock.Verify(repository => repository.Insert(
            It.IsAny<Category>(),
            It.IsAny<CancellationToken>()), 
            Times.Once
        );
    }

    [Fact(DisplayName = nameof(CreateCategoryWithOnlyName))]
    [Trait("Application", "CreateCategory - Use Cases")]
    public async void CreateCategoryWithOnlyName()
    {
        var unitOfWorkMock = _fixture.GetUnityOfWorkMock();
        var repositoryMock = _fixture.GetCategoryRepositoryMock();

        var useCase = new CreateCategory(repositoryMock.Object, unitOfWorkMock.Object);

        var input = new CreateCategoryInput(_fixture.GetValidCategoryName());

        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Id.Should().NotBeEmpty();
        output.Name.Should().Be(input.Name);
        output.IsActive.Should().BeTrue();
        output.Description.Should().Be("");
        output.CreatedAt.Should().NotBeSameDateAs(default);

        unitOfWorkMock.Verify(uow => uow.Commit(It.IsAny<CancellationToken>()), Times.Once);
        repositoryMock.Verify(repository => repository.Insert(
            It.IsAny<Category>(),
            It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    [Fact(DisplayName = nameof(CreateCategoryWithOnlyNameAndDescription))]
    [Trait("Application", "CreateCategory - Use Cases")]
    public async void CreateCategoryWithOnlyNameAndDescription()
    {
        var unitOfWorkMock = _fixture.GetUnityOfWorkMock();
        var repositoryMock = _fixture.GetCategoryRepositoryMock();

        var useCase = new CreateCategory(repositoryMock.Object, unitOfWorkMock.Object);

        var input = new CreateCategoryInput(_fixture.GetValidCategoryName(), _fixture.GetValidCategoryDescription());

        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Id.Should().NotBeEmpty();
        output.Name.Should().Be(input.Name);
        output.IsActive.Should().BeTrue();
        output.Description.Should().Be(input.Description);
        output.CreatedAt.Should().NotBeSameDateAs(default);

        unitOfWorkMock.Verify(uow => uow.Commit(It.IsAny<CancellationToken>()), Times.Once);
        repositoryMock.Verify(repository => repository.Insert(
            It.IsAny<Category>(),
            It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    [Theory(DisplayName = nameof(ThrowWhenCantInstantiateAggregate))]
    [Trait("Application", "CreateCategory - Use Cases")]
    [MemberData(nameof(GetInvalidInputs))]
    public async void ThrowWhenCantInstantiateAggregate(CreateCategoryInput input, string exceptionMessage)
    {
        var unitOfWorkMock = _fixture.GetUnityOfWorkMock();
        var repositoryMock = _fixture.GetCategoryRepositoryMock();

        var useCase = new CreateCategory(repositoryMock.Object, unitOfWorkMock.Object);

        Func<Task> task = async () => await useCase.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<EntityValidationException>().WithMessage(exceptionMessage);
        unitOfWorkMock.Verify(uow => uow.Commit(It.IsAny<CancellationToken>()), Times.Never);
        repositoryMock.Verify(repository => repository.Insert(
            It.IsAny<Category>(),
            It.IsAny<CancellationToken>()),
            Times.Never
        );
    }

    public static IEnumerable<object[]> GetInvalidInputs()
    {
        var fixture = new CreateCategoryUseCaseTestFixture();
        var invalidInputsLists = new List<object[]>();

        var invalidInputShortName = fixture.GetInput();
        invalidInputShortName.Name = invalidInputShortName.Name[..2];
        invalidInputsLists.Add(new object[]
        {
            invalidInputShortName,
            "Name should be at least 3 characters long"
        });

        var invalidInputTooLongName = fixture.GetInput();
        var tooLongNameForCategory = fixture.Faker.Commerce.ProductName();
        while (tooLongNameForCategory.Length <= 255)
            tooLongNameForCategory = $"{tooLongNameForCategory} {fixture.Faker.Commerce.ProductName()}";
        invalidInputTooLongName.Name = tooLongNameForCategory;
        invalidInputsLists.Add(new object[]
        {
            invalidInputTooLongName,
            "Name should be less or equal 255 characters long"
        });

        var invalidInputDescriptionNull = fixture.GetInput();
        invalidInputDescriptionNull.Description = null!;
        invalidInputsLists.Add(new object[]
        {
            invalidInputDescriptionNull,
            "Description should not be null"
        });

        var invalidInputTooLongDescription = fixture.GetInput();
        var tooLongDescriptionForCategory = fixture.Faker.Commerce.ProductDescription();
        while (tooLongDescriptionForCategory.Length <= 10000)
            tooLongDescriptionForCategory = $"{tooLongDescriptionForCategory} {fixture.Faker.Commerce.ProductDescription()}";
        invalidInputTooLongDescription.Description = tooLongDescriptionForCategory;
        invalidInputsLists.Add(new object[]
        {
            invalidInputTooLongDescription,
            "Description should be less or equal 10000 characters long"
        });

        return invalidInputsLists;
    }
}