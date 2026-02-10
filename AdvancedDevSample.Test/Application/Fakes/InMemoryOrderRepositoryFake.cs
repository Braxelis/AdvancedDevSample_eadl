using System;
using System.Collections.Generic;
using System.Linq;
using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Interfaces;

namespace AdvancedDevSample.Test.Application.Fakes
{
    /// <summary>
    /// Implémentation de test d'IOrderRepository pour tester OrderService sans dépendre de l'infrastructure.
    /// </summary>
    public class InMemoryOrderRepositoryFake : IOrderRepository
    {
        private readonly List<Order> _orders = new();

        public void Add(Order order)
        {
            _orders.Add(order);
        }

        public Order? GetById(Guid id)
        {
            return _orders.FirstOrDefault(o => o.Id == id);
        }

        public IEnumerable<Order> ListAll()
        {
            return _orders.ToList();
        }

        public void Remove(Guid id)
        {
            var existing = _orders.FirstOrDefault(o => o.Id == id);
            if (existing is not null)
            {
                _orders.Remove(existing);
            }
        }

        public void Save(Order order)
        {
            // Rien de spécial à faire, l'ordre est déjà suivi par référence.
        }
    }
}

