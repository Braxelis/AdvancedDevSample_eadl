using System;
using System.Collections.Generic;
using System.Linq;
using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Interfaces;

namespace AdvancedDevSample.Infrastructure.Repositories
{
    /// <summary>
    /// Implémentation en mémoire d'un repository de commandes.
    /// Sert de simulation de persistance pour les tests et le développement.
    /// </summary>
    public class InMemoryOrderRepository : IOrderRepository
    {
        private static readonly List<Order> _orders = new();

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
            // Pour une implémentation en mémoire, l'objet est déjà dans la liste
            // et modifié par référence, donc rien de spécial à faire ici.
        }
    }
}

