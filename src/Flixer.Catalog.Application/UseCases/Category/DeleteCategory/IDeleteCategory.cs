using MediatR;

namespace Flixer.Catalog.Application.UseCases.Category.DeleteCategory;

public interface IDeleteCategory : IRequestHandler<DeleteCategoryInput>
{

}