//using Flixer.Catalog.Infra.Data.EF.Repositories;
//using Flixer.Catalog.Application.Dtos.InputModel.Category;
//using UseCase = Flixer.Catalog.Application.UseCases.Category;
//using Flixer.Catalog.Application.Exceptions;

//namespace Flixer.Catalog.IntegrationTests.Application.UseCases.Category.GetCategory;

//[Collection(nameof(GetCategoryTestFixture))]
//public class GetCategoryUseCaseTest
//{
//    private readonly GetCategoryTestFixture _fixture;

//    public GetCategoryUseCaseTest(GetCategoryTestFixture fixture)
//    {
//        _fixture = fixture;
//    }

//    [Fact(DisplayName = nameof(GetCategory))]
//    [Trait("Integration/Application", "GetCategory - Use Cases")]
//    public async Task GetCategory()
//    {
//        var dbContext = _fixture.CreateDbContext();
//        var exampleCategory = _fixture.GetExampleCategory();

//        dbContext.Categories.Add(exampleCategory);
//        dbContext.SaveChanges();

//        var repository = new CategoryRepository(dbContext);

//        var input = new GetCategoryInputModel(exampleCategory.Id);
//        var useCase = new UseCase.GetCategory(repository);

//        var output = await useCase.Handle(input, CancellationToken.None);

//        output.Should().NotBeNull();
//        output.Id.Should().Be(exampleCategory.Id);
//        output.Name.Should().Be(exampleCategory.Name);
//        output.IsActive.Should().Be(exampleCategory.IsActive);
//        output.CreatedAt.Should().Be(exampleCategory.CreatedAt);
//        output.Description.Should().Be(exampleCategory.Description);
//    }

//    [Fact(DisplayName = nameof(NotFoundExceptionWhenCategoryDoesntExist))]
//    [Trait("Integration/Application", "GetCategory - Use Cases")]
//    public async Task NotFoundExceptionWhenCategoryDoesntExist()
//    {
//        var dbContext = _fixture.CreateDbContext();
//        var exampleCategory = _fixture.GetExampleCategory();

//        dbContext.Categories.Add(exampleCategory);
//        dbContext.SaveChanges();

//        var repository = new CategoryRepository(dbContext);
//        var input = new GetCategoryInputModel(Guid.NewGuid());

//        var useCase = new UseCase.GetCategory(repository);

//        var task = async () => await useCase.Handle(input, CancellationToken.None);

//        await task.Should().ThrowAsync<NotFoundException>().WithMessage($"Category '{input.Id}' not found.");
//    }
//}