using System;
using AdvancedDevSample.Domain.Entities;

namespace AdvancedDevSample.Infrastructure.Repositories
{
    /// <summary>
    /// Entité de persistence simple pour les clients (pour EF / mapping).
    /// </summary>
    public class CustomerEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public bool IsActive { get; set; }

        public CustomerEntity() { }

        public CustomerEntity(Guid id, string name, string email, string? phone, string? address, bool isActive)
        {
            Id = id;
            Name = name;
            Email = email;
            Phone = phone;
            Address = address;
            IsActive = isActive;
        }

        /// <summary>
        /// Convertit l'entité de persistence en objet domaine.
        /// </summary>
        public Customer ToDomain()
        {
            var domain = new Customer(Id, Name, Email, Phone, Address);
            if (!IsActive) domain.Deactivate();
            return domain;
        }

        /// <summary>
        /// Crée une entité de persistence à partir de l'objet domaine.
        /// </summary>
        public static CustomerEntity FromDomain(Customer customer) =>
            new CustomerEntity(customer.Id, customer.Name, customer.Email, customer.Phone, customer.Address, customer.IsActive);
    }
}
