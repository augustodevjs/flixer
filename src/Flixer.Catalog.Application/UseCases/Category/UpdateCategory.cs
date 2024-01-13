using Flixer.Catalog.Domain.SeedWork;
using Flixer.Catalog.Domain.Repository;
using Flixer.Catalog.Application.Exceptions;
using Flixer.Catalog.Application.Contracts.Category;
using Flixer.Catalog.Application.Dtos.ViewModel.Category;
using Flixer.Catalog.Application.Dtos.InputModel.Category;

namespace Flixer.Catalog.Application.UseCases.Category;

public class UpdateCategory : IUpdateCategory
{
    private readonly IUnityOfWork _unitOfWork;
    private readonly ICategoryRepository _categoryRepository;

    public UpdateCategory(ICategoryRepository categoryRepository, IUnityOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _categoryRepository = categoryRepository;
    }

    public async Task<CategoryViewModel> Handle(UpdateCategoryInputModel request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.Get(request.Id, cancellationToken);

        if (category == null)
        {
            NotFoundException.ThrowIfNull(category, $"Category '{request.Id}' not found.");
        }

        category!.Update(request.Name, request.Description);

        if (request.IsActive != null && request.IsActive != category.IsActive)
            if ((bool)request.IsActive!)
                category.Activate();
            else
                category.Deactivate();

        await _unitOfWork.Commit(cancellationToken);
        await _categoryRepository.Update(category, cancellationToken);

        return CategoryViewModel.FromCategory(category);
    }
}