using DomainEntity = Flixer.Catalog.Domain.Entities;

namespace Flixer.Catalog.Application.Dtos.ViewModel.Genre;

public class GenreViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public IReadOnlyList<Guid> Categories { get; set; }

    public GenreViewModel(
        Guid id,
        string name,
        bool isActive,
        DateTime createdAt,
        IReadOnlyList<Guid> categories
    )
    {
        Id = id;
        Name = name;
        IsActive = isActive;
        CreatedAt = createdAt;
        Categories = categories;
    }

    public static GenreViewModel FromGenre(DomainEntity.Genre genre)
        => new(
            genre.Id,
            genre.Name,
            genre.IsActive,
            genre.CreatedAt,
            genre.Categories
        );
}