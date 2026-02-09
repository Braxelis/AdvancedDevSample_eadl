using System;
using AdvancedDevSample.Domain.Exceptions;
using AdvancedDevSample.Domain.ValueObjects;

namespace AdvancedDevSample.Domain.ValueObjects
{
    /// <summary>
    /// Ligne de commande : produit + quantité + prix unitaire.
    /// Invariants :
    /// - Quantity > 0
    /// - UnitPrice est un Price valide (strictement positif)
    /// </summary>
    public readonly record struct OrderLine
    {
        public Guid ProductId { get; init; }
        public int Quantity { get; init; }
        public Price UnitPrice { get; init; }

        public decimal LineTotal => UnitPrice.Value * Quantity;

        public OrderLine(Guid productId, int quantity, Price unitPrice)
        {
            if (productId == Guid.Empty)
            {
                throw new DomainException("Une ligne de commande doit référencer un produit valide.");
            }

            if (quantity <= 0)
            {
                throw new DomainException("La quantité doit être strictement positive.");
            }

            ProductId = productId;
            Quantity = quantity;
            UnitPrice = unitPrice; // Price garantit déjà le prix > 0
        }
    }
}

