using MediatR;
using Flixer.Catalog.Application.Common.Output;
using Flixer.Catalog.Application.Common.Output.Category;

namespace Flixer.Catalog.Application.Common.Input.Category;

public class UpdateCategoryInput : IRequest<CategoryOutput>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public bool? IsActive { get; set; }
    public string? Description { get; set; }

    public UpdateCategoryInput(
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