using System;
using System.Collections.Generic;
using AdvancedDevSample.Domain.Entities;

namespace AdvancedDevSample.Domain.Interfaces
{
    /// <summary>
    /// Contrat de persistance pour les clients.
    /// </summary>
    public interface ICustomerRepository
    {
        void Add(Customer customer);
        Customer? GetById(Guid id);
        IEnumerable<Customer> ListAll();
        void Remove(Guid id);
        void Save(Customer customer);
    }
}
