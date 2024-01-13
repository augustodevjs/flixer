using Flixer.Catalog.Domain.SeedWork;
using Flixer.Catalog.Domain.Repository;
using Flixer.Catalog.Application.Exceptions;
using Flixer.Catalog.Application.Dtos.ViewModel.Genre;
using Flixer.Catalog.Application.Dtos.InputModel.Genre;
using Flixer.Catalog.Application.Contracts.UseCases.Genre;

namespace Flixer.Catalog.Application.UseCases.Genre;
public class UpdateGenre
    : IUpdateGenre
{
    private readonly IUnityOfWork _unitOfWork;
    private readonly IGenreRepository _genreRepository;
    private readonly ICategoryRepository _categoryRepository;

    public UpdateGenre(
        IUnityOfWork unitOfWork,
        IGenreRepository genreRepository,
        ICategoryRepository categoryRepository
    )
    {
        _unitOfWork = unitOfWork;
        _genreRepository = genreRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<GenreViewModel> Handle(UpdateGenreInputModel request, CancellationToken cancellationToken)
    {
        var genre = await _genreRepository.Get(request.Id, cancellationToken);

        if (genre == null)
        {
            NotFoundException.ThrowIfNull(genre, $"Genre '{request.Id}' not found.");
        }

        genre!.Update(request.Name);

        if (request.IsActive is not null && request.IsActive != genre.IsActive)
        {
            if ((bool)request.IsActive)
                genre.Activate();
            else
                genre.Deactivate();
        }

        if (request.CategoriesIds is not null)
        {
            genre.RemoveAllCategories();

            if (request.CategoriesIds.Count > 0)
            {
                await ValidateCategoriesIds(request, cancellationToken);
                request.CategoriesIds?.ForEach(genre.AddCategory);
            }
        }

        await _genreRepository.Update(genre, cancellationToken);
        await _unitOfWork.Commit(cancellationToken);

        return GenreViewModel.FromGenre(genre);
    }

    private async Task ValidateCategoriesIds(UpdateGenreInputModel request, CancellationToken cancellationToken)
    {
        var IdsInPersistence = await _categoryRepository.GetIdsListByIds(request.CategoriesIds!, cancellationToken);

        if (IdsInPersistence.Count < request.CategoriesIds!.Count)
        {
            var notFoundIds = request.CategoriesIds.FindAll(x => !IdsInPersistence.Contains(x));
            var notFoundIdsAsString = string.Join(", ", notFoundIds);

            throw new RelatedAggregateException(
                $"Related category id (or ids) not found: {notFoundIdsAsString}"
            );
        }
    }
}