using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AdvancedDevSample.Api.Configuration
{
    public class SwaggerJwtConfiguration : IConfigureOptions<SwaggerGenOptions>
    {
        public void Configure(SwaggerGenOptions options)
        {
            // Utilisation de types dynamiques pour éviter les problèmes de namespace
            var securityScheme = new
            {
                Name = "Authorization",
                Type = "http",
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = "header",
                Description = "Entrez 'Bearer' suivi de votre token JWT. Exemple: Bearer eyJhbGc..."
            };

            // Cette approche utilise la réflexion pour contourner les problèmes de types
            var addSecurityDefinitionMethod = typeof(SwaggerGenOptions).GetMethod("AddSecurityDefinition");
            if (addSecurityDefinitionMethod != null)
            {
                // On ne peut pas utiliser cette approche facilement
                // Revenons à une solution plus simple
            }
        }
    }
}
