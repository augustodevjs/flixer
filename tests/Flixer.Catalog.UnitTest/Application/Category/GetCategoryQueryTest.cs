using Moq;
using Xunit;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Flixer.Catalog.UnitTest.Helpers;
using Flixer.Catalog.Application.Exceptions;
using Flixer.Catalog.Application.Queries.Category;
using Flixer.Catalog.Application.Queries.Category.GetCategory;
using Flixer.Catalog.UnitTest.Application.Fixtures.Category.GetCategory;

namespace Flixer.Catalog.UnitTest.Application.Category;

[Collection(nameof(GetCategoryQueryFixture))]
public class GetCategoryQueryTest
{
    private readonly GetCategoryQueryFixture _fixture;

    public GetCategoryQueryTest(GetCategoryQueryFixture fixture) => 
        _fixture = fixture;

    [Fact]
    [Trait("Application", "GetCategory - Query")]
    public async Task Query_ShouldGetCategory_WhenMethodHandleIsCalled()
    {
        var loggerMock = _fixture.GetLoggerMock();
        var repositoryMock = _fixture.GetRepositoryMock();
        var exampleCategory = _fixture.CategoryFixture.GetValidCategory();

        repositoryMock.Setup(x => x.GetById(
            It.IsAny<Guid>()
        )).ReturnsAsync(exampleCategory);

        var input = new GetCategoryQuery(exampleCategory.Id);
        
        var query = new GetCategoryQueryHandler(repositoryMock.Object, loggerMock.Object);

        var output = await query.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Id.Should().Be(exampleCategory.Id);
        output.Name.Should().Be(exampleCategory.Name);
        output.IsActive.Should().Be(exampleCategory.IsActive);
        output.CreatedAt.Should().Be(exampleCategory.CreatedAt);
        output.Description.Should().Be(exampleCategory.Description);

        loggerMock.VerifyLog(LogLevel.Information, Times.Exactly(2));
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

        var input = new GetCategoryQuery(exampleGuid);
        
        var query = new GetCategoryQueryHandler(repositoryMock.Object, loggerMock.Object);

        var task = async () => await query.Handle(input, CancellationToken.None);
    
        await task.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"Category '{exampleGuid}' not found.");
        
        loggerMock.VerifyLog(LogLevel.Warning, Times.Exactly(1));
        loggerMock.VerifyLog(LogLevel.Information, Times.Exactly(1));
        
        repositoryMock.Verify(x => x.GetById(
            It.IsAny<Guid>()
        ), Times.Once);
    }
}
