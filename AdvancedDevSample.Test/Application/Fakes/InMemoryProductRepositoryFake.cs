using System;
using System.Collections.Generic;
using System.Linq;
using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Interfaces;
using AdvancedDevSample.Domain.ValueObjects;

namespace AdvancedDevSample.Test.Application.Fakes
{
    /// <summary>
    /// Faux repository de produits pour tester OrderService.
    /// </summary>
    public class InMemoryProductRepositoryFake : IProductRepository
    {
        private readonly List<Product> _products = new();

        public void Add(Product product)
        {
            _products.Add(product);
        }

        public Product? GetById(Guid id)
        {
            return _products.FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<Product> ListAll()
        {
            return _products.ToList();
        }

        public void Remove(Guid id)
        {
            var existing = _products.FirstOrDefault(p => p.Id == id);
            if (existing is not null)
            {
                _products.Remove(existing);
            }
        }

        public void Save(Product product)
        {
            // Rien de spécial à faire en mémoire.
        }

        public Product SeedActiveProduct(decimal price)
        {
            var product = new Product(new Price(price));
            Add(product);
            return product;
        }
    }
}

