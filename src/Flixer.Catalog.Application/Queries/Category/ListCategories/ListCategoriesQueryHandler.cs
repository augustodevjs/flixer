using MediatR;
using Microsoft.Extensions.Logging;
using Flixer.Catalog.Domain.Contracts.Repository;

namespace Flixer.Catalog.Application.Queries.Category.ListCategories;

public class ListCategoriesQueryHandler : IRequestHandler<ListCategoriesQuery, ListCategoriesOutput>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ILogger<ListCategoriesQueryHandler> _logger;

    public ListCategoriesQueryHandler(
        ICategoryRepository categoryRepository, 
        ILogger<ListCategoriesQueryHandler> logger
    )
    {
        _logger = logger;
        _categoryRepository = categoryRepository;
    }

    public async Task<ListCategoriesOutput> Handle(ListCategoriesQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling ListCategoriesQuery for page {Page} with {PerPage} items per page.", request.Page, request.PerPage);

        var searchInput = request.ToSearchInput();
        _logger.LogDebug("SearchInput created: {@SearchInput}", searchInput);

        var searchOutput = await _categoryRepository.Search(searchInput);
        _logger.LogInformation("Search completed successfully with {TotalItems} total items.", searchOutput.Total);

        return ListCategoriesOutput.FromSearchOutput(searchOutput);
    }
}