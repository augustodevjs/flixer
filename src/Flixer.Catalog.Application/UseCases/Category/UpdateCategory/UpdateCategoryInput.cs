using MediatR;
using Flixer.Catalog.Application.UseCases.Category.Common;

namespace Flixer.Catalog.Application.UseCases.Category.UpdateCategory;

public class UpdateCategoryInput : IRequest<CategoryModelOutput>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public bool? IsActive { get; set; }
    public string? Description { get; set; }

    public UpdateCategoryInput(
        Guid id, 
        string name, 
        bool? isActive = null, 
        string? description = null
    )
    {
        Id = id;
        Name = name;
        IsActive = isActive;
        Description = description;
    }
}
