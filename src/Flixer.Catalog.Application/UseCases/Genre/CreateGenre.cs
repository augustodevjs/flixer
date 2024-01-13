using Flixer.Catalog.Domain.Repository;
using Flixer.Catalog.Application.Exceptions;
using DomainEntity = Flixer.Catalog.Domain.Entities;
using Flixer.Catalog.Application.Dtos.ViewModel.Genre;
using Flixer.Catalog.Application.Dtos.InputModel.Genre;
using Flixer.Catalog.Application.Contracts.UnityOfWork;
using Flixer.Catalog.Application.Contracts.UseCases.Genre;

namespace Flixer.Catalog.Application.UseCases.Genre;
public class CreateGenre : ICreateGenre
{
    private readonly IUnityOfWork _unitOfWork;
    private readonly IGenreRepository _genreRepository;
    private readonly ICategoryRepository _categoryRepository;

    public CreateGenre(
        IUnityOfWork unitOfWork,
        IGenreRepository genreRepository,
        ICategoryRepository categoryRepository
    )
    {
        _unitOfWork = unitOfWork;
        _genreRepository = genreRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<GenreViewModel> Handle(CreateGenreInputModel request, CancellationToken cancellationToken)
    {
        var genre = new DomainEntity.Genre(request.Name, request.IsActive);

        if ((request.CategoriesIds?.Count ?? 0) > 0)
        {
            await ValidateCategoriesIds(request, cancellationToken);
            request.CategoriesIds?.ForEach(genre.AddCategory);
        }

        await _genreRepository.Insert(genre, cancellationToken);
        await _unitOfWork.Commit(cancellationToken);

        return GenreViewModel.FromGenre(genre);
    }

    private async Task ValidateCategoriesIds(CreateGenreInputModel request, CancellationToken cancellationToken)
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
