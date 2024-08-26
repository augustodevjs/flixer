using Microsoft.Extensions.Logging;
using Flixer.Catalog.UnitTest.Common;
using Flixer.Catalog.Domain.Contracts.Repository;
using Flixer.Catalog.UnitTest.Domain.Fixtures.Category;
using Flixer.Catalog.Application.Commands.Category.CreateCategory;

namespace Flixer.Catalog.UnitTest.Application.Fixtures.Category;

[CollectionDefinition(nameof(CreateCategoryCommandFixtureCollection))]
public class CreateCategoryCommandFixtureCollection : ICollectionFixture<CreateCategoryCommandFixture>
{
    
}

public class CreateCategoryCommandFixture : BaseFixture
{
    public CategoryFixture CategoryFixture { get; set; } = new();
    
    public Mock<ICategoryRepository> GetRepositoryMock() => new();
    public Mock<ILogger<CreateCategoryCommandHandler>> GetLoggerMock() => new();
    
    public CreateCategoryCommand GetInputCreate()
     {
         return new(
             CategoryFixture.GetValidCategoryName(), 
             CategoryFixture.GetValidCategoryDescription(), 
             CategoryFixture.GetRandomBoolean()
         );
     }
}