using MediatR;
using Microsoft.Extensions.Logging;
using Flixer.Catalog.Domain.Contracts;
using Flixer.Catalog.Domain.Exceptions;
using Flixer.Catalog.Domain.Contracts.Repository;
using Flixer.Catalog.Application.Common.Input.Category;
using Flixer.Catalog.Application.Common.Output.Category;

namespace Flixer.Catalog.Application.Commands.Category;

public class CreateCategory : IRequestHandler<CreateCategoryInput, CategoryOutput>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateCategory> _logger;
    private readonly ICategoryRepository _categoryRepository;

    public CreateCategory(
        IUnitOfWork unityOfWork,
        ILogger<CreateCategory> logger,
        ICategoryRepository categoryRepository
    )
    {
        _logger = logger;
        _unitOfWork = unityOfWork;
        _categoryRepository = categoryRepository;
    }

    public async Task<CategoryOutput> Handle(CreateCategoryInput request, CancellationToken cancellationToken)
    {
        try
        {
            var category = new Domain.Entities.Category(
                request.Name, 
                request.Description, 
                request.IsActive
            );

            _categoryRepository.Create(category);

            await _unitOfWork.Commit();

            _logger.LogInformation("Category created successfully with ID: {CategoryId}", category.Id);

            return CategoryOutput.FromCategory(category);
        }
        catch (EntityValidationException ex)
        {
            _logger.LogError(ex, "Validation error occurred while creating category with Name: {Name}", request.Name);
            throw new EntityValidationException(ex.Message, ex.Errors);
        }
    }
}