using MediatR;

namespace Flixer.Catalog.Application.Common.Input.Genre;

public class DeleteGenreInput : IRequest
{
    public Guid Id { get; private set; }
    
    public DeleteGenreInput(Guid id)
    {
        Id = id;
    }
}