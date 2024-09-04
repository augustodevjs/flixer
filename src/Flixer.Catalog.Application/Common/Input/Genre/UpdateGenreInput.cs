using MediatR;
using Flixer.Catalog.Application.Common.Output;
using Flixer.Catalog.Application.Common.Output.Genre;

namespace Flixer.Catalog.Application.Common.Input.Genre;

public class UpdateGenreInput : IRequest<GenreOutput>
{
    public Guid Id { get; private set; }
    public string? Name { get; private set; }
    public bool? IsActive { get; private set; }
    public List<Guid>? CategoriesIds { get; private set; }

    public UpdateGenreInput(
        Guid id, 
        string? name, 
        bool? isActive, 
        List<Guid>? categoriesIds
    )
    {
        Id = id;
        Name = name;
        IsActive = isActive;
        CategoriesIds = categoriesIds;
    }
}