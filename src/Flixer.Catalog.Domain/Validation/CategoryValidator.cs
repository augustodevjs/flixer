using FluentValidation;
using Flixer.Catalog.Domain.Entities;

namespace Flixer.Catalog.Domain.Validation;

public class CategoryValidator : AbstractValidator<Category>
{
    public CategoryValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("Name is required.")
            .Length(3, 255).WithMessage("Name must be between 3 and 255 characters.");

        RuleFor(c => c.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(10000).WithMessage("Description cannot exceed 10000 characters.");
    }
}