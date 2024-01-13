using FluentValidation;

namespace Flixer.Catalog.Application.Dtos.InputModel.Category;

public class UpdateCategoryValidatorInputModel : AbstractValidator<UpdateCategoryInputModel>
{
    public UpdateCategoryValidatorInputModel()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("'Id' must not be empty.");
    }
}
