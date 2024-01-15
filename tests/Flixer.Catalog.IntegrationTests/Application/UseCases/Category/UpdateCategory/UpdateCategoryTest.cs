﻿//using Flixer.Catalog.Infra.Data.EF;
//using Microsoft.EntityFrameworkCore;
//using Flixer.Catalog.Domain.Exceptions;
//using Flixer.Catalog.Application.Exceptions;
//using Flixer.Catalog.Infra.Data.EF.Repositories;
//using DomainEntity = Flixer.Catalog.Domain.Entities;
//using Flixer.Catalog.Application.Dtos.InputModel.Category;
//using UseCase = Flixer.Catalog.Application.UseCases.Category;

//namespace Flixer.Catalog.IntegrationTests.Application.UseCases.Category.UpdateCategory;

//[Collection(nameof(UpdateCategoryTestFixture))]
//public class UpdateCategoryTest
//{
//    private readonly UpdateCategoryTestFixture _fixture;

//    public UpdateCategoryTest(UpdateCategoryTestFixture fixture)
//    {
//        _fixture = fixture;
//    }

//    [Theory(DisplayName = nameof(UpdateCategory))]
//    [Trait("Integration/Application", "UpdateCategory - Use Cases")]
//    [MemberData(
//         nameof(UpdateCategoryTestDataGenerator.GetCategoriesToUpdate),
//         parameters: 5,
//         MemberType = typeof(UpdateCategoryTestDataGenerator)
//     )]
//    public async Task UpdateCategory(
//         DomainEntity.Category exampleCategory,
//         UpdateCategoryInputModel input
//     )
//    {
//        var dbContext = _fixture.CreateDbContext();

//        var trackingInfo = await dbContext.AddAsync(exampleCategory);
//        dbContext.SaveChanges();
//        trackingInfo.State = EntityState.Detached;

//        var unitOfWork = new UnityOfWork(dbContext);
//        var repository = new CategoryRepository(dbContext);
//        var useCase = new UseCase.UpdateCategory(repository, unitOfWork);

//        var output = await useCase.Handle(input, CancellationToken.None);

//        var dbCategory = await (_fixture.CreateDbContext(true)).Categories.FindAsync(output.Id);

//        dbCategory.Should().NotBeNull();
//        dbCategory!.Name.Should().Be(input.Name);
//        dbCategory.CreatedAt.Should().Be(output.CreatedAt);
//        dbCategory.Description.Should().Be(input.Description);
//        dbCategory.IsActive.Should().Be((bool)input.IsActive!);

//        output.Should().NotBeNull();
//        output.Name.Should().Be(input.Name);
//        output.Description.Should().Be(input.Description);
//        output.IsActive.Should().Be((bool)input.IsActive!);
//    }

//    [Theory(DisplayName = nameof(UpdateCategoryWithoutIsActive))]
//    [Trait("Integration/Application", "UpdateCategory - Use Cases")]
//    [MemberData(
//        nameof(UpdateCategoryTestDataGenerator.GetCategoriesToUpdate),
//        parameters: 5,
//        MemberType = typeof(UpdateCategoryTestDataGenerator)
//    )]
//    public async Task UpdateCategoryWithoutIsActive(
//        DomainEntity.Category exampleCategory,
//        UpdateCategoryInputModel exampleInput
//    )
//    {
//        var input = new UpdateCategoryInputModel(
//            exampleInput.Id,
//            exampleInput.Name,
//            null,
//            exampleInput.Description
//        );
//        var dbContext = _fixture.CreateDbContext();
//        await dbContext.AddRangeAsync(_fixture.GetExampleCategoriesList());
//        var trackingInfo = await dbContext.AddAsync(exampleCategory);
//        dbContext.SaveChanges();
//        trackingInfo.State = EntityState.Detached;
//        var repository = new CategoryRepository(dbContext);
//        var unitOfWork = new UnityOfWork(dbContext);
//        var useCase = new UseCase.UpdateCategory(repository, unitOfWork);

//        var output = await useCase.Handle(input, CancellationToken.None);

//        var dbCategory = await (_fixture.CreateDbContext(true))
//            .Categories.FindAsync(output.Id);
//        dbCategory.Should().NotBeNull();
//        dbCategory!.Name.Should().Be(input.Name);
//        dbCategory.Description.Should().Be(input.Description);
//        dbCategory.IsActive.Should().Be(exampleCategory.IsActive);
//        dbCategory.CreatedAt.Should().Be(output.CreatedAt);
//        output.Should().NotBeNull();
//        output.Name.Should().Be(input.Name);
//        output.Description.Should().Be(input.Description);
//        output.IsActive.Should().Be(exampleCategory.IsActive);
//    }

