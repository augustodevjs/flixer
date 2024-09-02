using MediatR;
using Microsoft.Extensions.Logging;
using Flixer.Catalog.Domain.Exceptions;
using Flixer.Catalog.Application.Exceptions;
using Flixer.Catalog.Domain.Contracts.Repository;
using Flixer.Catalog.Application.Common.Input.Genre;
using Flixer.Catalog.Application.Common.Output.Genre;

namespace Flixer.Catalog.Application.Commands.Genre;

public class UpdateGenre : IRequestHandler<UpdateGenreInput, GenreOutput>
{
    private readonly ILogger<UpdateGenre> _logger;
    private readonly IGenreRepository _genreRepository;
    private readonly ICategoryRepository _categoryRepository;

    public UpdateGenre(
        ILogger<UpdateGenre> logger, 
        IGenreRepository genreRepository, 
        ICategoryRepository categoryRepository
    )
    {
        _logger = logger;
        _genreRepository = genreRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<GenreOutput> Handle(UpdateGenreInput request, CancellationToken cancellationToken) 
    {
        try
        {
            var genre = await _genreRepository.GetById(request.Id);

            if (genre == null)
            {
                _logger.LogWarning("Genre with ID: {Id} not found.", request.Id);
                throw new NotFoundException($"Genre with ID {request.Id} not found.");
            }

            genre.Update(request.Name);
        
            if (request.IsActive is not null && request.IsActive != genre.IsActive)
            {
                if ((bool)request.IsActive) 
                {
                    genre.Activate();
                }
                else 
                {
                    genre.Deactivate();
                }
            }
        
            if (request.CategoriesIds is not null)
            {
                genre.RemoveAllCategories();
                
                if (request.CategoriesIds.Count > 0)
                {
                    await ValidateCategoriesIds(request);
                    request.CategoriesIds.ForEach(genre.AddCategory);
                }
            }
        
            _genreRepository.Update(genre);

            await _genreRepository.UnityOfWork.Commit();
            
            _logger.LogInformation("Genre with ID: {GenreId} updated successfully.", request.Id);

            return GenreOutput.FromGenre(genre);
        }
        catch (EntityValidationException ex)
        {
            _logger.LogError(ex, "Validation error occurred while updating category with ID: {CategoryId}", request.Id);
            throw new EntityValidationException(ex.Message, ex.Errors);
        }
        catch (RelatedAggregateException ex)
        {
            _logger.LogError(ex, "Related aggregate exception occurred: {Message}", ex.Message);
            throw;
        }
    }

    private async Task ValidateCategoriesIds(UpdateGenreInput request)
    {
        var idsInPersistence = await _categoryRepository
            .GetIdsListByIds(request.CategoriesIds!);
        
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
