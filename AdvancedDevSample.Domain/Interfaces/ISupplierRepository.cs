using System;
using System.Collections.Generic;
using AdvancedDevSample.Domain.Entities;

namespace AdvancedDevSample.Domain.Interfaces
{
    /// <summary>
    /// Contrat de persistance pour les fournisseurs.
    /// </summary>
    public interface ISupplierRepository
    {
        void Add(Supplier supplier);
        Supplier? GetById(Guid id);
        IEnumerable<Supplier> ListAll();
        void Remove(Guid id);
        void Save(Supplier supplier);
    }
}
