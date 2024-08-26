using MediatR;
using Flixer.Catalog.Application.Commands.Category.CreateCategory;

namespace Flixer.Catalog.Application.Queries.Category;

public class GetCategoryQuery : IRequest<CategoryViewModel>
{
    public Guid Id { get; private set; }

    public GetCategoryQuery(Guid id) => Id = id;
}