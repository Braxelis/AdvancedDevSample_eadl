using System.ComponentModel.DataAnnotations;

namespace AdvancedDevSample.Application.DTOs
{
    public class CreateCustomerRequest
    {
        [Required(ErrorMessage = "Le nom est obligatoire.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "L'email est obligatoire.")]
        [EmailAddress(ErrorMessage = "Le format de l'email est invalide.")]
        public string Email { get; set; } = string.Empty;

        public string? Phone { get; set; }
        public string? Address { get; set; }
    }
}
