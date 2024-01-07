﻿using MediatR;
using Flixer.Catalog.Domain.Repository;
using Flixer.Catalog.Application.Contracts;

namespace Flixer.Catalog.Application.UseCases.Category.DeleteCategory;

public class DeleteCategory : IDeleteCategory
{
    private readonly IUnityOfWork _unitOfWork;
    private readonly ICategoryRepository _categoryRepository;

    public DeleteCategory(ICategoryRepository categoryRepository, IUnityOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _categoryRepository = categoryRepository;
    }

    public async Task<Unit> Handle(DeleteCategoryInput request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.Get(request.Id, cancellationToken);

        await _categoryRepository.Delete(category, cancellationToken);
        await _unitOfWork.Commit(cancellationToken);

        return Unit.Value;
    }
}
