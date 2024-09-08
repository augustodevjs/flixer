using FluentValidation.Results;
using Flixer.Catalog.Domain.SeedWork;
using Flixer.Catalog.Domain.Exceptions;
using Flixer.Catalog.Domain.Validation;

namespace Flixer.Catalog.Domain.Entities;

public class Genre : AggregateRoot
{
    private readonly List<Guid> _categories;
    public string? Name { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    
    public IReadOnlyList<Guid> Categories => _categories.AsReadOnly();

    public Genre(string? name, bool isActive = true)
    {
        Name = name;
        IsActive = isActive;
        CreatedAt = DateTime.Now;
        _categories = new List<Guid>();
        
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

    public void Update(string? name)
    {
        Name = name;
        ValidateAndThrow();
    }
    
    public void AddCategory(Guid categoryId)
    {
        _categories.Add(categoryId);
        ValidateAndThrow();
    }
    
    public void RemoveCategory(Guid categoryId)
    {
        _categories.Remove(categoryId);
        ValidateAndThrow();
    }

    public void RemoveAllCategories()
    {
        _categories.Clear();
        ValidateAndThrow();
    }

    private bool Validate(out ValidationResult validationResult)
    {
        validationResult = new GenreValidator().Validate(this);
        return validationResult.IsValid;
    }

    private void ValidateAndThrow()
    {
        if (Validate(out var validationResult)) return;
        
        var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
        
        throw new EntityValidationException("Genre is invalid", errors);
    }
}