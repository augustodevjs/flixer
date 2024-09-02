using MediatR;
using Flixer.Catalog.Application.Common.Output;
using Flixer.Catalog.Application.Common.Output.Genre;

namespace Flixer.Catalog.Application.Common.Input.Genre;

public class CreateGenreInput : IRequest<GenreOutput>
{
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public List<Guid>? CategoriesIds { get; set; }

    public CreateGenreInput(
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