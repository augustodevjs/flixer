using MediatR;
using Microsoft.Extensions.Logging;
using Flixer.Catalog.Application.Exceptions;
using Flixer.Catalog.Domain.Contracts.Repository;
using Flixer.Catalog.Application.Commands.Category.CreateCategory;

namespace Flixer.Catalog.Application.Queries.Category
{
    public class GetCategoryQueryHandler : IRequestHandler<GetCategoryQuery, CategoryViewModel>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILogger<GetCategoryQueryHandler> _logger;

        public GetCategoryQueryHandler(
            ICategoryRepository categoryRepository, 
            ILogger<GetCategoryQueryHandler> logger
        )
        {
            _logger = logger;
            _categoryRepository = categoryRepository;
        }

        public async Task<CategoryViewModel> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetCategoryQuery with ID: {CategoryId}", request.Id);

            var category = await _categoryRepository.GetById(request.Id);

            if (category == null)
            {
                _logger.LogWarning("Category with ID: {CategoryId} not found.", request.Id);
                NotFoundException.ThrowIfNull(category, $"Category '{request.Id}' not found.");
            }

            _logger.LogInformation("Category with ID: {CategoryId} retrieved successfully.", request.Id);

            return CategoryViewModel.FromCategory(category!);
        }
    }
}