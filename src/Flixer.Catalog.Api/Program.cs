using Flixer.Catalog.Api.Configuration;
using Flixer.Catalog.Application.Extensions;
using Flixer.Catalog.Infra.Data.EF.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplication()
    .AddInfraData(builder.Configuration)
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

if (!app.Environment.IsEnvironment("EndToEndTest"))
{
    app.UseMigrations(app.Services);
}

app.UseCors("*");
app.UseDocumentation();
app.ConfigureRequestLogging();
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.UseApplicationHealthCheck();
app.MapControllers();

app.Run();

public partial class Program { }