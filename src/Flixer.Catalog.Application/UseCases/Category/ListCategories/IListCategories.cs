using MediatR;

namespace Flixer.Catalog.Application.UseCases.Category.ListCategories;

public interface IListCategories : IRequestHandler<ListCategoriesInput, ListCategoriesOutput>
{
}
