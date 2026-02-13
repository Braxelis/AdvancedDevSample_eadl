using System;
using System.Collections.Generic;
using System.Linq;
using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Interfaces;

namespace AdvancedDevSample.Infrastructure.Repositories
{
    /// <summary>
    /// Implémentation en mémoire d'un repository de fournisseurs.
    /// Sert de simulation de base de données.
    /// </summary>
    public class InMemorySupplierRepository : ISupplierRepository
    {
        // "Table" de fournisseurs en mémoire.
        private static readonly List<SupplierEntity> _storage = new();

        public void Add(Supplier supplier)
        {
            var entity = SupplierEntity.FromDomain(supplier);
            _storage.Add(entity);
        }

        public Supplier? GetById(Guid id)
        {
            var entity = _storage.FirstOrDefault(s => s.Id == id);

            if (entity is null)
            {
                return null;
            }

            return entity.ToDomain();
        }

        public IEnumerable<Supplier> ListAll()
        {
            return _storage.Select(e => e.ToDomain()).ToList();
        }

        public void Remove(Guid id)
        {
            var entity = _storage.FirstOrDefault(s => s.Id == id);
            if (entity is not null)
            {
                _storage.Remove(entity);
            }
        }

        public void Save(Supplier supplier)
        {
            // Ici on simule la sauvegarde :
            var existing = _storage.FirstOrDefault(s => s.Id == supplier.Id);
            var entity = SupplierEntity.FromDomain(supplier);

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

            Console.WriteLine($"Fournisseur avec ID {supplier.Id} sauvegardé : {supplier.Name}.");
        }
    }
}
