using MediatR;
using Flixer.Catalog.Application.Dtos.ViewModel.Genre;

namespace Flixer.Catalog.Application.Dtos.InputModel.Genre;

public class CreateGenreInputModel : IRequest<GenreViewModel>
{
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public List<Guid>? CategoriesIds { get; set; }

    public CreateGenreInputModel(
        string name,
        bool isActive,
        List<Guid>? categoriesIds = null
    )
    {
        Name = name;
        IsActive = isActive;
        CategoriesIds = categoriesIds;
    }
}
