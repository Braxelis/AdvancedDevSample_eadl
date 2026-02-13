using System;
using System.Collections.Generic;
using System.Linq;
using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Interfaces;

namespace AdvancedDevSample.Test.Application.Fakes
{
    /// <summary>
    /// Fake repository pour les tests du CustomerService.
    /// </summary>
    public class InMemoryCustomerRepositoryFake : ICustomerRepository
    {
        private readonly List<Customer> _customers = new();

        public void Add(Customer customer)
        {
            _customers.Add(customer);
        }

        public Customer? GetById(Guid id)
        {
            return _customers.FirstOrDefault(c => c.Id == id);
        }

        public IEnumerable<Customer> ListAll()
        {
            return _customers.ToList();
        }

        public void Remove(Guid id)
        {
            var customer = GetById(id);
            if (customer != null)
            {
                _customers.Remove(customer);
            }
        }

        public void Save(Customer customer)
        {
            var existing = GetById(customer.Id);
            if (existing == null)
            {
                _customers.Add(customer);
            }
            // Pour un fake, on ne fait rien de plus car l'objet est déjà en mémoire
        }

        /// <summary>
        /// Méthode utilitaire pour les tests : ajouter des données de test.
        /// </summary>
        public void Seed(params Customer[] customers)
        {
            foreach (var customer in customers)
            {
                _customers.Add(customer);
            }
        }
    }
}
