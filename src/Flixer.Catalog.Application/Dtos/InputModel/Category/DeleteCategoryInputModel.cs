using MediatR;

namespace Flixer.Catalog.Application.Dtos.InputModel.Category;

public class DeleteCategoryInputModel : IRequest
{
    public Guid Id { get; set; }

    public DeleteCategoryInputModel(Guid id)
        => Id = id;
}
