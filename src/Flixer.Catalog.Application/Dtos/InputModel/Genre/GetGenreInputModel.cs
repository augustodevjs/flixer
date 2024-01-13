using MediatR;
using Flixer.Catalog.Application.Dtos.ViewModel.Genre;

namespace Flixer.Catalog.Application.Dtos.InputModel.Genre;

public class GetGenreInputModel : IRequest<GenreViewModel>
{
    public Guid Id { get; set; }

    public GetGenreInputModel(Guid id)
        => Id = id;
}