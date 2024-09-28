using Flixer.Catalog.Api.Configuration;
using Flixer.Catalog.Application.Extensions;
using Flixer.Catalog.Infra.Data.EF.Extensions;
using Flixer.Catalog.Infra.Storage.Extensions;
using Flixer.Catalog.Infra.Messaging.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplication()
    .AddMessaging(builder.Configuration)
    .AddInfraData(builder.Configuration)
    .AddInfraStorage(builder.Configuration)
    .AddAndConfigureControllers()
    .AddHealthChecks()
    .ConfigureApplicationHealthChecks(builder.Configuration);

builder
    .Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
    .AddEnvironmentVariables();

builder.Host.ConfigureApplicationLogging();

var app = builder.Build();

app.UseCors("*");
app.UseDocumentation();
app.UseMigrations(app.Services);
app.ConfigureRequestLogging();
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.UseApplicationHealthCheck();
app.MapControllers();

app.Run();

public partial class Program { }