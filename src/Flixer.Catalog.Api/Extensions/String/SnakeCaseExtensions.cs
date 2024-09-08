using Newtonsoft.Json.Serialization;

namespace Flixer.Catalog.Api.Extensions.String;

public static class SnakeCaseExtensions
{
    private static readonly NamingStrategy SnakeCaseNamingStrategy = new SnakeCaseNamingStrategy();

    public static string ToSnakeCase(this string stringToConvert)
    {
        ArgumentNullException.ThrowIfNull(stringToConvert, nameof(stringToConvert));

        return SnakeCaseNamingStrategy.GetPropertyName(stringToConvert, false);
    }
}
