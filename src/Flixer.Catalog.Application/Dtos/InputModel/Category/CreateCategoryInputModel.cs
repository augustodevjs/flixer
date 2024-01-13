using MediatR;
using Flixer.Catalog.Application.Dtos.ViewModel.Category;

namespace Flixer.Catalog.Application.Dtos.InputModel.Category;

public class CreateCategoryInputModel : IRequest<CategoryViewModel>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }

    public CreateCategoryInputModel(string name, string? description = null, bool isActive = true)
    {
        Name = name;
        IsActive = isActive;
        Description = description ?? "";
    }
}
