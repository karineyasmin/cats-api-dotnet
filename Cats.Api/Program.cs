using Cats.Api.Database;
using Cats.Api.Services;
using Cats.Api.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("secrets.json", optional: true, reloadOnChange: true);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), 
    new MySqlServerVersion(new Version(8, 0, 21)),
    mySqlOptions => mySqlOptions.EnableRetryOnFailure())); // Adicionado EnableRetryOnFailure()

builder.Services.AddScoped<ICatRepository, CatRepository>();
builder.Services.AddScoped<CatService>();
builder.Services.AddHttpClient<TheCatApiService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Adicione a configuração de versão de API
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});

// Adicione a configuração do Swagger
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Cats API",
        Version = "v1",
        Description = "API for managing cats",
        Contact = new OpenApiContact
        {
            Name = "Karine Ribeiro",
            Email = "karine.ribeiro@gft.com",
            Url = new Uri("https://www.github.com/karineyasmin")
        },
        License = new OpenApiLicense
        {
            Name = "MIT License",
            Url = new Uri("https://opensource.org/licenses/MIT")
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Cats API v1");
        options.RoutePrefix = string.Empty; // Para abrir o Swagger na raiz
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();