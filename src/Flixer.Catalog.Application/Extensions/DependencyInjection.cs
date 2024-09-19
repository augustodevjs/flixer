using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Flixer.Catalog.Domain.Events;
using Flixer.Catalog.Domain.Contracts;
using Flixer.Catalog.Application.EventHandlers;
using Microsoft.Extensions.DependencyInjection;
using Flixer.Catalog.Application.EventPublisher;
using Flixer.Catalog.Application.Intefaces;

namespace Flixer.Catalog.Application.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(o => o.SuppressModelStateInvalidFilter = true);
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        
        services.AddDomainEvents();

        return services;
    }
    
    private static IServiceCollection AddDomainEvents(
        this IServiceCollection services)
    {
        services.AddTransient<IDomainEventPublisher, DomainEventPublisher>();
        services.AddTransient<IDomainEventHandler<VideoUploadedEvent>,
            SendToEncoderEventHandler>();
        
        return services;
    }
}
