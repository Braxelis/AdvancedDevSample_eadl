using Microsoft.VisualStudio.TestPlatform.TestHost;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Supprimer le vrai repository enregistré dans Program.cs
            services.RemoveAll(typeof(IProductRepositoryAsync));

            // Ajouter la version In-Memory pour les tests
            services.AddSingleton<IProductRepositoryAsync, InMemoryProductRepositoryAsync>();
        });
    }
}