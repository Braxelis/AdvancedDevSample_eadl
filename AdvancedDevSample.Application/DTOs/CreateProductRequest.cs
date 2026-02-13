using System.ComponentModel.DataAnnotations;

namespace AdvancedDevSample.Application.DTOs
{
    /// <summary>
    /// Corps de requête pour créer un produit.
    /// </summary>
    public class CreateProductRequest
    {
        [Range(0.01, double.MaxValue, ErrorMessage = "Le prix doit être strictement positif.")]
        public decimal InitialPrice { get; set; }
        
        public Guid? SupplierId { get; set; }
    }
}