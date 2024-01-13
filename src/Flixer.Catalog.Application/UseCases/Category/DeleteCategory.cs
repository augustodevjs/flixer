using MediatR;
using Flixer.Catalog.Domain.Repository;
using Flixer.Catalog.Application.Exceptions;
using Flixer.Catalog.Application.Contracts.UnityOfWork;
using Flixer.Catalog.Application.Dtos.InputModel.Category;
using Flixer.Catalog.Application.Contracts.UseCases.Category;

namespace Flixer.Catalog.Application.UseCases.Category;

public class DeleteCategory : IDeleteCategory
{
    private readonly IUnityOfWork _unitOfWork;
    private readonly ICategoryRepository _categoryRepository;

    public DeleteCategory(ICategoryRepository categoryRepository, IUnityOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _categoryRepository = categoryRepository;
    }

    public async Task<Unit> Handle(DeleteCategoryInputModel request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.Get(request.Id, cancellationToken);

        if (category == null)
        {
            NotFoundException.ThrowIfNull(category, $"Category '{request.Id}' not found.");
        }

        await _categoryRepository.Delete(category!, cancellationToken);
        await _unitOfWork.Commit(cancellationToken);

        return Unit.Value;
    }
}