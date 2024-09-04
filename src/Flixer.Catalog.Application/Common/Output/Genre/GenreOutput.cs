namespace Flixer.Catalog.Application.Common.Output.Genre;

public class GenreOutput
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public IReadOnlyList<GenreOutputCategory> Categories { get; private set; }
    public GenreOutput(
        Guid id, 
        string? name, 
        bool isActive, 
        DateTime createdAt, 
        IReadOnlyList<GenreOutputCategory> categories
    )
    {
        Id = id;
        Name = name;
        IsActive = isActive;
        CreatedAt = createdAt;
        Categories = categories;
    }
    
    public static GenreOutput FromGenre(Domain.Entities.Genre genre)
        => new(
            genre.Id,
            genre.Name,
            genre.IsActive,
            genre.CreatedAt,
            genre.Categories
                .Select(categoryId => new GenreOutputCategory(categoryId))
                .ToList()
                .AsReadOnly()
        );
}