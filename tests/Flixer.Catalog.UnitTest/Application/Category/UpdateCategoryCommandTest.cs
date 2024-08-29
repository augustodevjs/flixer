using Moq;
using Xunit;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Flixer.Catalog.Domain.Exceptions;
using Flixer.Catalog.Application.Exceptions;
using DomainEntity = Flixer.Catalog.Domain.Entities;
using Flixer.Catalog.Application.Commands.Category.UpdateCategory;
using Flixer.Catalog.UnitTest.Fixture.Application.Category.UpdateCategory;
using Flixer.Catalog.UnitTest.Helpers;

namespace Flixer.Catalog.UnitTest.Application.Category;

[Collection(nameof(UpdateCategoryCommandFixture))]
public class UpdateCategoryCommandTest
{
    private readonly UpdateCategoryCommandFixture _fixture;

    public UpdateCategoryCommandTest(UpdateCategoryCommandFixture fixture)
        => _fixture = fixture;

     [Theory]
     [Trait("Application", "UpdateCategory - Command")]
     [MemberData(
         nameof(DataGenerator.GetCategoriesToUpdate),
         parameters: 10,
         MemberType = typeof(DataGenerator)
     )]
     public async Task Command_ShouldUpdateCategory_WhenMethodHandleIsCalled(
         DomainEntity.Category exampleCategory,
         UpdateCategoryCommand input
     )
     {
         var loggerMock = _fixture.GetLoggerMock();
         var repositoryMock = _fixture.GetRepositoryMock();

         repositoryMock.Setup(x => x.GetById(
             exampleCategory.Id)
         ).ReturnsAsync(exampleCategory);
         
         repositoryMock.Setup(x => x.UnityOfWork.Commit())
             .ReturnsAsync(true);

         var command = new UpdateCategoryCommandHandler(repositoryMock.Object, loggerMock.Object);
         
         var output = await command.Handle(input, CancellationToken.None);

         output.Should().NotBeNull();
         output.Name.Should().Be(input.Name);
         output.Description.Should().Be(input.Description);
         output.IsActive.Should().Be((bool)input.IsActive!);
         
         repositoryMock.Verify(x => x.UnityOfWork.Commit(), Times.Once);
         repositoryMock.Verify(x => x.Update(exampleCategory), Times.Once);
         repositoryMock.Verify(x => x.GetById(exampleCategory.Id), Times.Once);
     }
     
     [Theory]
     [Trait("Application", "UpdateCategory - Command")]
     [MemberData(
         nameof(DataGenerator.GetCategoriesToUpdate),
         parameters: 10,
         MemberType = typeof(DataGenerator)
     )]
     public async Task Command_ShouldUpdateCategory_WhenMethodHandleIsCalledWithoutProvidingIsActive(
         DomainEntity.Category exampleCategory,
         UpdateCategoryCommand exampleInput
     )
     {
         var input = new UpdateCategoryCommand(
             exampleInput.Id,
             exampleInput.Name,
             exampleInput.Description!
         );

         var loggerMock = _fixture.GetLoggerMock();
         var repositoryMock = _fixture.GetRepositoryMock();

         repositoryMock.Setup(x => x
             .GetById(exampleCategory.Id)
         ).ReturnsAsync(exampleCategory);
         
         repositoryMock.Setup(x => x.UnityOfWork.Commit())
             .ReturnsAsync(true);

         var command = new UpdateCategoryCommandHandler(
             repositoryMock.Object,
             loggerMock.Object
         );

         var output = await command.Handle(input, CancellationToken.None);

         output.Should().NotBeNull();
         output.Name.Should().Be(input.Name);
         output.Description.Should().Be(input.Description);
         output.IsActive.Should().Be(exampleCategory.IsActive);
         repositoryMock.Verify(x => x.UnityOfWork.Commit(), Times.Once);
         repositoryMock.Verify(x => x.Update(exampleCategory), Times.Once);
         repositoryMock.Verify(x => x.GetById(exampleCategory.Id), Times.Once);
     }
     
     [Theory]
     [Trait("Application", "UpdateCategory - Command")]
     [MemberData(
         nameof(DataGenerator.GetCategoriesToUpdate),
         parameters: 10,
         MemberType = typeof(DataGenerator)
     )]
     public async Task Command_ShouldUpdateCategory_WhenMethodHandleIsCalledWithOnlyNameAndDescription(
         DomainEntity.Category exampleCategory,
         UpdateCategoryCommand exampleInput
     )
     {
         var input = new UpdateCategoryCommand(
             exampleInput.Id,
             exampleInput.Name,
             exampleInput.Description!
         );

         var loggerMock = _fixture.GetLoggerMock();
         var repositoryMock = _fixture.GetRepositoryMock();

         repositoryMock.Setup(x => x
             .GetById(exampleCategory.Id)
         ).ReturnsAsync(exampleCategory);
         
         repositoryMock.Setup(x => x.UnityOfWork.Commit())
             .ReturnsAsync(true);

         var command = new UpdateCategoryCommandHandler(
             repositoryMock.Object,
             loggerMock.Object
         );

         var output = await command.Handle(input, CancellationToken.None);

         output.Should().NotBeNull();
         output.Name.Should().Be(input.Name);
         output.IsActive.Should().Be(exampleCategory.IsActive);
         output.Description.Should().Be(exampleCategory.Description);
         
         repositoryMock.Verify(x => x.UnityOfWork.Commit(), Times.Once);
         repositoryMock.Verify(x => x.Update(exampleCategory), Times.Once);
         repositoryMock.Verify(x => x.GetById(exampleCategory.Id), Times.Once);
     }

     [Fact]
     [Trait("Application", "UpdateCategory - Command")]
     public async Task Command_ShouldThrowError_WhenCategoryNotFound()
     {
         var loggerMock = _fixture.GetLoggerMock();
         var repositoryMock = _fixture.GetRepositoryMock();
         var input = _fixture.GetInputUpdate();

         var command = new UpdateCategoryCommandHandler(repositoryMock.Object, loggerMock.Object);

         var task = async () => await command.Handle(input, CancellationToken.None);

         await task.Should().ThrowAsync<NotFoundException>().WithMessage($"Category '{input.Id}' not found.");
         
         loggerMock.VerifyLog(LogLevel.Warning, Times.Exactly(1));
         
         repositoryMock.Verify(x => x
                 .GetById(input.Id)
         , Times.Once);
     }

     [Theory]
     [Trait("Application", "UpdateCategory - Command")]
     [MemberData(
         nameof(DataGenerator.GetInvalidUpdateInputs),
         parameters: 12,
         MemberType = typeof(DataGenerator)
     )]
     public async Task Command_ShouldThrowError_WhenCantUpdateCategory(UpdateCategoryCommand input)
     {
         var exampleCategory = _fixture.CategoryFixture.GetValidCategory();
         input.Id = exampleCategory.Id;

         var repositoryMock = _fixture.GetRepositoryMock();
         var loggerMock = _fixture.GetLoggerMock();

         repositoryMock.Setup(x => x.GetById(
             exampleCategory.Id)
         ).ReturnsAsync(exampleCategory);

         var command = new UpdateCategoryCommandHandler(
             repositoryMock.Object,
             loggerMock.Object
         );

         var task = async () => await command.Handle(input, CancellationToken.None);

         await task.Should().ThrowAsync<EntityValidationException>().WithMessage("Category is invalid");

         repositoryMock.Verify(x => x.GetById(exampleCategory.Id), Times.Once);
     }
}
