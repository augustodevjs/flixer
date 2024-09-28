namespace Flixer.Catalog.Infra.Messaging.Configuration;

public class RabbitMqConfiguration
{
    public string? Hostname { get; set; }
    public int? Port { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? Exchange { get; set; }
    public string? VideoEncodedQueue { get; set; } 
}