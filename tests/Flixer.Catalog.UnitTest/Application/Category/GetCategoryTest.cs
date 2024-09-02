using Moq;
using Xunit;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Flixer.Catalog.Tests.Shared.Helpers;
using Flixer.Catalog.Application.Exceptions;
using Flixer.Catalog.Application.Queries.Category;
using Flixer.Catalog.Application.Common.Output.Category;
using Flixer.Catalog.UnitTest.Fixture.Application.Category.GetCategory;

namespace Flixer.Catalog.UnitTest.Application.Category;

[Collection(nameof(GetCategoryFixture))]
public class GetCategoryTest
{
    private readonly GetCategoryFixture _fixture;

    public GetCategoryTest(GetCategoryFixture fixture) => 
        _fixture = fixture;

    [Fact]
    [Trait("Application", "GetCategory - Query")]
    public async Task Query_ShouldGetCategory_WhenMethodHandleIsCalled()
    {
        var loggerMock = _fixture.GetLoggerMock();
        var repositoryMock = _fixture.GetRepositoryMock();
        var exampleCategory = _fixture.DataGenerator.GetValidCategory();

        repositoryMock.Setup(x => x.GetById(
            It.IsAny<Guid>()
        )).ReturnsAsync(exampleCategory);

        var input = new GetCategoryInput(exampleCategory.Id);
        
        var query = new GetCategory(loggerMock.Object, repositoryMock.Object);

        var output = await query.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Id.Should().Be(exampleCategory.Id);
        output.Name.Should().Be(exampleCategory.Name);
        output.IsActive.Should().Be(exampleCategory.IsActive);
        output.CreatedAt.Should().Be(exampleCategory.CreatedAt);
        output.Description.Should().Be(exampleCategory.Description);

        loggerMock.VerifyLog(LogLevel.Information, Times.Exactly(1));
        repositoryMock.Verify(x => x.GetById(
            It.IsAny<Guid>()
        ), Times.Once);
    }
    
    [Fact]
    [Trait("Application", "GetCategory - Query")]
    public async Task Query_ShouldThrowException_WhenCategoryDoesntExist()
    {
        
        var exampleGuid = Guid.NewGuid();
        var loggerMock = _fixture.GetLoggerMock();
        var repositoryMock = _fixture.GetRepositoryMock();

        var input = new GetCategoryInput(exampleGuid);
        
        var query = new GetCategory(loggerMock.Object, repositoryMock.Object);

        var task = async () => await query.Handle(input, CancellationToken.None);
    
        await task.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"Category '{exampleGuid}' not found.");
        
        loggerMock.VerifyLog(LogLevel.Information, Times.Exactly(0));
        
        repositoryMock.Verify(x => x.GetById(
            It.IsAny<Guid>()
        ), Times.Once);
    }
}
