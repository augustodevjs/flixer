using Moq;
using Xunit;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Flixer.Catalog.UnitTest.Helpers;
using Flixer.Catalog.Application.Exceptions;
using Flixer.Catalog.Application.Commands.Category.DeleteCategory;
using Flixer.Catalog.UnitTest.Application.Fixtures.Category.DeleteCategory;

namespace Flixer.Catalog.UnitTest.Application.Category;

[Collection(nameof(DeleteCategoryCommandFixture))]
public class DeleteCategoryCommandTest
{
    private readonly DeleteCategoryCommandFixture _fixture;

    public DeleteCategoryCommandTest(DeleteCategoryCommandFixture fixture) =>
        _fixture = fixture;

     [Fact]
     [Trait("Application", "DeleteCategory - Command")]
     public async Task Command_ShouldDeleteCategory_WhenMethodHandleIsCalled()
     {
         var loggerMock = _fixture.GetLoggerMock();
         var repositoryMock = _fixture.GetRepositoryMock();
         var categoryExample = _fixture.CategoryFixture.GetValidCategory();
         
         repositoryMock.Setup(x => x
                 .GetById(categoryExample.Id))
        .ReturnsAsync(categoryExample);
         
         repositoryMock.Setup(x => x
                 .UnityOfWork.Commit())
             .ReturnsAsync(true);

         var input = new DeleteCategoryCommand(categoryExample.Id);

         var command = new DeleteCategoryCommandHandler(repositoryMock.Object, loggerMock.Object);

         await command.Handle(input, CancellationToken.None);

         loggerMock.VerifyLog(LogLevel.Information, Times.Exactly(3));
         repositoryMock.Verify(x => x.UnityOfWork.Commit(), Times.Once);
         repositoryMock.Verify(x => x.Delete(categoryExample), Times.Once);
         repositoryMock.Verify(x => x.GetById(categoryExample.Id), Times.Once);
     }

     [Fact]
     [Trait("Application", "DeleteCategory - Command")]
     public async Task Command_ShouldThrowError_WhenCategoryNotFound()
     {
         var exampleGuid = Guid.NewGuid();
         var loggerMock = _fixture.GetLoggerMock();
         var repositoryMock = _fixture.GetRepositoryMock();

         var input = new DeleteCategoryCommand(exampleGuid);

         var command = new DeleteCategoryCommandHandler(repositoryMock.Object, loggerMock.Object);

         var task = async () => await command.Handle(input, CancellationToken.None);

         await task.Should().ThrowAsync<NotFoundException>()
             .WithMessage($"Category '{exampleGuid}' not found.");

         loggerMock.VerifyLog(LogLevel.Warning, Times.Exactly(1));
         loggerMock.VerifyLog(LogLevel.Information, Times.Exactly(1));
         repositoryMock.Verify(x => x.GetById(exampleGuid), Times.Once);
         repositoryMock.Verify(x => x.UnityOfWork.Commit(), Times.Never);
         repositoryMock.Verify(x => x.Delete(It.IsAny<Catalog.Domain.Entities.Category>()), Times.Never);
     }
}