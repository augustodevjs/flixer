using MediatR;
using Flixer.Catalog.Application.Commands.Category.CreateCategory;

namespace Flixer.Catalog.Application.Commands.Category.UpdateCategory;

public class UpdateCategoryCommand : IRequest<CategoryViewModel>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public bool? IsActive { get; set; }
    public string? Description { get; set; }

    public UpdateCategoryCommand(
        Guid id,
        string name,
        string description,
        bool? isActive = null
    )
    {
        Id = id;
        Name = name;
        IsActive = isActive;
        Description = description;
    }
}