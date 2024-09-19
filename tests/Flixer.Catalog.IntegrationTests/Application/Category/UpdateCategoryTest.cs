// using Xunit;
// using FluentAssertions;
// using Microsoft.Extensions.Logging;
// using Microsoft.EntityFrameworkCore;
// using Flixer.Catalog.Domain.Exceptions;
// using Flixer.Catalog.Application.Exceptions;
// using Flixer.Catalog.Infra.Data.EF.Repositories;
// using DomainEntity = Flixer.Catalog.Domain.Entities;
// using Flixer.Catalog.Application.Common.Input.Category;
// using Flixer.Catalog.Infra.Data.EF.UnitOfWork;
// using Flixer.Catalog.IntegrationTests.Fixtures.Repository;
// using Flixer.Catalog.UnitTest.Fixture.Application.Category.UpdateCategory;
//
// namespace Flixer.Catalog.IntegrationTests.Application.Category;
//
// [Collection(nameof(CategoryRepositoryFixture))]
// public class UpdateCategoryTest
// {
//     private readonly CategoryRepositoryFixture _fixture;
//     private const string NameDbContext = "integration-tests-db";
//
//     public UpdateCategoryTest(CategoryRepositoryFixture fixture)
//     {
//         _fixture = fixture;
//     }
//
//     [Theory]
//     [Trait("Integration/Application", "UpdateCategory - Command")]
//     [MemberData(
//          nameof(DataGenerator.GetCategoriesToUpdate),
//          parameters: 5,
//          MemberType = typeof(DataGenerator)
//      )]
//     public async Task Command_UpdateCategory(
//          DomainEntity.Category exampleCategory,
//          UpdateCategoryInput input
//      )
//     {
//         var dbContext = _fixture.CreateDbContext(NameDbContext);
//         var unitOfWork = new UnitOfWork(dbContext);
//         
//         var loggerFactory = LoggerFactory.Create(builder =>
//         {
//             builder.SetMinimumLevel(LogLevel.Debug);
//         });
//         
//         var logger = loggerFactory.CreateLogger<Catalog.Application.Commands.Category.UpdateCategory>();
//
//         var trackingInfo = await dbContext.AddAsync(exampleCategory);
//         await dbContext.SaveChangesAsync();
//         trackingInfo.State = EntityState.Detached;
//
//         var repository = new CategoryRepository(dbContext);
//         var command = new Catalog.Application.Commands.Category.UpdateCategory(unitOfWork, logger, repository);
//
//         var output = await command.Handle(input, CancellationToken.None);
//
//         var dbCategory = await _fixture.CreateDbContext(NameDbContext, true)
//             .Categories.FindAsync(output.Id);
//
//         dbCategory.Should().NotBeNull();
//         dbCategory!.Name.Should().Be(input.Name);
//         dbCategory.CreatedAt.Should().Be(output.CreatedAt);
//         dbCategory.Description.Should().Be(input.Description);
//         dbCategory.IsActive.Should().Be((bool)input.IsActive!);
//
//         output.Should().NotBeNull();
//         output.Name.Should().Be(input.Name);
//         output.Description.Should().Be(input.Description);
//         output.IsActive.Should().Be((bool)input.IsActive!);
//     }
//
//     [Theory]
//     [Trait("Integration/Application", "UpdateCategory - Command")]
//     [MemberData(
//         nameof(DataGenerator.GetCategoriesToUpdate),
//         parameters: 5,
//         MemberType = typeof(DataGenerator)
//     )]
//     public async Task Command_UpdateCategoryWithoutIsActive(
//         DomainEntity.Category exampleCategory,
//         UpdateCategoryInput exampleInput
//     )
//     {
//         var input = new UpdateCategoryInput(
//             exampleInput.Id,
//             exampleInput.Name,
//             exampleInput.Description!
//         );
//         
//         var dbContext = _fixture.CreateDbContext(NameDbContext);
//         var unitOfWork = new UnitOfWork(dbContext);
//         
//         var loggerFactory = LoggerFactory.Create(builder =>
//         {
//             builder.SetMinimumLevel(LogLevel.Debug);
//         });
//         
//         var logger = loggerFactory.CreateLogger<Catalog.Application.Commands.Category.UpdateCategory>();
//         
//         await dbContext.AddRangeAsync(_fixture.DataGenerator.GetExampleCategoriesList());
//         
//         var trackingInfo = await dbContext.AddAsync(exampleCategory);
//         await dbContext.SaveChangesAsync();
//         
//         trackingInfo.State = EntityState.Detached;
//         var repository = new CategoryRepository(dbContext);
//         
//         var command = new Catalog.Application.Commands.Category.UpdateCategory(unitOfWork, logger, repository);
//     
//         var output = await command.Handle(input, CancellationToken.None);
//     
//         var dbCategory = await _fixture.CreateDbContext(NameDbContext, true)
//             .Categories.FindAsync(output.Id);
//         
//         output.Should().NotBeNull();
//         dbCategory.Should().NotBeNull();
//         output.Name.Should().Be(input.Name);
//         dbCategory!.Name.Should().Be(input.Name);
//         output.Description.Should().Be(input.Description);
//         dbCategory.CreatedAt.Should().Be(output.CreatedAt);
//         output.IsActive.Should().Be(exampleCategory.IsActive);
//         dbCategory.Description.Should().Be(input.Description);
//         dbCategory.IsActive.Should().Be(exampleCategory.IsActive);
//     }
//     
//     [Fact]
//     [Trait("Integration/Application", "UpdateCategory - Command")]
//     public async Task Command_UpdateThrowsWhenNotFoundCategory()
//     {
//         var input = _fixture.DataGenerator.GetInputUpdate();
//         var dbContext = _fixture.CreateDbContext(NameDbContext);
//         var unitOfWork = new UnitOfWork(dbContext);
//         
//         var loggerFactory = LoggerFactory.Create(builder =>
//         {
//             builder.SetMinimumLevel(LogLevel.Debug);
//         });
//         
//         var logger = loggerFactory.CreateLogger<Catalog.Application.Commands.Category.UpdateCategory>();
//         
//         await dbContext.AddRangeAsync(_fixture.DataGenerator.GetExampleCategoriesList());
//         await dbContext.SaveChangesAsync();
//         
//         var repository = new CategoryRepository(dbContext);
//         var command = new Catalog.Application.Commands.Category.UpdateCategory(unitOfWork, logger, repository);
//     
//         var task = async () => await command.Handle(input, CancellationToken.None);
//     
//         await task.Should().ThrowAsync<NotFoundException>()
//             .WithMessage($"Category '{input.Id}' not found.");
//     }
//     
//     [Theory]
//     [Trait("Integration/Application", "UpdateCategory - Command")]
//     [MemberData(
//         nameof(DataGenerator.GetInvalidUpdateInputs),
//         parameters: 6,
//         MemberType = typeof(DataGenerator)
//     )]
//     public async Task Command_UpdateThrowsWhenCantInstantiateCategory(
//         UpdateCategoryInput input
//     )
//     {
//         var dbContext = _fixture.CreateDbContext(NameDbContext);
//         var exampleCategories = _fixture.DataGenerator.GetExampleCategoriesList();
//         var unitOfWork = new UnitOfWork(dbContext);
//         
//         var loggerFactory = LoggerFactory.Create(builder =>
//         {
//             builder.SetMinimumLevel(LogLevel.Debug);
//         });
//         
//         var logger = loggerFactory.CreateLogger<Catalog.Application.Commands.Category.UpdateCategory>();
//         
//         await dbContext.AddRangeAsync(exampleCategories);
//         await dbContext.SaveChangesAsync();
//         
//         var repository = new CategoryRepository(dbContext);
//         
//         var command = new Catalog.Application.Commands.Category.UpdateCategory(unitOfWork, logger, repository);
//         input.Id = exampleCategories[0].Id;
//     
//         var task = async () => await command.Handle(input, CancellationToken.None);
//     
//         await task.Should().ThrowAsync<EntityValidationException>()
//             .WithMessage("Category is invalid");
//     }
// }
