using System;
using System.Collections.Generic;
using System.Linq;
using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Interfaces;
using AdvancedDevSample.Domain.ValueObjects;

namespace AdvancedDevSample.Infrastructure.Repositories
{
    /// <summary>
    /// Implémentation en mémoire d'un repository de produits.
    /// Sert de simulation de base de données.
    /// </summary>
    public class EfProductRepository : IProductRepository
    {
        // "Table" de produits en mémoire.
        private static readonly List<ProductEntity> _storage = new();

        public void Add(Product product)
        {
            var entity = ProductEntity.FromDomain(product);
            _storage.Add(entity);
        }

        public Product GetById(Guid id)
        {
            var entity = _storage.FirstOrDefault(p => p.Id == id);

            if (entity is null)
            {
                throw new KeyNotFoundException($"Product with ID {id} not found.");
            }

            return entity.ToDomain();
        }

        public IEnumerable<Product> ListAll()
        {
            return _storage.Select(e => e.ToDomain()).ToList();
        }

        public void Remove(Guid id)
        {
            var entity = _storage.FirstOrDefault(p => p.Id == id);
            if (entity is not null)
            {
                _storage.Remove(entity);
            }
        }

        public void Save(Product product)
        {
            // Ici on simule la sauvegarde :
            var existing = _storage.FirstOrDefault(p => p.Id == product.Id);
            var entity = ProductEntity.FromDomain(product);

            if (existing is null)
            {
                _storage.Add(entity);
            }
            else
            {
                existing.Price = entity.Price;
                existing.IsActive = entity.IsActive;
            }

            Console.WriteLine($"Produit avec ID {product.Id} sauvegardé avec le prix {product.Price}.");
        }
    }
}