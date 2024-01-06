using MediatR;

namespace Flixer.Catalog.Application.UseCases.Category.CreateCategory;
public class CreateCategoryInput : IRequest<CreateCategoryOutput>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }

    public CreateCategoryInput(string name, string? description = null, bool isActive = true)
    {
        Name = name;
        IsActive = isActive;
        Description = description ?? "";
    }
}
