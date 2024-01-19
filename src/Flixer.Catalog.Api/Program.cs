using Flixer.Catalog.Application;
using Flixer.Catalog.Infra.Data.EF;
using Flixer.Catalog.Api.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddUseCases();
builder.Services.AddInfraData();
builder.Services.AddAndConfigureControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program
{

}
