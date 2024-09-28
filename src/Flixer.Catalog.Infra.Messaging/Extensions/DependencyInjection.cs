using RabbitMQ.Client;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Flixer.Catalog.Application.Intefaces;
using Flixer.Catalog.Infra.Messaging.Producer;
using Microsoft.Extensions.DependencyInjection;
using Flixer.Catalog.Infra.Messaging.Configuration;

namespace Flixer.Catalog.Infra.Messaging.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddMessaging(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RabbitMqConfiguration>(configuration.GetSection("RabbitMqConfiguration"));

        services.AddSingleton(sp =>
        {
            var config = sp.GetRequiredService<IOptions<RabbitMqConfiguration>>().Value;
            
            var factory = new ConnectionFactory
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
            var config = sp.GetRequiredService<IOptions<RabbitMqConfiguration>>();
            
            return new RabbitMqProducer(channelManager.GetChannel(), config);
        });
        
        return services;
    }
}