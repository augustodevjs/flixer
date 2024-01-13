using Flixer.Catalog.Domain.SeedWork;
using Flixer.Catalog.Domain.Repository;
using DomainEntity = Flixer.Catalog.Domain.Entities;
using Flixer.Catalog.Application.Contracts.UnityOfWork;
using Flixer.Catalog.Application.Dtos.ViewModel.Category;
using Flixer.Catalog.Application.Dtos.InputModel.Category;
using Flixer.Catalog.Application.Contracts.UseCases.Category;

namespace Flixer.Catalog.Application.UseCases.Category;

public class CreateCategory : ICreateCategory
{
    private readonly IUnityOfWork _unityOfWork;
    private readonly ICategoryRepository _categoryRepository;

    public CreateCategory(ICategoryRepository categoryRepository, IUnityOfWork unityOfWork)
    {
        _unityOfWork = unityOfWork;
        _categoryRepository = categoryRepository;
    }

    public async Task<CategoryViewModel> Handle(CreateCategoryInputModel input, CancellationToken cancellationToken)
    {
        var category = new DomainEntity.Category(input.Name, input.Description, input.IsActive);

        await _categoryRepository.Insert(category, cancellationToken);

        await _unityOfWork.Commit(cancellationToken);

        return CategoryViewModel.FromCategory(category);
    }
}
