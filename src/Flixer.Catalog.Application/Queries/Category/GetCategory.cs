using MediatR;
using Microsoft.Extensions.Logging;
using Flixer.Catalog.Application.Exceptions;
using Flixer.Catalog.Domain.Contracts.Repository;
using Flixer.Catalog.Application.Common.Output.Category;

namespace Flixer.Catalog.Application.Queries.Category
{
    public class GetCategory : IRequestHandler<GetCategoryInput, CategoryOutput>
    {
        private readonly ILogger<GetCategory> _logger;
        private readonly ICategoryRepository _categoryRepository;

        public GetCategory(
            ILogger<GetCategory> logger,
            ICategoryRepository categoryRepository
        )
        {
            _logger = logger;
            _categoryRepository = categoryRepository;
        }

        public async Task<CategoryOutput> Handle(GetCategoryInput request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetById(request.Id);

            if (category == null)
                NotFoundException.ThrowIfNull(category, $"Category '{request.Id}' not found.");

            _logger.LogInformation("Category with ID: {CategoryId} retrieved successfully.", request.Id);

            return CategoryOutput.FromCategory(category!);
        }
    }
}