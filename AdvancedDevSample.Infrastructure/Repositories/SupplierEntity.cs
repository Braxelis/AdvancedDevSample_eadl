using System;
using AdvancedDevSample.Domain.Entities;

namespace AdvancedDevSample.Infrastructure.Repositories
{
    /// <summary>
    /// Entité de persistence simple pour les fournisseurs (pour EF / mapping).
    /// </summary>
    public class SupplierEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public bool IsActive { get; set; }

        public SupplierEntity() { }

        public SupplierEntity(Guid id, string name, string email, string? phone, string? address, bool isActive)
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
        public Supplier ToDomain()
        {
            var domain = new Supplier(Id, Name, Email, Phone, Address);
            if (!IsActive) domain.Deactivate();
            return domain;
        }

        /// <summary>
        /// Crée une entité de persistence à partir de l'objet domaine.
        /// </summary>
        public static SupplierEntity FromDomain(Supplier supplier) =>
            new SupplierEntity(supplier.Id, supplier.Name, supplier.Email, supplier.Phone, supplier.Address, supplier.IsActive);
    }
}
