using AdvancedDevSample.Api.Middlewares;
using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using AdvancedDevSample.Domain.Services;
using AdvancedDevSample.Domain.Interfaces; // Ajouté pour IProductRepository
using AdvancedDevSample.Infrastructure.Repositories; // Ajouté pour EfProductRepository
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

// --- AJOUT DES SERVICES AU CONTENEUR (DI) ---

builder.Services.AddControllers();

// Configuration Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var basePath = AppContext.BaseDirectory;
    foreach (var xmlFile in Directory.GetFiles(basePath, "*.xml"))
    {
        options.IncludeXmlComments(xmlFile);
    }
});

// Enregistrement des dépendances de l'application
// C'est ici que l'on lie l'interface à son implémentation pour corriger l'erreur d'exécution
builder.Services.AddScoped<IProductRepository, EfProductRepository>();
builder.Services.AddScoped<ProductService>();


var app = builder.Build();

// --- CONFIGURATION DU PIPELINE HTTP ---

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// Middleware personnalisé pour la gestion des erreurs
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();