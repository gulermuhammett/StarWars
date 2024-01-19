using Microsoft.OpenApi.Models;
using StarWars.API.Entities;
using StarWars.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "StarWars", Version = "v1" });
});
builder.Services.AddHttpClient<GenericService<People>>();
builder.Services.AddHttpClient<GenericService<Films>>();
builder.Services.AddHttpClient<GenericService<Planets>>();
builder.Services.AddHttpClient<GenericService<Species>>();
builder.Services.AddHttpClient<GenericService<Starships>>();
builder.Services.AddHttpClient<GenericService<Vehicles>>();
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "StarWars v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
