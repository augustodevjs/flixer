using MediatR;
using Flixer.Catalog.Application.Common.Output;
using Flixer.Catalog.Application.Common.Output.Genre;

namespace Flixer.Catalog.Application.Common.Input.Genre;

public class GetGenreInput : IRequest<GenreOutput>
{
    public Guid Id { get; private set; }

    public GetGenreInput(Guid id)
    {
        Id = id;
    }
}