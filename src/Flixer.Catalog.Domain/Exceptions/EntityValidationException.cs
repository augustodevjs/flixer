namespace Flixer.Catalog.Domain.Exceptions;

public class EntityValidationException : Exception
{
    public List<string> Errors { get; }

    public EntityValidationException(string message, List<string> errors) : base(message)
    {
        Errors = errors;
    }
}