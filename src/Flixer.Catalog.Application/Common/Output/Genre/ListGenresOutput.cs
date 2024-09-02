using Flixer.Catalog.Application.Common.Output.Common;
using Flixer.Catalog.Domain.SeedWork.SearchableRepository;

namespace Flixer.Catalog.Application.Common.Output.Genre;

public class ListGenresOutput : PaginatedListOutput<GenreOutput>
{
    public ListGenresOutput(
        int page, 
        int total, 
        int perPage, 
        IReadOnlyList<GenreOutput> items
    ) : base(page, total, perPage, items)
    {
    }
    
    public static ListGenresOutput FromSearchOutput(
        SearchOutput<Domain.Entities.Genre> searchOutput
    ) => new(
        searchOutput.CurrentPage,
        searchOutput.Total,
        searchOutput.PerPage,
        searchOutput.Items
            .Select(GenreOutput.FromGenre)
            .ToList()
    );
    
    public void FillWithCategoryNames(IReadOnlyList<Domain.Entities.Category> listCategories)
    {
        foreach (var categoryOutput in from item in Items
                 from categoryOutput in item.Categories
                 select categoryOutput)
        {
            categoryOutput.Name = listCategories?
                .FirstOrDefault(category => category.Id == categoryOutput.Id)?
                .Name;
        }
    }
}