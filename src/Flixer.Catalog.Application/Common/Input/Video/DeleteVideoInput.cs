using MediatR;

namespace Flixer.Catalog.Application.Common.Input.Video;

public class DeleteVideoInput : IRequest
{
    public Guid Id { get; private set; }

    public DeleteVideoInput(Guid id)
        => Id = id;
}