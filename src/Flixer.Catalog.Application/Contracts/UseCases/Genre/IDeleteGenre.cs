using MediatR;
using Flixer.Catalog.Application.Dtos.InputModel.Genre;

namespace Flixer.Catalog.Application.Contracts.UseCases.Genre;

public interface IDeleteGenre : IRequestHandler<DeleteGenreInputModel>
{
}