using MediatR;
using Microsoft.Extensions.Logging;
using Flixer.Catalog.Domain.Contracts;
using Flixer.Catalog.Domain.Exceptions;
using Flixer.Catalog.Application.Exceptions;
using Flixer.Catalog.Domain.Contracts.Repository;
using Flixer.Catalog.Application.Common.Input.Category;
using Flixer.Catalog.Application.Common.Output.Category;

namespace Flixer.Catalog.Application.Commands.Category;

public class UpdateCategory : IRequestHandler<UpdateCategoryInput, CategoryOutput>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateCategory> _logger;
    private readonly ICategoryRepository _categoryRepository;

    public UpdateCategory(
        IUnitOfWork unitOfWork,
        ILogger<UpdateCategory> logger,
        ICategoryRepository categoryRepository
    )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _categoryRepository = categoryRepository;
    }

    public async Task<CategoryOutput> Handle(UpdateCategoryInput request, CancellationToken cancellationToken)
    {
        try
        {
            var category = await _categoryRepository.GetById(request.Id);

            if (category == null)
                NotFoundException.ThrowIfNull(category, $"Category '{request.Id}' not found.");

            category!.Update(request.Name, request.Description);

            if (request.IsActive.HasValue && request.IsActive != category.IsActive)
            {
                (request.IsActive.Value ? (Action)category.Activate : category.Deactivate)();
            }

            _categoryRepository.Update(category);
            await _unitOfWork.Commit();

            _logger.LogInformation("Category with ID: {CategoryId} updated successfully.", request.Id);

            return CategoryOutput.FromCategory(category);
        }
        catch (EntityValidationException ex)
        {
            _logger.LogError(ex, "Validation error occurred while updating category with ID: {CategoryId}", request.Id);
            throw new EntityValidationException(ex.Message, ex.Errors);
        }
    }
}