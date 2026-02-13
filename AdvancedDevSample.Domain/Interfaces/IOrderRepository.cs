using System;
using System.Collections.Generic;
using AdvancedDevSample.Domain.Entities;

namespace AdvancedDevSample.Domain.Interfaces
{
    /// <summary>
    /// Contrat de persistance pour les commandes.
    /// </summary>
    public interface IOrderRepository
    {
        void Add(Order order);
        Order? GetById(Guid id);
        IEnumerable<Order> ListAll();
        void Remove(Guid id);
        void Save(Order order);
    }
}

