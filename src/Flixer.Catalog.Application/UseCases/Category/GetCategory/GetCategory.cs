using Flixer.Catalog.Domain.Repository;
using Flixer.Catalog.Application.Exceptions;
using Flixer.Catalog.Application.UseCases.Category.Common;

namespace Flixer.Catalog.Application.UseCases.Category.GetCategory;

public class GetCategory : IGetCategory
{
    private readonly ICategoryRepository _categoryRepository;

    public GetCategory(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<CategoryModelOutput> Handle(GetCategoryInput request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.Get(request.Id, cancellationToken);
        
        if(category == null)
        {
            NotFoundException.ThrowIfNull(category, $"Category '{request.Id}' not found");
        }

        return CategoryModelOutput.FromCategory(category);
    }
}
