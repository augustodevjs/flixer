using Flixer.Catalog.Domain.Exceptions;

namespace Flixer.Catalog.Domain.Validation
{
    public static class DomainValidation
    {
        public static void NotNull(object? target, string fieldName)
        {
            if(target == null)
                throw new EntityValidationException($"{fieldName} should not be null.");
        }
    }
}
