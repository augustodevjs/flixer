using Flixer.Catalog.Domain.Events;

namespace Flixer.Catalog.Infra.Messaging.Configuration;

internal static class EventsMapping
{
    private static Dictionary<string, string> RoutingKeys => new()
    {
        { nameof(VideoUploadedEvent), "video.created" }
    };

    public static string GetRoutingKey<T>() => RoutingKeys[typeof(T).Name];
}
