using Flixer.Catalog.Api.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplication()
    .AddInfraData(builder.Configuration)
    .AddInfraStorage(builder.Configuration)
    .AddMessaging(builder.Configuration)
    .AddSecurityConfiguration(builder.Configuration)
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
app.UseAuthentication();
app.UseAuthorization();
app.UseApplicationHealthCheck();
app.MapControllers();

app.Run();

public partial class Program { }