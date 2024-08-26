using MediatR;

namespace Flixer.Catalog.Application.Commands.Category.DeleteCategory;

public class DeleteCategoryCommand : IRequest
{
    public Guid Id { get; private set; }

    public DeleteCategoryCommand(Guid id)
        => Id = id;
}