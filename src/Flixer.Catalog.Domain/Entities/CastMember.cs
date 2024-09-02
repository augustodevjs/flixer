using FluentValidation.Results;
using Flixer.Catalog.Domain.Enums;
using Flixer.Catalog.Domain.SeedWork;
using Flixer.Catalog.Domain.Exceptions;
using Flixer.Catalog.Domain.Validation;

namespace Flixer.Catalog.Domain.Entities;

public class CastMember : AggregateRoot
{
    public string Name { get; private set; }
    public CastMemberType Type { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public CastMember(string name, CastMemberType type)
    {
        Name = name;
        Type = type;
        CreatedAt = DateTime.Now;
        
        ValidateAndThrow();
    }
    
    public void Update(string newName, CastMemberType newType)
    {
        Name = newName;
        Type = newType;
        
        ValidateAndThrow();
    }

    public override bool Validate(out ValidationResult validationResult)
    {
        validationResult = new CastMemberValidator().Validate(this);
        return validationResult.IsValid;
    }

    private void ValidateAndThrow()
    {
        if (Validate(out var validationResult)) return;
        
        var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
        
        throw new EntityValidationException("CastMember is invalid", errors);
    }
}