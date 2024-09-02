using MediatR;

namespace Flixer.Catalog.Application.Common.Input.Category;

public class DeleteCategoryInput : IRequest
{
    public Guid Id { get; private set; }

    public DeleteCategoryInput(Guid id)
        => Id = id;
}