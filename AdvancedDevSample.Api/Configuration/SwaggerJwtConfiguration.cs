using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AdvancedDevSample.Api.Configuration
{
    public class SwaggerJwtConfiguration : IConfigureOptions<SwaggerGenOptions>
    {
        public void Configure(SwaggerGenOptions options)
        {
            // Configuration Swagger JWT déjà effectuée dans Program.cs
            // Cette classe est conservée pour référence future
        }
    }
}
