using System;
using AdvancedDevSample.Domain.Exceptions;
using AdvancedDevSample.Domain.ValueObjects;

namespace AdvancedDevSample.Domain.Entities
{
    public class Product
    {
        /// <summary>
        /// Représente un produit vendable.
        /// </summary>
        public Guid Id { get; private set; } // Identité
        public Price Price { get; private set; } // Invariant encapsulé dans Price
        public bool IsActive { get; private set; } // true par défaut

        // Constructeur principal
        public Product(Guid id, Price price, bool isActive = true)
        {
            Id = id == Guid.Empty ? Guid.NewGuid() : id;
            Price = price; // Price valide par construction (value object)
            IsActive = isActive;
        }

        // Constructeur pratique : nouvelle entité avec Id généré
        public Product(Price price) : this(Guid.NewGuid(), price, true) { }

        // Constructeur requis par certains ORMs ; protégé pour empêcher l'utilisation publique.
        protected Product()
        {
            // L'ORM pourra initialiser les propriétés
        }

        public void ChangePrice(Price newPrice)
        {
            // Règle métier : le produit ne doit pas être inactif
            if (!IsActive)
            {
                throw new DomainException("Le produit est inactif.");
            }

            // Invariant déjà garanti par Price
            Price = newPrice;
        }

        // Surcharge attendue par l'application (DTO fournit un decimal)
        public void ChangePrice(decimal newPriceValue)
        {
            var newPrice = new Price(newPriceValue); // peut lancer DomainException si <= 0
            ChangePrice(newPrice);
        }

        public void Deactivate() => IsActive = false;
        public void Activate() => IsActive = true;
    }
}