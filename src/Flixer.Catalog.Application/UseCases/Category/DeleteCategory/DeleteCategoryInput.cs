
using MediatR;

namespace Flixer.Catalog.Application.UseCases.Category.DeleteCategory;

public class DeleteCategoryInput : IRequest
{
    public Guid Id { get; set; }

    public DeleteCategoryInput(Guid id)
        => Id = id;
}
