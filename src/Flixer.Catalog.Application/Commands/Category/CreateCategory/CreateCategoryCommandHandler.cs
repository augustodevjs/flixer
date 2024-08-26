using MediatR;
using Microsoft.Extensions.Logging;
using Flixer.Catalog.Domain.Exceptions;
using Flixer.Catalog.Domain.Contracts.Repository;

namespace Flixer.Catalog.Application.Commands.Category.CreateCategory
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, CategoryViewModel>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILogger<CreateCategoryCommandHandler> _logger;

        public CreateCategoryCommandHandler(
            ICategoryRepository categoryRepository, 
            ILogger<CreateCategoryCommandHandler> logger
        )
        {
            _logger = logger;
            _categoryRepository = categoryRepository;
        }

        public async Task<CategoryViewModel> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling CreateCategoryCommand with Name: {Name}", request.Name);

            try
            {
                var category = new Domain.Entities.Category(request.Name, request.Description, request.IsActive);

                _logger.LogInformation("Creating category with Name: {Name}", request.Name);

                _categoryRepository.Create(category);

                await _categoryRepository.UnityOfWork.Commit();

                _logger.LogInformation("Category created successfully with ID: {CategoryId}", category.Id);

                return CategoryViewModel.FromCategory(category);
            }
            catch (EntityValidationException ex)
            {
                _logger.LogError(ex, "Validation error occurred while creating category with Name: {Name}", request.Name);
                throw new EntityValidationException(ex.Message, ex.Errors);
            }
        }
    }
}