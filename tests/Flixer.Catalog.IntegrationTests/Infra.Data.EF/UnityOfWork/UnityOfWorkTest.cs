//using Flixer.Catalog.Infra.Data.EF;
//using Microsoft.EntityFrameworkCore;

//namespace Flixer.Catalog.IntegrationTests.Infra.Data.EF.UnityOfWork;

//[Collection(nameof(UnitOfWorkTestFixture))]
//public class UnityOfWorkTest
//{
//    private readonly UnitOfWorkTestFixture _fixture;

//    public UnityOfWorkTest(UnitOfWorkTestFixture fixture)
//    {
//        _fixture = fixture;
//    }

//    [Fact(DisplayName = nameof(Commit))]
//    [Trait("Integration/Infra.Data", "UnitOfWork - Persistence")]
//    public async Task Commit()
//    {
//        var dbContext = _fixture.CreateDbContext();
//        var exampleCategory = _fixture.GetExampleCategory();

//        await dbContext.AddAsync(exampleCategory);

//        var unitOfWork = new Catalog.Infra.Data.EF.UnityOfWork(dbContext);

//        await unitOfWork.Commit(CancellationToken.None);

//        var assertDbContext = _fixture.CreateDbContext(true);
//        var savedCategory = await assertDbContext.Categories.FirstOrDefaultAsync(c => c.Id == exampleCategory.Id);

//        savedCategory.Should().NotBeNull();
//        savedCategory.Should().BeEquivalentTo(exampleCategory);
//    }


//    [Fact(DisplayName = nameof(Rollback))]
//    [Trait("Integration/Infra.Data", "UnitOfWork - Persistence")]
//    public async Task Rollback()
//    {
//        var dbContext = _fixture.CreateDbContext();
//        var unitOfWork = new Catalog.Infra.Data.EF.UnityOfWork(dbContext);

//        var task = async () => await unitOfWork.RollBack(CancellationToken.None);

//        await task.Should().NotThrowAsync();
//    }
//}