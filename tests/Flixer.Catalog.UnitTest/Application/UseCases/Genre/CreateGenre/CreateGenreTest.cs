﻿//using Flixer.Catalog.Domain.Exceptions;
//using Flixer.Catalog.Application.Exceptions;
//using Flixer.Catalog.Common.Tests.Fixture.Genre;
//using Flixer.Catalog.Application.UseCases.Genre;
//using DomainEntity = Flixer.Catalog.Domain.Entities;

//namespace Flixer.Catalog.UnitTest.Application.UseCases.Genre.CreateGenreUseCase;

//[Collection(nameof(GenreTestFixture))]
//public class CreateGenreTest
//{
//    private readonly GenreTestFixture _fixture;

//    public CreateGenreTest(GenreTestFixture fixture)
//        => _fixture = fixture;

//    [Fact]
//    [Trait("Application", "CreateGenre - Use Cases")]
//    public async Task UseCase_ShouldCreateGenre_WhenMethodHandleIsCalled()
//    {
//        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
//        var genreRepositoryMock = _fixture.GetGenreRepositoryMock();
//        var categoryRepositoryMock = _fixture.GetCategoryRepositoryMock();

//        var useCase = new CreateGenre(
//            unitOfWorkMock.Object,
//            genreRepositoryMock.Object,
//            categoryRepositoryMock.Object
//        );

//        var input = _fixture.GetGenreCreateInput();

//        var datetimeBefore = DateTime.Now;

//        var output = await useCase.Handle(input, CancellationToken.None);

//        var datetimeAfter = DateTime.Now.AddSeconds(1);

//        output.Should().NotBeNull();
//        output.Id.Should().NotBeEmpty();
//        output.Name.Should().Be(input.Name);
//        output.Categories.Should().HaveCount(0);
//        output.IsActive.Should().Be(input.IsActive);
//        output.CreatedAt.Should().NotBeSameDateAs(default);
//        (output.CreatedAt <= datetimeAfter).Should().BeTrue();
//        (output.CreatedAt >= datetimeBefore).Should().BeTrue();

//        genreRepositoryMock.Verify(x => x.Insert(It.IsAny<DomainEntity.Genre>(), It.IsAny<CancellationToken>()), Times.Once);
//        unitOfWorkMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Once);
//    }

//    [Fact]
//    [Trait("Application", "CreateGenre - Use Cases")]
//    public async Task UseCase_ShouldCreateGenre_WhenMethodHandleIsCalledWithRelatedCategories()
//    {
//        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
//        var input = _fixture.GetGenreCreateInputWithCategories();
//        var genreRepositoryMock = _fixture.GetGenreRepositoryMock();
//        var categoryRepositoryMock = _fixture.GetCategoryRepositoryMock();

//        categoryRepositoryMock.Setup(x => x.GetIdsListByIds(
//            It.IsAny<List<Guid>>(), It.IsAny<CancellationToken>()
//        )).ReturnsAsync(input.CategoriesIds!);

//        var useCase = new CreateGenre(
//            unitOfWorkMock.Object,
//            genreRepositoryMock.Object,
//            categoryRepositoryMock.Object
//        );

//        var output = await useCase.Handle(input, CancellationToken.None);

//        output.Should().NotBeNull();
//        output.Id.Should().NotBeEmpty();
//        output.Name.Should().Be(input.Name);
//        output.IsActive.Should().Be(input.IsActive);
//        output.CreatedAt.Should().NotBeSameDateAs(default);
//        output.Categories.Should().HaveCount(input.CategoriesIds?.Count ?? 0);
//        input.CategoriesIds?.ForEach(id => output.Categories.Should().Contain(id));

//        genreRepositoryMock.Verify(x => x.Insert(It.IsAny<DomainEntity.Genre>(), It.IsAny<CancellationToken>()), Times.Once);
//        unitOfWorkMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Once);
//    }

//    [Fact]
//    [Trait("Application", "CreateGenre - Use Cases")]
//    public async Task UseCase_ShouldThrowError_WhenRelatedCategoryNotFound()
//    {
//        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
//        var input = _fixture.GetGenreCreateInputWithCategories();
//        var genreRepositoryMock = _fixture.GetGenreRepositoryMock();
//        var categoryRepositoryMock = _fixture.GetCategoryRepositoryMock();

//        var exampleGuid = input.CategoriesIds![^1];

//        categoryRepositoryMock.Setup(x => x.GetIdsListByIds(
//                It.IsAny<List<Guid>>(),
//                It.IsAny<CancellationToken>()
//            )
//        ).ReturnsAsync(input.CategoriesIds.FindAll(x => x != exampleGuid));

//        var useCase = new CreateGenre(
//            unitOfWorkMock.Object,
//            genreRepositoryMock.Object,
//            categoryRepositoryMock.Object
//        );

//        var action = async () => await useCase.Handle(input, CancellationToken.None);

//        await action.Should().ThrowAsync<RelatedAggregateException>().WithMessage($"Related category id (or ids) not found: {exampleGuid}");

//        categoryRepositoryMock.Verify(x => x.GetIdsListByIds(
//            It.IsAny<List<Guid>>(),
//            It.IsAny<CancellationToken>())
//        , Times.Once);

//        unitOfWorkMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Never);
//    }

//    [Theory]
//    [Trait("Application", "CreateGenre - Use Cases")]
//    [InlineData("")]
//    [InlineData(null)]
//    [InlineData("  ")]
//    public async Task UseCase_ShouldThrowErro_WhenNameIsInvalid(string name)
//    {
//        var input = _fixture.GetGenreCreateInput(name);
//        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
//        var genreRepositoryMock = _fixture.GetGenreRepositoryMock();
//        var categoryRepositoryMock = _fixture.GetCategoryRepositoryMock();

//        var useCase = new CreateGenre(
//            unitOfWorkMock.Object,
//            genreRepositoryMock.Object,
//            categoryRepositoryMock.Object
//        );

//        var action = async () => await useCase.Handle(input, CancellationToken.None);

//        await action.Should().ThrowAsync<EntityValidationException>().WithMessage($"Name should not be empty or null");
//        unitOfWorkMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Never);
//    }
//}