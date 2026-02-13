using System;
using System.Collections.Generic;
using System.Linq;
using AdvancedDevSample.Application.DTOs;
using AdvancedDevSample.Domain.Exceptions;
using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Interfaces;

namespace AdvancedDevSample.Application.Services
{
    /// <summary>
    /// Service applicatif pour gérer les clients.
    /// </summary>
    public class CustomerService
    {
        private readonly ICustomerRepository _repo;

        public CustomerService(ICustomerRepository repo)
        {
            _repo = repo;
        }

        // --------- Cas d'usage : Lister les clients ---------
        public IReadOnlyList<CustomerDto> GetAll()
        {
            var customers = _repo.ListAll();
            return customers
                .Select(MapToDto)
                .ToList();
        }

        // --------- Cas d'usage : Détail client ---------
        public CustomerDto GetById(Guid id)
        {
            var customer = GetCustomer(id);
            return MapToDto(customer);
        }

        // --------- Cas d'usage : Créer un client ---------
        public CustomerDto Create(CreateCustomerRequest request)
        {
            var customer = new Customer(request.Name, request.Email, request.Phone, request.Address);
            _repo.Add(customer);
            return MapToDto(customer);
        }

        // --------- Cas d'usage : Mettre à jour un client ---------
        public void Update(Guid id, UpdateCustomerRequest request)
        {
            var customer = GetCustomer(id);
            customer.UpdateContactInfo(request.Name, request.Email, request.Phone, request.Address);
            _repo.Save(customer);
        }

        // --------- Cas d'usage : Supprimer un client ---------
        public void Delete(Guid id)
        {
            var customer = GetCustomer(id);
            _repo.Remove(id);
        }

        // --------- Cas d'usage : Activer / Désactiver ---------
        public void Activate(Guid id)
        {
            var customer = GetCustomer(id);
            customer.Activate();
            _repo.Save(customer);
        }

        public void Deactivate(Guid id)
        {
            var customer = GetCustomer(id);
            customer.Deactivate();
            _repo.Save(customer);
        }

        // --------- Méthodes privées utilitaires ---------
        private Customer GetCustomer(Guid id)
        {
            return _repo.GetById(id)
                   ?? throw new ApplicationServiceException("Client non trouvé.");
        }

        private static CustomerDto MapToDto(Customer customer) =>
            new CustomerDto
            {
                Id = customer.Id,
                Name = customer.Name,
                Email = customer.Email,
                Phone = customer.Phone,
                Address = customer.Address,
                IsActive = customer.IsActive
            };
    }
}
