using Microsoft.EntityFrameworkCore;
using InfraData = Flixer.Catalog.Infra.Data.EF;

namespace Flixer.Catalog.IntegrationTests.Infra.Data.EF.UnityOfWork;

[Collection(nameof(UnitOfWorkTestFixture))]
public class UnityOfWorkTest
{
    private readonly UnitOfWorkTestFixture _fixture;

    public UnityOfWorkTest(UnitOfWorkTestFixture fixture)
    {
        _fixture = fixture;
    }
    [Fact(DisplayName = nameof(Commit))]
    [Trait("Integration/Infra.Data", "UnitOfWork - Persistence")]
    public async Task Commit()
    {
        var dbContext = _fixture.CreateDbContext();
        var examplecategoriesList = _fixture.GetExampleCategoriesList();

        await dbContext.AddRangeAsync(examplecategoriesList);
        var unitOfWork = new InfraData.UnityOfWork(dbContext);

        await unitOfWork.Commit(CancellationToken.None);

        var assertDbContext = _fixture.CreateDbContext(true);

        var savedCategories = assertDbContext.Categories.AsNoTracking().ToList();
        savedCategories.Should().HaveCount(examplecategoriesList.Count);
    }


    [Fact(DisplayName = nameof(Rollback))]
    [Trait("Integration/Infra.Data", "UnitOfWork - Persistence")]
    public async Task Rollback()
    {
        var dbContext = _fixture.CreateDbContext();
        var unitOfWork = new InfraData.UnityOfWork(dbContext);

        var task = async ()
            => await unitOfWork.Rollback(CancellationToken.None);

        await task.Should().NotThrowAsync();
    }
}