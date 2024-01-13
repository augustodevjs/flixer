using MediatR;
using Flixer.Catalog.Application.Dtos.ViewModel.Category;
using Flixer.Catalog.Application.Dtos.InputModel.Category;

namespace Flixer.Catalog.Application.Contracts.Category;

public interface ICreateCategory : IRequestHandler<CreateCategoryInputModel, CategoryViewModel>
{
    Task<CategoryViewModel> Handle(CreateCategoryInputModel input, CancellationToken cancellationToken);
}
