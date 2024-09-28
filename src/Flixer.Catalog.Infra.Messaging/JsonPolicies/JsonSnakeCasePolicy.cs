using System.Text.Json;
using Newtonsoft.Json.Serialization;

namespace Flixer.Catalog.Infra.Messaging.JsonPolicies;

public class JsonSnakeCasePolicy : JsonNamingPolicy
{
    public override string ConvertName(string name)
        => name.ToSnakeCase();
}

public static class StringSnakeCaseExtension
{
    private static readonly NamingStrategy SnakeCaseNamingStrategy =
        new SnakeCaseNamingStrategy();

    public static string ToSnakeCase(this string stringToConvert)
    {
        ArgumentNullException
            .ThrowIfNull(stringToConvert, nameof(stringToConvert));
        
        return SnakeCaseNamingStrategy
            .GetPropertyName(stringToConvert, false);
    }
}