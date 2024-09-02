namespace Flixer.Catalog.Application.Common.Output.Genre;

public class GenreOutputCategory
{
    public Guid Id { get; set; }
    public string? Name { get; set; }

    public GenreOutputCategory(Guid id, string? name = null)
    {
        Id = id;
        Name = name;
    }
}