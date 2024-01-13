using DomainEntity = Flixer.Catalog.Domain.Entities;
using Flixer.Catalog.Domain.SeedWork.SearchableRepository;
using Flixer.Catalog.Application.Dtos.Common;

namespace Flixer.Catalog.Application.Dtos.ViewModel.Genre;

public class ListGenresViewModel : PaginatedListOutput<GenreViewModel>
{
    public ListGenresViewModel(
        int page,
        int perPage,
        int total,
        IReadOnlyList<GenreViewModel> items
    )
        : base(page, perPage, total, items)
    { }

    public static ListGenresViewModel FromSearchOutput(SearchOutput<DomainEntity.Genre> searchOutput)
        => new(
            searchOutput.CurrentPage,
            searchOutput.PerPage,
            searchOutput.Total,
            searchOutput.Items
                .Select(GenreViewModel.FromGenre)
                .ToList()
        );
}