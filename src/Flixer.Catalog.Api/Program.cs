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

builder.Host.ConfigureApplicationLogging();

var app = builder.Build();

app.UseDocumentation();
app.ConfigureRequestLogging();
app.UseMigrations(app.Services);
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.UseApplicationHealthCheck();
app.MapControllers();

app.Run();