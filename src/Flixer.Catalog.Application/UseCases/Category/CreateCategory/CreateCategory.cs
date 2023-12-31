﻿using Flixer.Catalog.Domain.Repository;
using Flixer.Catalog.Application.Contracts;
using DomainEntity = Flixer.Catalog.Domain.Entities;
using Flixer.Catalog.Application.UseCases.Category.Common;

namespace Flixer.Catalog.Application.UseCases.Category.CreateCategory;

public class CreateCategory : ICreateCategory
{
    private readonly IUnityOfWork _unityOfWork;
    private readonly ICategoryRepository _categoryRepository;

    public CreateCategory(ICategoryRepository categoryRepository, IUnityOfWork unityOfWork)
    {
        _unityOfWork = unityOfWork;
        _categoryRepository = categoryRepository;
    }

    public async Task<CategoryModelOutput> Handle(CreateCategoryInput input, CancellationToken cancellationToken)
    {
        var category = new DomainEntity.Category(input.Name, input.Description, input.IsActive);

        await _categoryRepository.Insert(category, cancellationToken);

        await _unityOfWork.Commit(cancellationToken);

        return CategoryModelOutput.FromCategory(category);
    }
}
