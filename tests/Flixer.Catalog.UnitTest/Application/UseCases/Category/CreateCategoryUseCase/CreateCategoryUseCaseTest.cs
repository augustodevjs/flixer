﻿using Flixer.Catalog.Domain.Exceptions; 
using Flixer.Catalog.Application.UseCases.Category;
using DomainEntity = Flixer.Catalog.Domain.Entities;
using Flixer.Catalog.Application.Dtos.InputModel.Category;
 
namespace Flixer.Catalog.UnitTest.Application.UseCases.Category.CreateCategoryUseCase;

[Collection(nameof(CreateCategoryUseCaseTestFixture))]
public class CreateCategoryUseCaseTest
{
    private readonly CreateCategoryUseCaseTestFixture _fixture;

    public CreateCategoryUseCaseTest(CreateCategoryUseCaseTestFixture fixture) =>
        _fixture = fixture;

    [Fact]
    [Trait("Application", "CreateCategory - Use Cases")]
    public async void UseCase_ShouldCreateCategory_WhenMethodHandleIsCalled()
    {
        var unityOfWorkMock = _fixture.GetUnitOfWorkMock();
        var repositoryMock = _fixture.GetRepositoryMock();

        var useCase = new CreateCategory(repositoryMock.Object, unityOfWorkMock.Object);

        var input = _fixture.GetInput();

        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Id.Should().NotBeEmpty();
        output.Name.Should().Be(input.Name);
        output.IsActive.Should().Be(input.IsActive);
        output.Description.Should().Be(input.Description);
        output.CreatedAt.Should().NotBeSameDateAs(default);

        unityOfWorkMock.Verify(uow => uow.Commit(It.IsAny<CancellationToken>()), Times.Once);
        repositoryMock.Verify(repository => repository.Insert(
            It.IsAny<DomainEntity.Category>(),
            It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    [Fact]
    [Trait("Application", "CreateCategory - Use Cases")]
    public async void UseCase_ShouldCreateCategory_WhenMethodHandleIsCalledWithOnlyName()
    {
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var repositoryMock = _fixture.GetRepositoryMock();

        var useCase = new CreateCategory(repositoryMock.Object, unitOfWorkMock.Object);

        var input = new CreateCategoryInputModel(_fixture.GetValidCategoryName());

        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Id.Should().NotBeEmpty();
        output.Name.Should().Be(input.Name);
        output.IsActive.Should().BeTrue();
        output.Description.Should().Be("");
        output.CreatedAt.Should().NotBeSameDateAs(default);

        unitOfWorkMock.Verify(uow => uow.Commit(It.IsAny<CancellationToken>()), Times.Once);
        repositoryMock.Verify(repository => repository.Insert(
            It.IsAny<DomainEntity.Category>(),
            It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    [Fact]
    [Trait("Application", "CreateCategory - Use Cases")]
    public async void UseCase_ShouldCreateCategory_WhenMethodHandleIsCalledWithOnlyNameAndDescription()
    {
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var repositoryMock = _fixture.GetRepositoryMock();

        var useCase = new CreateCategory(repositoryMock.Object, unitOfWorkMock.Object);

        var input = new CreateCategoryInputModel(_fixture.GetValidCategoryName(), _fixture.GetValidCategoryDescription());

        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Id.Should().NotBeEmpty();
        output.Name.Should().Be(input.Name);
        output.IsActive.Should().BeTrue();
        output.Description.Should().Be(input.Description);
        output.CreatedAt.Should().NotBeSameDateAs(default);

        unitOfWorkMock.Verify(uow => uow.Commit(It.IsAny<CancellationToken>()), Times.Once);
        repositoryMock.Verify(repository => repository.Insert(
            It.IsAny<DomainEntity.Category>(),
            It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    [Theory]
    [Trait("Application", "CreateCategory - Use Cases")]
    [MemberData(
        nameof(CreateCategoryUseCaseTestDataGenerator.GetInvalidInputs),
        parameters: 24,
        MemberType = typeof(CreateCategoryUseCaseTestDataGenerator)
     )]
    public async void UseCase_ShouldThrowError_WhenMethodHandleIsCalledWithInvalidInputs(CreateCategoryInputModel input, string exceptionMessage)
    {
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var repositoryMock = _fixture.GetRepositoryMock();

        var useCase = new CreateCategory(repositoryMock.Object, unitOfWorkMock.Object);

        Func<Task> task = async () => await useCase.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<EntityValidationException>().WithMessage(exceptionMessage);
        unitOfWorkMock.Verify(uow => uow.Commit(It.IsAny<CancellationToken>()), Times.Never);
        repositoryMock.Verify(repository => repository.Insert(
            It.IsAny<DomainEntity.Category>(),
            It.IsAny<CancellationToken>()),
            Times.Never
        );
    }
}