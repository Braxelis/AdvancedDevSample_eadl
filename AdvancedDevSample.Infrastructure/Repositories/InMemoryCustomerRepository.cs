using System;
using System.Collections.Generic;
using System.Linq;
using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Interfaces;

namespace AdvancedDevSample.Infrastructure.Repositories
{
    /// <summary>
    /// Implémentation en mémoire d'un repository de clients.
    /// Sert de simulation de base de données.
    /// </summary>
    public class InMemoryCustomerRepository : ICustomerRepository
    {
        // "Table" de clients en mémoire.
        private static readonly List<CustomerEntity> _storage = new();

        public void Add(Customer customer)
        {
            var entity = CustomerEntity.FromDomain(customer);
            _storage.Add(entity);
        }

        public Customer? GetById(Guid id)
        {
            var entity = _storage.FirstOrDefault(c => c.Id == id);

            if (entity is null)
            {
                return null;
            }

            return entity.ToDomain();
        }

        public IEnumerable<Customer> ListAll()
        {
            return _storage.Select(e => e.ToDomain()).ToList();
        }

        public void Remove(Guid id)
        {
            var entity = _storage.FirstOrDefault(c => c.Id == id);
            if (entity is not null)
            {
                _storage.Remove(entity);
            }
        }

        public void Save(Customer customer)
        {
            // Ici on simule la sauvegarde :
            var existing = _storage.FirstOrDefault(c => c.Id == customer.Id);
            var entity = CustomerEntity.FromDomain(customer);

            if (existing is null)
            {
                _storage.Add(entity);
            }
            else
            {
                existing.Name = entity.Name;
                existing.Email = entity.Email;
                existing.Phone = entity.Phone;
                existing.Address = entity.Address;
                existing.IsActive = entity.IsActive;
            }

            Console.WriteLine($"Client avec ID {customer.Id} sauvegardé : {customer.Name}.");
        }
    }
}
