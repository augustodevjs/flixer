using MediatR;
using Flixer.Catalog.Application.Dtos.ViewModel.Category;

namespace Flixer.Catalog.Application.Dtos.InputModel.Category;

public class UpdateCategoryInputModel : IRequest<CategoryViewModel>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public bool? IsActive { get; set; }
    public string? Description { get; set; }

    public UpdateCategoryInputModel(
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
