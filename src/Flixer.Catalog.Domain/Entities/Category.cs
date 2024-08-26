using FluentValidation.Results;
using Flixer.Catalog.Domain.SeedWork;
using Flixer.Catalog.Domain.Exceptions;
using Flixer.Catalog.Domain.Validation;

namespace Flixer.Catalog.Domain.Entities;

public class Category : AggregateRoot
{
    public string Name { get; private set; }
    public bool IsActive { get; private set; }
    public string Description { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public Category(string name, string description, bool isActive = true)
    {
        Name = name;
        Description = description;
        IsActive = isActive;
        CreatedAt = DateTime.Now;

        ValidateAndThrow();
    }

    public void Activate()
    {
        IsActive = true;
        ValidateAndThrow();
    }

    public void Deactivate()
    {
        IsActive = false;
        ValidateAndThrow();
    }

    public void Update(string name, string? description = null )
    {
        Name = name;
        Description = description ?? Description;
        ValidateAndThrow();
    }

    public override bool Validate(out ValidationResult validationResult)
    {
        validationResult = new CategoryValidator().Validate(this);
        return validationResult.IsValid;
    }

    private void ValidateAndThrow()
    {
        if (!Validate(out var validationResult))
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            throw new EntityValidationException("Category is invalid", errors);
        }
    }
}