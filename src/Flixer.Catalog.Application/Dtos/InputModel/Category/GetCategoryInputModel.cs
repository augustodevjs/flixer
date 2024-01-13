using MediatR;
using Flixer.Catalog.Application.Dtos.ViewModel.Category;

namespace Flixer.Catalog.Application.Dtos.InputModel.Category;

public class GetCategoryInputModel : IRequest<CategoryViewModel>
{
    public Guid Id { get; set; }

    public GetCategoryInputModel(Guid id) => Id = id;
}
