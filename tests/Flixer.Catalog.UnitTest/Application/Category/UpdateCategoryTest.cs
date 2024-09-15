using Moq;
using Xunit;
using FluentAssertions;
using Flixer.Catalog.Domain.Exceptions;
using Flixer.Catalog.Application.Exceptions;
using Flixer.Catalog.Application.Commands.Category;
using DomainEntity = Flixer.Catalog.Domain.Entities;
using Flixer.Catalog.Application.Common.Input.Category;
using Flixer.Catalog.UnitTest.Fixture.Application.Category.UpdateCategory;

namespace Flixer.Catalog.UnitTest.Application.Category;

[Collection(nameof(UpdateCategoryFixture))]
public class UpdateCategoryTest
{
    private readonly UpdateCategoryFixture _fixture;

    public UpdateCategoryTest(UpdateCategoryFixture fixture)
        => _fixture = fixture;

     [Theory]
     [Trait("Application", "UpdateCategory - Command")]
     [MemberData(
         nameof(DataGenerator.GetCategoriesToUpdate),
         parameters: 20,
         MemberType = typeof(DataGenerator)
     )]
     public async Task Command_ShouldUpdateCategory_WhenMethodHandleIsCalled(
         DomainEntity.Category exampleCategory,
         UpdateCategoryInput input
     )
     {
         var loggerMock = _fixture.GetLoggerMock();
         var repositoryMock = _fixture.GetRepositoryMock();
         var unitOfWorkMock = _fixture.GetUnitOfWorkMock();

         repositoryMock.Setup(x => x.GetById(
             exampleCategory.Id)
         ).ReturnsAsync(exampleCategory);
         
         unitOfWorkMock.Setup(uow => uow.Commit())
             .ReturnsAsync(true);

         var command = new UpdateCategory(unitOfWorkMock.Object, loggerMock.Object, repositoryMock.Object);
         
         var output = await command.Handle(input, CancellationToken.None);

         output.Should().NotBeNull();
         output.Name.Should().Be(input.Name);
         output.Description.Should().Be(input.Description);
         output.IsActive.Should().Be((bool)input.IsActive!);
         
         unitOfWorkMock.Verify(uow => uow.Commit(), Times.Once);
         repositoryMock.Verify(x => x.Update(exampleCategory), Times.Once);
         repositoryMock.Verify(x => x.GetById(exampleCategory.Id), Times.Once);
     }
     
     [Theory]
     [Trait("Application", "UpdateCategory - Command")]
     [MemberData(
         nameof(DataGenerator.GetCategoriesToUpdate),
         parameters: 20,
         MemberType = typeof(DataGenerator)
     )]
     public async Task Command_ShouldUpdateCategory_WhenMethodHandleIsCalledWithoutProvidingIsActive(
         DomainEntity.Category exampleCategory,
         UpdateCategoryInput exampleInput
     )
     {
         var input = new UpdateCategoryInput(
             exampleInput.Id,
             exampleInput.Name,
             exampleInput.Description!
         );

         var loggerMock = _fixture.GetLoggerMock();
         var repositoryMock = _fixture.GetRepositoryMock();
         var unitOfWorkMock = _fixture.GetUnitOfWorkMock();

         repositoryMock.Setup(x => x
             .GetById(exampleCategory.Id)
         ).ReturnsAsync(exampleCategory);
         
         unitOfWorkMock.Setup(uow => uow.Commit())
             .ReturnsAsync(true);

         var command = new UpdateCategory(
             unitOfWorkMock.Object,
             loggerMock.Object,
             repositoryMock.Object
         );

         var output = await command.Handle(input, CancellationToken.None);

         output.Should().NotBeNull();
         output.Name.Should().Be(input.Name);
         output.Description.Should().Be(input.Description);
         output.IsActive.Should().Be(exampleCategory.IsActive);
         
         unitOfWorkMock.Verify(uow => uow.Commit(), Times.Once);
         repositoryMock.Verify(x => x.Update(exampleCategory), Times.Once);
         repositoryMock.Verify(x => x.GetById(exampleCategory.Id), Times.Once);
     }
     
