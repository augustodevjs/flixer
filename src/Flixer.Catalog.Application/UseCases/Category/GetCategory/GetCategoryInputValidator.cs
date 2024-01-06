﻿using FluentValidation;

namespace Flixer.Catalog.Application.UseCases.Category.GetCategory;

public class GetCategoryInputValidator : AbstractValidator<GetCategoryInput>
{
    public GetCategoryInputValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("'Id' must not be empty.");
    }
}
