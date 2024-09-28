namespace Flixer.Catalog.Api.ApiModels.Category;

public class UpdateCategoryApiInput
{
    public string Name { get; set; }
    public bool? IsActive { get; set; }
    public string? Description { get; set; }

    public UpdateCategoryApiInput(
        string name, 
        bool? isActive = null,
        string? description = null
    ) 
    {
        Name = name;
        IsActive = isActive;
        Description = description;
    }
}