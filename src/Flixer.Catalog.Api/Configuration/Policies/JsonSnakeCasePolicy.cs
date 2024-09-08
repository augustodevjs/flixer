using System.Text.Json;
using Flixer.Catalog.Api.Extensions.String;

namespace Flixer.Catalog.Api.Configuration.Policies;

public class JsonSnakeCasePolicy : JsonNamingPolicy
{
    public override string ConvertName(string name) =>
        name.ToSnakeCase();
}
