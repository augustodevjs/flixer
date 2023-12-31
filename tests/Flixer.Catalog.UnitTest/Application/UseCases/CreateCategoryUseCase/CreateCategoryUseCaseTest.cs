﻿using Flixer.Catalog.Domain.Entities;
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
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var repositoryMock = _fixture.GetRepositoryMock();

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
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var repositoryMock = _fixture.GetRepositoryMock();

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
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var repositoryMock = _fixture.GetRepositoryMock();

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

    [Theory(DisplayName = nameof(ThrowWhenCantInstantiateCategory))]
    [Trait("Application", "CreateCategory - Use Cases")]
    [MemberData(
        nameof(CreateCategoryUseCaseTestDataGenerator.GetInvalidInputs), 
        parameters: 24,
        MemberType = typeof(CreateCategoryUseCaseTestDataGenerator)
     )]
    public async void ThrowWhenCantInstantiateCategory(CreateCategoryInput input, string exceptionMessage)
    {
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var repositoryMock = _fixture.GetRepositoryMock();

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
}