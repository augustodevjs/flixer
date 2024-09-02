using MediatR;

namespace Flixer.Catalog.Application.Common.Output.Category;

public class GetCategoryInput : IRequest<CategoryOutput>
{
    public Guid Id { get; private set; }

    public GetCategoryInput(Guid id) => Id = id;
}