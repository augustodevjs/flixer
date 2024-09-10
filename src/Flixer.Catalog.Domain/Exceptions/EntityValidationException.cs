namespace Flixer.Catalog.Domain.Exceptions;

public class EntityValidationException : Exception
{
    public IReadOnlyList<string> Errors { get; }

    public EntityValidationException(string message, IReadOnlyList<string> errors) : base(message)
    {
        Errors = errors;
    }
}