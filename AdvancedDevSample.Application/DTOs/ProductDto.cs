using System;

namespace AdvancedDevSample.Application.DTOs
{
    /// <summary>
    /// Représentation d'un produit pour l'API.
    /// </summary>
    public class ProductDto
    {
        public Guid Id { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
    }
}