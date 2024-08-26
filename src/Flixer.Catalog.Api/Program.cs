using Serilog;
using Flixer.Catalog.Application;
using Flixer.Catalog.Infra.Data.EF;
using Flixer.Catalog.Api.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplication()
    .ConfigureCulture()
    .AddInfraData(builder.Configuration)
    .AddAndConfigureControllers()
    .AddHealthChecks()
    .ConfigureApplicationHealthChecks(builder.Configuration);

builder.Host.ConfigureApplicationLogging();

var app = builder.Build();

app.UseSerilogRequestLogging(o =>
{
    o.IncludeQueryInRequestPath = true;
});

app.UseMigrations(app.Services);
app.UseConfiguredRequestLocalization();
app.UseDocumentation();
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.UseApplicationHealthCheck();
app.MapControllers();

app.Run();