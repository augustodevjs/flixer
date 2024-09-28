using System.Text.Json;
using RabbitMQ.Client;
using Microsoft.Extensions.Options;
using Flixer.Catalog.Application.Intefaces;
using Flixer.Catalog.Infra.Messaging.JsonPolicies;
using Flixer.Catalog.Infra.Messaging.Configuration;

namespace Flixer.Catalog.Infra.Messaging.Producer;

public class RabbitMqProducer : IMessageProducer
{
    private readonly IModel _channel;
    private readonly string _exchange;

    public RabbitMqProducer(
        IModel channel,
        IOptions<RabbitMqConfiguration> options
    )
    {
        _channel = channel;
        _exchange = options.Value.Exchange!;
    }

    public Task SendMessageAsync<T>(T message)
    {
        var routingKey = EventsMapping.GetRoutingKey<T>();
        
        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = new JsonSnakeCasePolicy()
        };
        
        var @event = JsonSerializer.SerializeToUtf8Bytes(message, jsonOptions);
        
        _channel.BasicPublish(
            exchange: _exchange,
            routingKey: routingKey,
            body: @event
        );
        
        _channel.WaitForConfirmsOrDie();
        
        return Task.CompletedTask;
    }
}
