using MediatR;
using Microsoft.Extensions.Logging;
using Flixer.Catalog.Domain.Exceptions;
using Flixer.Catalog.Application.Exceptions;
using Flixer.Catalog.Domain.Contracts.Repository;
using Flixer.Catalog.Application.Commands.Category.CreateCategory;

namespace Flixer.Catalog.Application.Commands.Category.UpdateCategory
{
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, CategoryViewModel>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILogger<UpdateCategoryCommandHandler> _logger;

        public UpdateCategoryCommandHandler(
            ICategoryRepository categoryRepository, 
            ILogger<UpdateCategoryCommandHandler> logger
        )
        {
            _logger = logger;
            _categoryRepository = categoryRepository;
        }

        public async Task<CategoryViewModel> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling UpdateCategoryCommand with ID: {CategoryId}", request.Id);

            try
            {
                var category = await _categoryRepository.GetById(request.Id);

                if (category == null)
                {
                    _logger.LogWarning("Category with ID: {CategoryId} not found.", request.Id);
                    NotFoundException.ThrowIfNull(category, $"Category '{request.Id}' not found.");
                }

                _logger.LogInformation("Updating category with ID: {CategoryId}", request.Id);

                category!.Update(request.Name, request.Description);

                if (request.IsActive != null && request.IsActive != category.IsActive)
                {
                    if ((bool)request.IsActive)
                    {
                        _logger.LogInformation("Activating category with ID: {CategoryId}", request.Id);
                        category.Activate();
                    }
                    else
                    {
                        _logger.LogInformation("Deactivating category with ID: {CategoryId}", request.Id);
                        category.Deactivate();
                    }
                }

                _categoryRepository.Update(category);
                await _categoryRepository.UnityOfWork.Commit();

                _logger.LogInformation("Category with ID: {CategoryId} updated successfully.", request.Id);

                return CategoryViewModel.FromCategory(category);
            }
            catch (EntityValidationException ex)
            {
                _logger.LogError(ex, "Validation error occurred while updating category with ID: {CategoryId}", request.Id);
                throw new EntityValidationException(ex.Message, ex.Errors);
            }
        }
    }
}
