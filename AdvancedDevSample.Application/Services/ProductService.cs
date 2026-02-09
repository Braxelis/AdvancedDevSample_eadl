using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Exceptions;
using AdvancedDevSample.Domain.Interfaces;
using AdvancedDevSample.Application.DTOs;


namespace AdvancedDevSample.Domain.Services
{
    public class ProductService
    {
        private readonly IProductRepository _repo;

        public ProductService(IProductRepository repo)
        {
            _repo = repo;
        }

        public void ChangeProductPrice(Guid productId, decimal newPriceValue)
        {
            var product = GetProduct(productId);

            product.ChangePrice(newPriceValue);

            _repo.Save(product);
        }

        private Product GetProduct(Guid id)
        {
            return _repo.GetById(id)
                ?? throw new ApplicationServiceException("Produit non trouvé.");
        }

        public void ChangePrice(Guid id, ChangePriceRequest request)
        {
            var product = GetProduct(id);
            product.ChangePrice(request.NewPrice);
            _repo.Save(product);
        }
    }
}