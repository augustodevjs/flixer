using Flixer.Catalog.Domain.Repository;
using Flixer.Catalog.Application.Contracts.Category;
using Flixer.Catalog.Application.Dtos.ViewModel.Category;
using Flixer.Catalog.Application.Dtos.InputModel.Category;

namespace Flixer.Catalog.Application.UseCases.Category;

public class ListCategories : IListCategories
{
    private readonly ICategoryRepository _categoryRepository;

    public ListCategories(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<ListCategoriesViewModel> Handle(ListCategoriesInputModel request, CancellationToken cancellationToken)
    {
        var searchOutput = await _categoryRepository.Search(
            request.ToSearchInput(),
            cancellationToken
        );

        return ListCategoriesViewModel.FromSearchOutput(searchOutput);
    }
}
