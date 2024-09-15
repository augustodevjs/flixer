using Moq;
using Xunit;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Flixer.Catalog.Tests.Shared.Helpers;
using Flixer.Catalog.Application.Exceptions;
using Flixer.Catalog.Application.Commands.Category;
using Flixer.Catalog.Application.Common.Input.Category;
using Flixer.Catalog.UnitTest.Fixture.Application.Category.DeleteCategory;

namespace Flixer.Catalog.UnitTest.Application.Category;

[Collection(nameof(DeleteCategoryFixture))]
public class DeleteCategoryTest
{
    private readonly DeleteCategoryFixture _fixture;

    public DeleteCategoryTest(DeleteCategoryFixture fixture) =>
        _fixture = fixture;

     [Fact]
     [Trait("Application", "DeleteCategory - Command")]
     public async Task Command_ShouldDeleteCategory_WhenMethodHandleIsCalled()
     {
         var loggerMock = _fixture.GetLoggerMock();
         var repositoryMock = _fixture.GetRepositoryMock();
         var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
         var categoryExample = _fixture.DataGenerator.GetValidCategory();
         
         repositoryMock.Setup(x => x
                 .GetById(categoryExample.Id))
        .ReturnsAsync(categoryExample);
         
         unitOfWorkMock.Setup(uow => uow.Commit())
             .ReturnsAsync(true);

         var input = new DeleteCategoryInput(categoryExample.Id);

         var command = new DeleteCategory(unitOfWorkMock.Object, loggerMock.Object, repositoryMock.Object);

         await command.Handle(input, CancellationToken.None);

         loggerMock.VerifyLog(LogLevel.Information, Times.Exactly(1));
         
         unitOfWorkMock.Verify(uow => uow.Commit(), Times.Once);
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
         var unitOfWorkMock = _fixture.GetUnitOfWorkMock();

         var input = new DeleteCategoryInput(exampleGuid);

         var command = new DeleteCategory(unitOfWorkMock.Object, loggerMock.Object, repositoryMock.Object);

         var task = async () => await command.Handle(input, CancellationToken.None);

         await task.Should().ThrowAsync<NotFoundException>()
             .WithMessage($"Category '{exampleGuid}' not found.");

         loggerMock.VerifyLog(LogLevel.Information, Times.Exactly(0));
         
         unitOfWorkMock.Verify(uow => uow.Commit(), Times.Never);
         repositoryMock.Verify(x => x.GetById(exampleGuid), Times.Once);
         repositoryMock.Verify(x => x.Delete(It.IsAny<Catalog.Domain.Entities.Category>()), Times.Never);
     }
}