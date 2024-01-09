using Flixer.Catalog.Application.Common;
using DomainEntity = Flixer.Catalog.Domain.Entities;
using Flixer.Catalog.Application.UseCases.Genre.Common;
using Flixer.Catalog.Domain.SeedWork.SearchableRepository;

namespace Flixer.Catalog.Application.UseCases.Genre.ListGenres;

public class ListGenresOutput
    : PaginatedListOutput<GenreModelOutput>
{
    public ListGenresOutput(
        int page,
        int perPage,
        int total,
        IReadOnlyList<GenreModelOutput> items
    )
        : base(page, perPage, total, items)
    { }

    public static ListGenresOutput FromSearchOutput(SearchOutput<DomainEntity.Genre> searchOutput) 
        => new(
            searchOutput.CurrentPage,
            searchOutput.PerPage,
            searchOutput.Total,
            searchOutput.Items
                .Select(GenreModelOutput.FromGenre)
                .ToList()
        );
}