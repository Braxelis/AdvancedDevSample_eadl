using System;
using System.Collections.Generic;
using System.Linq;
using AdvancedDevSample.Domain.Exceptions;
using AdvancedDevSample.Domain.ValueObjects;

namespace AdvancedDevSample.Domain.Entities
{
    /// <summary>
    /// Représente une commande client contenant une ou plusieurs lignes de produits.
    /// Invariants principaux :
    /// - Une commande Confirmed ou Cancelled n'est plus modifiable.
    /// - Chaque ligne a une quantité strictement positive et un prix unitaire valide.
    /// </summary>
    public class Order
    {
        private readonly List<OrderLine> _lines = new();

        public Guid Id { get; private set; }

        public IReadOnlyCollection<OrderLine> Lines => _lines.AsReadOnly();

        public OrderStatus Status { get; private set; }

        public DateTime CreatedAt { get; private set; }

        public decimal Total => _lines.Sum(l => l.LineTotal);

        public Guid? CustomerId { get; private set; } // Relation optionnelle vers un client

        public Order(Guid id, Guid? customerId = null)
        {
            Id = id == Guid.Empty ? Guid.NewGuid() : id;
            Status = OrderStatus.Draft;
            CreatedAt = DateTime.UtcNow;
            CustomerId = customerId;
        }

        public Order(Guid? customerId = null) : this(Guid.NewGuid(), customerId)
        {
        }

        private void EnsureIsMutable()
        {
            if (Status is OrderStatus.Confirmed or OrderStatus.Cancelled)
            {
                throw new DomainException("La commande n'est plus modifiable.");
            }
        }

        public void AddLine(Guid productId, int quantity, Price unitPrice)
        {
            EnsureIsMutable();

            var line = new OrderLine(productId, quantity, unitPrice);
            _lines.Add(line);
        }

        public void RemoveLine(Guid productId)
        {
            EnsureIsMutable();

            var existing = _lines.FirstOrDefault(l => l.ProductId == productId);
            if (existing.Equals(default(OrderLine)))
            {
                throw new DomainException("Ligne de commande introuvable pour ce produit.");
            }

            _lines.Remove(existing);
        }

        public void ChangeQuantity(Guid productId, int newQuantity)
        {
            EnsureIsMutable();

            if (newQuantity <= 0)
            {
                throw new DomainException("La quantité doit être strictement positive.");
            }

            var index = _lines.FindIndex(l => l.ProductId == productId);
            if (index < 0)
            {
                throw new DomainException("Ligne de commande introuvable pour ce produit.");
            }

            var existing = _lines[index];
            _lines[index] = new OrderLine(existing.ProductId, newQuantity, existing.UnitPrice);
        }

        public void Confirm()
        {
            if (!_lines.Any())
            {
                throw new DomainException("Impossible de confirmer une commande vide.");
            }

            if (Status != OrderStatus.Draft)
            {
                throw new DomainException("Seules les commandes en brouillon peuvent être confirmées.");
            }

            Status = OrderStatus.Confirmed;
        }

        public void SetCustomer(Guid? customerId)
        {
            EnsureIsMutable();
            CustomerId = customerId;
        }

        public void Cancel()
        {
            if (Status == OrderStatus.Cancelled)
            {
                return;
            }

            if (Status == OrderStatus.Confirmed)
            {
                // Selon le choix métier, on pourrait interdire l'annulation après confirmation.
                // Ici, on l'autorise mais on pourrait lever une DomainException si besoin.
            }

            Status = OrderStatus.Cancelled;
        }
    }

    public enum OrderStatus
    {
        Draft = 0,
        Confirmed = 1,
        Cancelled = 2
    }
}

