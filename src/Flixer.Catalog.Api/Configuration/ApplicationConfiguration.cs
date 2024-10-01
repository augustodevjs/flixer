using Microsoft.AspNetCore.Mvc;
using Flixer.Catalog.Domain.Events;
using Flixer.Catalog.Domain.Contracts;
using Flixer.Catalog.Application.EventHandlers;
using Flixer.Catalog.Application.EventPublisher;

namespace Flixer.Catalog.Api.Configuration;

public static class ApplicationConfiguration
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(o => o.SuppressModelStateInvalidFilter = true);
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(Application.Commands.Category.CreateCategory).Assembly));
        
        services.AddDomainEvents();

        return services;
    }
    
    private static void AddDomainEvents(this IServiceCollection services)
    {
        services.AddTransient<IDomainEventPublisher, DomainEventPublisher>();
        services.AddTransient<IDomainEventHandler<VideoUploadedEvent>, SendToEncoderEventHandler>();
    }
}