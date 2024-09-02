using MediatR;
using Microsoft.Extensions.Logging;
using Flixer.Catalog.Domain.Contracts.Repository;
using Flixer.Catalog.Application.Common.Input.Category;
using Flixer.Catalog.Application.Common.Output.Category;

namespace Flixer.Catalog.Application.Queries.Category;

public class ListCategories : IRequestHandler<ListCategoriesInput, ListCategoriesOutput>
{
    private readonly ILogger<ListCategories> _logger;
    private readonly ICategoryRepository _categoryRepository;

    public ListCategories(
        ILogger<ListCategories> logger,
        ICategoryRepository categoryRepository 
    )
    {
        _logger = logger;
        _categoryRepository = categoryRepository;
    }

    public async Task<ListCategoriesOutput> Handle(ListCategoriesInput request, CancellationToken cancellationToken)
    {
        var searchInput = request.ToSearchInput();
        var searchOutput = await _categoryRepository.Search(searchInput);
        
        _logger.LogInformation("Search completed successfully with {TotalItems} total items.", searchOutput.Total);

        return ListCategoriesOutput.FromSearchOutput(searchOutput);
    }
}