using System;
using System.Collections.Generic;
using System.Linq;
using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Interfaces;

namespace AdvancedDevSample.Test.Application.Fakes
{
    /// <summary>
    /// Fake repository pour les tests du SupplierService.
    /// </summary>
    public class InMemorySupplierRepositoryFake : ISupplierRepository
    {
        private readonly List<Supplier> _suppliers = new();

        public void Add(Supplier supplier)
        {
            _suppliers.Add(supplier);
        }

        public Supplier? GetById(Guid id)
        {
            return _suppliers.FirstOrDefault(s => s.Id == id);
        }

        public IEnumerable<Supplier> ListAll()
        {
            return _suppliers.ToList();
        }

        public void Remove(Guid id)
        {
            var supplier = GetById(id);
            if (supplier != null)
            {
                _suppliers.Remove(supplier);
            }
        }

        public void Save(Supplier supplier)
        {
            var existing = GetById(supplier.Id);
            if (existing == null)
            {
                _suppliers.Add(supplier);
            }
            // Pour un fake, on ne fait rien de plus car l'objet est déjà en mémoire
        }

        /// <summary>
        /// Méthode utilitaire pour les tests : ajouter des données de test.
        /// </summary>
        public void Seed(params Supplier[] suppliers)
        {
            foreach (var supplier in suppliers)
            {
                _suppliers.Add(supplier);
            }
        }
    }
}
