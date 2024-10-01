using RabbitMQ.Client;
using Microsoft.Extensions.Options;
using Flixer.Catalog.Application.Intefaces;
using Flixer.Catalog.Infra.Messaging.Producer;
using Flixer.Catalog.Infra.Messaging.Configuration;

namespace Flixer.Catalog.Api.Configuration;

public static class RabbitMqConfiguration
{
    public static IServiceCollection AddMessaging(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<Infra.Messaging.Configuration.RabbitMqConfiguration>(configuration.GetSection("RabbitMqConfiguration"));

        services.AddSingleton(sp =>
        {
            var config = sp.GetRequiredService<IOptions<Infra.Messaging.Configuration.RabbitMqConfiguration>>().Value;
            
            var factory = new ConnectionFactory()
            {
                HostName = config.Hostname,
                UserName = config.Username,
                Password = config.Password
            };
            
            return factory.CreateConnection();
        });

        services.AddSingleton<ChannelManager>();

        services.AddTransient<IMessageProducer>(sp =>
        {
            var channelManager = sp.GetRequiredService<ChannelManager>();
            var config = sp.GetRequiredService<IOptions<Infra.Messaging.Configuration.RabbitMqConfiguration>>();
            
            return new RabbitMqProducer(channelManager.GetChannel(), config);
        });
        
        return services;
    }
}