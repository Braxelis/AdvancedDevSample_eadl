using System;
using System.Collections.Generic;
using System.Linq;
using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Exceptions;
using AdvancedDevSample.Domain.Interfaces;
using AdvancedDevSample.Domain.ValueObjects;
using AdvancedDevSample.Application.DTOs;

namespace AdvancedDevSample.Application.Services
{
    public class ProductService
    {
        private readonly IProductRepository _repo;

        public ProductService(IProductRepository repo)
        {
            _repo = repo;
        }

        // --------- Cas d'usage : Lister les produits ---------
        public IReadOnlyList<ProductDto> GetAll()
        {
            var products = _repo.ListAll();
            return products
                .Select(MapToDto)
                .ToList();
        }

        // --------- Cas d'usage : Détail produit ---------
        public ProductDto GetById(Guid id)
        {
            var product = GetProduct(id);
            return MapToDto(product);
        }

        // --------- Cas d'usage : Créer un produit ---------
        public ProductDto Create(CreateProductRequest request)
        {
            var price = new Price(request.InitialPrice);
            var product = new Product(price, request.SupplierId); // actif par défaut

            _repo.Add(product);

            return MapToDto(product);
        }

        // --------- Cas d'usage : Modifier le prix ---------
        public void ChangePrice(Guid id, ChangePriceRequest request)
        {
            var product = GetProduct(id);
            product.ChangePrice(request.NewPrice);
            _repo.Save(product);
        }

        // --------- Cas d'usage : Activer / Désactiver ---------
        public void Activate(Guid id)
        {
            var product = GetProduct(id);
            product.Activate();
            _repo.Save(product);
        }

        public void Deactivate(Guid id)
        {
            var product = GetProduct(id);
            product.Deactivate();
            _repo.Save(product);
        }

        // --------- Méthodes privées utilitaires ---------
        private Product GetProduct(Guid id)
        {
            return _repo.GetById(id)
                   ?? throw new ApplicationServiceException("Produit non trouvé.");
        }

        private static ProductDto MapToDto(Product product) =>
            new ProductDto
            {
                Id = product.Id,
                Price = product.Price.Value,
                IsActive = product.IsActive,
                SupplierId = product.SupplierId
            };
    }
}