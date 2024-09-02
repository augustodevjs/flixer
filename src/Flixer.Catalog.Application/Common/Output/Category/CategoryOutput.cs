namespace Flixer.Catalog.Application.Common.Output.Category;

public class CategoryOutput
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }

    public CategoryOutput(
        Guid id,
        string name,
        string description,
        bool isActive,
        DateTime createdAt
    )
    {
        Id = id;
        Name = name;
        IsActive = isActive;
        CreatedAt = createdAt;
        Description = description;
    }

    public static CategoryOutput FromCategory(Domain.Entities.Category category)
        => new(
            category.Id,
            category.Name,
            category.Description,
            category.IsActive,
            category.CreatedAt
        );
}