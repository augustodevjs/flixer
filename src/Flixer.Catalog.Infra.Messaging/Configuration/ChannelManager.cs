using RabbitMQ.Client;

namespace Flixer.Catalog.Infra.Messaging.Configuration;

public class ChannelManager
{
    private IModel? _channel;
    private readonly object _lock = new();
    private readonly IConnection _connection;
    
    public ChannelManager(IConnection connection)
    {
        _connection = connection;
    }

    public IModel GetChannel()
    {
        lock (_lock)
        {
            if (_channel is { IsClosed: false }) return _channel;
            
            _channel = _connection.CreateModel();
            _channel.ConfirmSelect();
            return _channel;
        }
    }
}