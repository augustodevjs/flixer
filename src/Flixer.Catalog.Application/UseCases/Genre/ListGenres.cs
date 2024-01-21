//using Flixer.Catalog.Domain.Repository;
//using Flixer.Catalog.Application.Dtos.ViewModel.Genre;
//using Flixer.Catalog.Application.Dtos.InputModel.Genre;
//using Flixer.Catalog.Application.Contracts.UseCases.Genre;

//namespace Flixer.Catalog.Application.UseCases.Genre;

//public class ListGenres : IListGenres
//{
//    private readonly IGenreRepository _genreRepository;

//    public ListGenres(IGenreRepository genreRepository)
//    {
//        _genreRepository = genreRepository;
//    }
//    public async Task<ListGenresViewModel> Handle(ListGenresInputModel input, CancellationToken cancellationToken)
//    {
//        var searchOutput = await _genreRepository.Search(
//            input.ToSearchInput(), cancellationToken
//        );

//        return ListGenresViewModel.FromSearchOutput(searchOutput);
//    }
//}