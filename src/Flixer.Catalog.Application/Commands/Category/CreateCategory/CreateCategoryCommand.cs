using MediatR;

namespace Flixer.Catalog.Application.Commands.Category.CreateCategory;

public class CreateCategoryCommand : IRequest<CategoryViewModel>
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public bool IsActive { get; private set; }
    
    public CreateCategoryCommand(string name, string description, bool isActive = true)
    {
        Name = name;
        IsActive = isActive;
        Description = description;
    }

    public void SetName(string name)
    {
        Name = name;
    }

    public void SetDescription(string description)
    {
        Description = description;
    }
}

public class CategoryViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }

    public CategoryViewModel(
        Guid id,
        string name,
        string description,
        bool isActive,
        DateTime createdAt
    )
    {
        Id = id;
        Name = name;
        Description = description;
        IsActive = isActive;
        CreatedAt = createdAt;
    }

    public static CategoryViewModel FromCategory(Domain.Entities.Category category)
        => new(
            category.Id,
            category.Name,
            category.Description,
            category.IsActive,
            category.CreatedAt
        );
}
