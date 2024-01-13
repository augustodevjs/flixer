using MediatR;
using Flixer.Catalog.Application.Dtos.ViewModel.Genre;

namespace Flixer.Catalog.Application.Dtos.InputModel.Genre;

public class UpdateGenreInputModel : IRequest<GenreViewModel>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public bool? IsActive { get; set; }
    public List<Guid>? CategoriesIds { get; set; }

    public UpdateGenreInputModel(
        Guid id,
        string name,
        bool? isActive = null,
        List<Guid>? categoriesIds = null
    )
    {
        Id = id;
        Name = name;
        IsActive = isActive;
        CategoriesIds = categoriesIds;
    }
}