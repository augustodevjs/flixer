﻿using MediatR;
using Microsoft.Extensions.Logging;
using Flixer.Catalog.Application.Exceptions;
using Flixer.Catalog.Domain.Contracts.Repository;
using Flixer.Catalog.Application.Common.Input.Category;

namespace Flixer.Catalog.Application.Commands.Category
{
    public class DeleteCategory : IRequestHandler<DeleteCategoryInput>
    {
        private readonly ILogger<DeleteCategory> _logger;
        private readonly ICategoryRepository _categoryRepository;

        public DeleteCategory(
            ILogger<DeleteCategory> logger,
            ICategoryRepository categoryRepository
        )
        {
            _logger = logger;
            _categoryRepository = categoryRepository;
        }

        public async Task Handle(DeleteCategoryInput request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetById(request.Id);

            if (category == null)
                NotFoundException.ThrowIfNull(category, $"Category '{request.Id}' not found.");
            
            _categoryRepository.Delete(category!);

            await _categoryRepository.UnityOfWork.Commit();

            _logger.LogInformation("Category with ID: {CategoryId} deleted successfully.", request.Id);
        }
    }
}