using MediatR;
using Flixer.Catalog.Application.UseCases.Category.Common;

namespace Flixer.Catalog.Application.UseCases.Category.CreateCategory;

public interface ICreateCategory : IRequestHandler<CreateCategoryInput, CategoryModelOutput>
{
    Task<CategoryModelOutput> Handle(CreateCategoryInput input, CancellationToken cancellationToken);
}
