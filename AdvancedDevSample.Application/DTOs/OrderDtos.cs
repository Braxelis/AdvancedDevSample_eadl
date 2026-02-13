using System;
using System.Collections.Generic;

namespace AdvancedDevSample.Application.DTOs
{
    public class OrderLineDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal { get; set; }
    }

    public class OrderDto
    {
        public Guid Id { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public decimal Total { get; set; }
        public Guid? CustomerId { get; set; }
        public IReadOnlyList<OrderLineDto> Lines { get; set; } = Array.Empty<OrderLineDto>();
    }

    public class CreateOrderRequest
    {
        // Pour l'instant, on crée une commande vide (pas de champs nécessaires).
        public Guid? CustomerId { get; set; }
    }

    public class AddOrderLineRequest
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class ChangeOrderLineQuantityRequest
    {
        public Guid ProductId { get; set; }
        public int NewQuantity { get; set; }
    }
}

