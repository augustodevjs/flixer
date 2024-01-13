using FluentValidation;

namespace Flixer.Catalog.Application.Dtos.InputModel.Category;

public class GetCategoryValidatorInputModel : AbstractValidator<GetCategoryInputModel>
{
    public GetCategoryValidatorInputModel()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("'Id' must not be empty.");
    }
}