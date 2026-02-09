// Ancienne implémentation de repository async utilisée par des tests d'intégration obsolètes.
// Désactivée pour le moment afin de simplifier le projet et éviter les erreurs de compilation
// liées à des types qui n'existent plus (IProductRepositoryAsync, Product2, etc.).

#if false
using AdvancedDevSample.Domain.Entities;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedDevSample.Api.Tests.Integration
{
    public class InMemoryProductRepositoryAsync : IProductRepositoryAsync
    {
        private readonly Dictionary<Guid, Product2> _store = new();

        public Task<Product2?> GetByIdAsync(Guid id)

            => Task.FromResult(_store.TryGetValue(id, out var p) ? p : null);


        public Task SaveAsync(Product2 product)
        {
            _store[product.Id] = product;
            return Task.CompletedTask;
        }

        //Helper pour initialiser les tests
        public void Seed(Product2 product)
        {
            _store[product.Id] = product;
        }
    }
}
#endif
