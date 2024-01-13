using Flixer.Catalog.Domain.Repository;
using Flixer.Catalog.Application.Contracts;
using Flixer.Catalog.Application.Exceptions;
using Flixer.Catalog.Application.UseCases.Category.Common;

namespace Flixer.Catalog.Application.UseCases.Category.UpdateCategory;

public class UpdateCategory : IUpdateCategory
{
    private readonly IUnityOfWork _unitOfWork;
    private readonly ICategoryRepository _categoryRepository;

    public UpdateCategory(ICategoryRepository categoryRepository, IUnityOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _categoryRepository = categoryRepository;
    }

    public async Task<CategoryModelOutput> Handle(UpdateCategoryInput request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.Get(request.Id, cancellationToken);

        if (category == null)
        {
            NotFoundException.ThrowIfNull(category, $"Category '{request.Id}' not found");
        }

        category!.Update(request.Name, request.Description);

        if (request.IsActive != null && request.IsActive != category.IsActive)
            if ((bool)request.IsActive!) 
                category.Activate();
            else 
                category.Deactivate();

        await _unitOfWork.Commit(cancellationToken);
        await _categoryRepository.Update(category, cancellationToken);

        return CategoryModelOutput.FromCategory(category);
    }
}
