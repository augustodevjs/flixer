using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Flixer.Catalog.Application.UseCases.Category;

namespace Flixer.Catalog.Application;

public static class DependecyInjection
{
    public static void AddUseCases(this IServiceCollection services)
    {
        services.AddMediatR(typeof(CreateCategory));
    }
}