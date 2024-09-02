using Flixer.Catalog.Domain.Entities;

namespace Flixer.Catalog.Infra.Data.EF.Models;

public class GenresCategories
{
    public Guid GenreId { get; set; }
    public Guid CategoryId { get; set; }
    
    public Genre? Genre { get; set; }
    public Category? Category { get; set; }

    public GenresCategories(Guid categoryId, Guid genreId)
    {
        GenreId = genreId;
        CategoryId = categoryId;
    }
}