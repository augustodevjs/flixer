using Flixer.Catalog.Domain.Repository;
using Flixer.Catalog.Application.Exceptions;
using Flixer.Catalog.Application.Dtos.ViewModel.Category;
using Flixer.Catalog.Application.Dtos.InputModel.Category;
using Flixer.Catalog.Application.Contracts.UseCases.Category;

namespace Flixer.Catalog.Application.UseCases.Category;

public class GetCategory : IGetCategory
{
    private readonly ICategoryRepository _categoryRepository;

    public GetCategory(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<CategoryViewModel> Handle(GetCategoryInputModel request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.Get(request.Id, cancellationToken);

        if (category == null)
        {
            NotFoundException.ThrowIfNull(category, $"Category '{request.Id}' not found.");
        }

        return CategoryViewModel.FromCategory(category!);
    }
}
