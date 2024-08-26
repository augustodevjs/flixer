using MediatR;
using Microsoft.Extensions.Logging;
using Flixer.Catalog.Application.Exceptions;
using Flixer.Catalog.Domain.Contracts.Repository;

namespace Flixer.Catalog.Application.Commands.Category.DeleteCategory
{
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILogger<DeleteCategoryCommandHandler> _logger;

        public DeleteCategoryCommandHandler(
            ICategoryRepository categoryRepository, 
            ILogger<DeleteCategoryCommandHandler> logger
        )
        {
            _logger = logger;
            _categoryRepository = categoryRepository;
        }

        public async Task Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling DeleteCategoryCommand with ID: {CategoryId}", request.Id);

            var category = await _categoryRepository.GetById(request.Id);

            if (category == null)
            {
                _logger.LogWarning("Category with ID: {CategoryId} not found.", request.Id);
                NotFoundException.ThrowIfNull(category, $"Category '{request.Id}' not found.");
            }
            
            _logger.LogInformation("Deleting category with ID: {CategoryId}", request.Id);

            _categoryRepository.Delete(category);

            await _categoryRepository.UnityOfWork.Commit();

            _logger.LogInformation("Category with ID: {CategoryId} deleted successfully.", request.Id);
        }
    }
}