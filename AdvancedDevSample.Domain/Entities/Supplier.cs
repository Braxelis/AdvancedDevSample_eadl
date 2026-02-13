using System;
using AdvancedDevSample.Domain.Exceptions;

namespace AdvancedDevSample.Domain.Entities
{
    /// <summary>
    /// Représente un fournisseur qui fournit des produits.
    /// Invariants:
    /// - Name ne peut pas être vide
    /// - Email doit avoir un format valide
    /// </summary>
    public class Supplier
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string? Phone { get; private set; }
        public string? Address { get; private set; }
        public bool IsActive { get; private set; }

        // Constructeur principal
        public Supplier(Guid id, string name, string email, string? phone = null, string? address = null, bool isActive = true)
        {
            Id = id == Guid.Empty ? Guid.NewGuid() : id;
            
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new DomainException("Le nom du fournisseur ne peut pas être vide.");
            }
            
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new DomainException("L'email du fournisseur ne peut pas être vide.");
            }
            
            if (!IsValidEmail(email))
            {
                throw new DomainException("Le format de l'email est invalide.");
            }

            Name = name;
            Email = email;
            Phone = phone;
            Address = address;
            IsActive = isActive;
        }

        // Constructeur pratique : nouvelle entité avec Id généré
        public Supplier(string name, string email, string? phone = null, string? address = null) 
            : this(Guid.NewGuid(), name, email, phone, address, true) 
        { }

        // Constructeur requis par certains ORMs ; protégé pour empêcher l'utilisation publique.
        protected Supplier()
        {
            // L'ORM pourra initialiser les propriétés
            Name = string.Empty;
            Email = string.Empty;
        }

        public void UpdateContactInfo(string name, string email, string? phone, string? address)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new DomainException("Le nom du fournisseur ne peut pas être vide.");
            }
            
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new DomainException("L'email du fournisseur ne peut pas être vide.");
            }
            
            if (!IsValidEmail(email))
            {
                throw new DomainException("Le format de l'email est invalide.");
            }

            Name = name;
            Email = email;
            Phone = phone;
            Address = address;
        }

        public void Activate() => IsActive = true;
        public void Deactivate() => IsActive = false;

        private static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
