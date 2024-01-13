using MediatR;

namespace Flixer.Catalog.Application.Dtos.InputModel.Genre;

public class DeleteGenreInputModel : IRequest
{
    public Guid Id { get; set; }

    public DeleteGenreInputModel(Guid id)
        => Id = id;
}