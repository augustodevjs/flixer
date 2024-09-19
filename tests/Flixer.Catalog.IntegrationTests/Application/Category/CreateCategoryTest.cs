// using Xunit;
// using FluentAssertions;
// using Microsoft.Extensions.Logging;
// using Flixer.Catalog.Infra.Data.EF.Repositories;
// using Flixer.Catalog.Application.Common.Input.Category;
// using Flixer.Catalog.Infra.Data.EF.UnitOfWork;
// using Flixer.Catalog.IntegrationTests.Fixtures.Repository;
//
// namespace Flixer.Catalog.IntegrationTests.Application.Category;
//
// [Collection(nameof(CategoryRepositoryFixture))]
// public class CreateCategoryTest
// {
//     private readonly CategoryRepositoryFixture _fixture;
//     private const string NameDbContext = "integration-tests-db";
//
//     public CreateCategoryTest(CategoryRepositoryFixture fixture)
//     {
//         _fixture = fixture;
//     }
//
//     [Fact]
//     [Trait("Integration/Application", "CreateCategory - Command")]
//     public async void Command_CreateCategory()
//     {
//         var input = _fixture.DataGenerator.GetInputCreate();
//         var dbContext = _fixture.CreateDbContext(NameDbContext);
//         
//         var unitOfWork = new UnitOfWork(dbContext);
//         
//         var loggerFactory = LoggerFactory.Create(builder =>
//         {
//             builder.SetMinimumLevel(LogLevel.Debug);
//         });
//         
//         var logger = loggerFactory.CreateLogger<Catalog.Application.Commands.Category.CreateCategory>();
//
//         var repository = new CategoryRepository(dbContext);
//
//         var command = new Catalog.Application.Commands.Category.CreateCategory(unitOfWork, logger, repository);
//
//         var output = await command.Handle(input, CancellationToken.None);
//
//         var dbCategory = await _fixture.CreateDbContext(NameDbContext, true)
//             .Categories.FindAsync(output.Id);
//
//         dbCategory.Should().NotBeNull();
//         dbCategory!.Name.Should().Be(input.Name);
//         dbCategory.IsActive.Should().Be(input.IsActive);
//         dbCategory.CreatedAt.Should().Be(output.CreatedAt);
//         dbCategory.Description.Should().Be(input.Description);
//
//         output.Should().NotBeNull();
//         output.Id.Should().NotBeEmpty();
//         output.Name.Should().Be(input.Name);
//         output.IsActive.Should().Be(input.IsActive);
//         output.Description.Should().Be(input.Description);
//         output.CreatedAt.Should().NotBeSameDateAs(default);
//     }
//     
//     [Fact]
//     [Trait("Integration/Application", "CreateCategory - Command")]
//     public async void Command_CreateCategoryOnlyWithNameAndDescription()
//     {
//         var exampleInput = _fixture.DataGenerator.GetInputCreate();
//         var dbContext = _fixture.CreateDbContext(NameDbContext);
//         
//         var unitOfWork = new UnitOfWork(dbContext);
//         
//         var loggerFactory = LoggerFactory.Create(builder =>
//         {
//             builder.SetMinimumLevel(LogLevel.Debug);
//         });
//         
//         var logger = loggerFactory.CreateLogger<Catalog.Application.Commands.Category.CreateCategory>();
//     
//         var repository = new CategoryRepository(dbContext);
//     
//         var command = new Catalog.Application.Commands.Category.CreateCategory(unitOfWork, logger, repository);
//     
//         var input = new CreateCategoryInput(exampleInput.Name, exampleInput.Description);
//     
//         var output = await command.Handle(input, CancellationToken.None);
//     
//         var dbCategory = await _fixture.CreateDbContext(NameDbContext, true)
//             .Categories.FindAsync(output.Id);
//     
//         dbCategory.Should().NotBeNull();
//         dbCategory!.IsActive.Should().Be(true);
//         dbCategory!.Name.Should().Be(input.Name);
//         dbCategory.CreatedAt.Should().Be(output.CreatedAt);
//         dbCategory.Description.Should().Be(input.Description);
//     
//         output.Should().NotBeNull();
//         output.Id.Should().NotBeEmpty();
//         output.IsActive.Should().Be(true);
//         output.Name.Should().Be(input.Name);
//         output.Description.Should().Be(input.Description);
//         output.CreatedAt.Should().NotBeSameDateAs(default);
//     }
// }