     [Theory]
     [Trait("Application", "UpdateCategory - Command")]
     [MemberData(
         nameof(DataGenerator.GetCategoriesToUpdate),
         parameters: 20,
         MemberType = typeof(DataGenerator)
     )]
     public async Task Command_ShouldUpdateCategory_WhenMethodHandleIsCalledWithOnlyNameAndDescription(
         DomainEntity.Category exampleCategory,
         UpdateCategoryInput exampleInput
     )
     {
         var input = new UpdateCategoryInput(
             exampleInput.Id,
             exampleInput.Name,
             exampleInput.Description!
         );

         var loggerMock = _fixture.GetLoggerMock();
         var repositoryMock = _fixture.GetRepositoryMock();
         var unitOfWorkMock = _fixture.GetUnitOfWorkMock();

         repositoryMock.Setup(x => x
             .GetById(exampleCategory.Id)
         ).ReturnsAsync(exampleCategory);
         
         unitOfWorkMock.Setup(uow => uow.Commit())
             .ReturnsAsync(true);

         var command = new UpdateCategory(
             unitOfWorkMock.Object,
             loggerMock.Object,
             repositoryMock.Object
         );

         var output = await command.Handle(input, CancellationToken.None);

         output.Should().NotBeNull();
         output.Name.Should().Be(input.Name);
         output.IsActive.Should().Be(exampleCategory.IsActive);
         output.Description.Should().Be(exampleCategory.Description);
         
         unitOfWorkMock.Verify(uow => uow.Commit(), Times.Once);
         repositoryMock.Verify(x => x.Update(exampleCategory), Times.Once);
         repositoryMock.Verify(x => x.GetById(exampleCategory.Id), Times.Once);
     }

     [Fact]
     [Trait("Application", "UpdateCategory - Command")]
     public async Task Command_ShouldThrowError_WhenCategoryNotFound()
     {
         var loggerMock = _fixture.GetLoggerMock();
         var repositoryMock = _fixture.GetRepositoryMock();
         var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
         var input = _fixture.DataGenerator.GetInputUpdate();

         var command = new UpdateCategory(unitOfWorkMock.Object, loggerMock.Object, repositoryMock.Object);

         var task = async () => await command.Handle(input, CancellationToken.None);

         await task.Should().ThrowAsync<NotFoundException>().WithMessage($"Category '{input.Id}' not found.");
         
         repositoryMock.Verify(x => x
                 .GetById(input.Id)
         , Times.Once);
     }

     [Theory]
     [Trait("Application", "UpdateCategory - Command")]
     [MemberData(
         nameof(DataGenerator.GetInvalidUpdateInputs),
         parameters: 20,
         MemberType = typeof(DataGenerator)
     )]
     public async Task Command_ShouldThrowError_WhenCantUpdateCategory(UpdateCategoryInput input)
     {
         var exampleCategory = _fixture.DataGenerator.GetValidCategory();
         input.Id = exampleCategory.Id;

         var loggerMock = _fixture.GetLoggerMock();
         var repositoryMock = _fixture.GetRepositoryMock();
         var unitOfWorkMock = _fixture.GetUnitOfWorkMock();

         repositoryMock.Setup(x => x.GetById(
             exampleCategory.Id)
         ).ReturnsAsync(exampleCategory);

         var command = new UpdateCategory(
             unitOfWorkMock.Object,
             loggerMock.Object,
             repositoryMock.Object
         );

         var task = async () => await command.Handle(input, CancellationToken.None);

         await task.Should().ThrowAsync<EntityValidationException>().WithMessage("Category is invalid");

         repositoryMock.Verify(x => x.GetById(exampleCategory.Id), Times.Once);
     }
}
