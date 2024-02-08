using Flixer.Catalog.Application;
using Flixer.Catalog.Infra.Data.EF;
using Flixer.Catalog.Api.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddUseCases(); 
builder.Services.AddInfraData(builder.Configuration);
builder.Services.AddAndConfigureControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

var appStartTime = DateTime.UtcNow;

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapGet("/healthz", async context =>
    {
        var currentTime = DateTime.UtcNow;

        if ((currentTime - appStartTime).TotalSeconds < 10)
        {
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("Internal Server Error");
        }
        else
        {
            await context.Response.WriteAsync("OK");
        }
    });
});

app.MapControllers();

app.Run();

public partial class Program
{

}