//    [Theory(DisplayName = nameof(UpdateCategoryOnlyName))]
//    [Trait("Integration/Application", "UpdateCategory - Use Cases")]
//    [MemberData(
//        nameof(UpdateCategoryTestDataGenerator.GetCategoriesToUpdate),
//        parameters: 5,
//        MemberType = typeof(UpdateCategoryTestDataGenerator)
//    )]
//    public async Task UpdateCategoryOnlyName(
//        DomainEntity.Category exampleCategory,
//        UpdateCategoryInputModel exampleInput
//    )
//    {
//        var input = new UpdateCategoryInputModel(
//            exampleInput.Id,
//            exampleInput.Name
//        );
//        var dbContext = _fixture.CreateDbContext();
//        await dbContext.AddRangeAsync(_fixture.GetExampleCategoriesList());
//        var trackingInfo = await dbContext.AddAsync(exampleCategory);
//        dbContext.SaveChanges();
//        trackingInfo.State = EntityState.Detached;
//        var repository = new CategoryRepository(dbContext);
//        var unitOfWork = new UnityOfWork(dbContext);
//        var useCase = new UseCase.UpdateCategory(repository, unitOfWork);

//        var output = await useCase.Handle(input, CancellationToken.None);

//        var dbCategory = await (_fixture.CreateDbContext(true))
//            .Categories.FindAsync(output.Id);
//        dbCategory.Should().NotBeNull();
//        dbCategory!.Name.Should().Be(input.Name);
//        dbCategory.Description.Should().Be(exampleCategory.Description);
//        dbCategory.IsActive.Should().Be(exampleCategory.IsActive);
//        dbCategory.CreatedAt.Should().Be(output.CreatedAt);
//        output.Should().NotBeNull();
//        output.Name.Should().Be(input.Name);
//        output.Description.Should().Be(exampleCategory.Description);
//        output.IsActive.Should().Be(exampleCategory.IsActive);
//    }

//    [Fact(DisplayName = nameof(UpdateThrowsWhenNotFoundCategory))]
//    [Trait("Integration/Application", "UpdateCategory - Use Cases")]
//    public async Task UpdateThrowsWhenNotFoundCategory()
//    {
//        var input = _fixture.GetValidInput();
//        var dbContext = _fixture.CreateDbContext();
//        await dbContext.AddRangeAsync(_fixture.GetExampleCategoriesList());
//        dbContext.SaveChanges();
//        var repository = new CategoryRepository(dbContext);
//        var unitOfWork = new UnityOfWork(dbContext);
//        var useCase = new UseCase.UpdateCategory(repository, unitOfWork);

//        var task = async ()
//            => await useCase.Handle(input, CancellationToken.None);

//        await task.Should().ThrowAsync<NotFoundException>()
//            .WithMessage($"Category '{input.Id}' not found.");
//    }

//    [Theory(DisplayName = nameof(UpdateThrowsWhenCantInstantiateCategory))]
//    [Trait("Integration/Application", "UpdateCategory - Use Cases")]
//    [MemberData(
//        nameof(UpdateCategoryTestDataGenerator.GetInvalidInputs),
//        parameters: 6,
//        MemberType = typeof(UpdateCategoryTestDataGenerator)
//    )]
//    public async Task UpdateThrowsWhenCantInstantiateCategory(
//        UpdateCategoryInputModel input,
//        string expectedExceptionMessage
//    )
//    {
//        var dbContext = _fixture.CreateDbContext();
//        var exampleCategories = _fixture.GetExampleCategoriesList();
//        await dbContext.AddRangeAsync(exampleCategories);
//        dbContext.SaveChanges();
//        var repository = new CategoryRepository(dbContext);
//        var unitOfWork = new UnityOfWork(dbContext);
//        var useCase = new UseCase.UpdateCategory(repository, unitOfWork);
//        input.Id = exampleCategories[0].Id;

//        var task = async ()
//            => await useCase.Handle(input, CancellationToken.None);

//        await task.Should().ThrowAsync<EntityValidationException>()
//            .WithMessage(expectedExceptionMessage);
//    }
//}
