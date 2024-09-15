using MediatR;
using Microsoft.Extensions.Logging;
using Flixer.Catalog.Domain.Contracts;
using Flixer.Catalog.Domain.Exceptions;
using Flixer.Catalog.Application.Exceptions;
using Flixer.Catalog.Domain.Contracts.Repository;
using Flixer.Catalog.Application.Common.Input.Genre;
using Flixer.Catalog.Application.Common.Output.Genre;

namespace Flixer.Catalog.Application.Commands.Genre;

public class CreateGenre : IRequestHandler<CreateGenreInput, GenreOutput>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateGenre> _logger;
    private readonly IGenreRepository _genreRepository;
    private readonly ICategoryRepository _categoryRepository;

    public CreateGenre(
        IUnitOfWork unitOfWork,
        ILogger<CreateGenre> logger,
        IGenreRepository genreRepository, 
        ICategoryRepository categoryRepository
    )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _genreRepository = genreRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<GenreOutput> Handle(CreateGenreInput request, CancellationToken cancellationToken)
    {
        try
        {
            var genre = new Domain.Entities.Genre(request.Name, request.IsActive);

            if ((request.CategoriesIds?.Count ?? 0) > 0)
            {
                await ValidateCategoriesIds(request);
                request.CategoriesIds?.ForEach(genre.AddCategory);
            }
            
            _genreRepository.Create(genre);
            
            await _unitOfWork.Commit();
            
            _logger.LogInformation("Genre created successfully with ID: {GenreId}", genre.Id);

            return GenreOutput.FromGenre(genre);
        }
        catch (EntityValidationException ex)
        {
            _logger.LogError(ex, "Validation error occurred while creating Genre with Name: {Name}, IsActive: {IsActive}", request.Name, request.IsActive);
            throw;
        }
        catch (RelatedAggregateException ex)
        {
            _logger.LogError(ex, "Related aggregate exception occurred: {Message}", ex.Message);
            throw;
        }
    }
    
    private async Task ValidateCategoriesIds(CreateGenreInput request)
    {
        var idsInPersistence = await _categoryRepository.GetIdsListByIds(request.CategoriesIds!);

        if (idsInPersistence.Count < request.CategoriesIds!.Count)
        {
            var notFoundIds = request.CategoriesIds
                .FindAll(x => !idsInPersistence.Contains(x));

            var notFoundIdsAsString = string.Join(", ", notFoundIds);

            throw new RelatedAggregateException(
                $"Related category id (or ids) not found: {notFoundIdsAsString}"
            );
        }
    }
}
