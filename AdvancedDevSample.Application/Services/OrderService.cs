using System;
using System.Collections.Generic;
using System.Linq;
using AdvancedDevSample.Application.DTOs;
using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Exceptions;
using AdvancedDevSample.Domain.Interfaces;
using AdvancedDevSample.Domain.ValueObjects;

namespace AdvancedDevSample.Application.Services
{
    /// <summary>
    /// Service applicatif pour gérer les commandes.
    /// Il orchestre le domaine (Order, OrderLine, Price) et la persistance (IOrderRepository),
    /// en récupérant les informations de produits via IProductRepository.
    /// </summary>
    public class OrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;

        public OrderService(IOrderRepository orderRepository, IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
        }

        public OrderDto Create(CreateOrderRequest? request = null)
        {
            var order = new Order(request?.CustomerId);
            _orderRepository.Add(order);
            return MapToDto(order);
        }

        public OrderDto GetById(Guid id)
        {
            var order = GetOrder(id);
            return MapToDto(order);
        }

        public void AddLine(Guid orderId, AddOrderLineRequest request)
        {
            var order = GetOrder(orderId);
            var product = GetProduct(request.ProductId);

            if (!product.IsActive)
            {
                throw new DomainException("Impossible d'ajouter un produit inactif à une commande.");
            }

            var price = product.Price; // Price value object du domaine
            order.AddLine(product.Id, request.Quantity, price);
            _orderRepository.Save(order);
        }

        public void ChangeLineQuantity(Guid orderId, ChangeOrderLineQuantityRequest request)
        {
            var order = GetOrder(orderId);
            order.ChangeQuantity(request.ProductId, request.NewQuantity);
            _orderRepository.Save(order);
        }

        public void RemoveLine(Guid orderId, Guid productId)
        {
            var order = GetOrder(orderId);
            order.RemoveLine(productId);
            _orderRepository.Save(order);
        }

        public void Confirm(Guid orderId)
        {
            var order = GetOrder(orderId);
            order.Confirm();
            _orderRepository.Save(order);
        }

        public void Cancel(Guid orderId)
        {
            var order = GetOrder(orderId);
            order.Cancel();
            _orderRepository.Save(order);
        }

        private Order GetOrder(Guid id)
        {
            return _orderRepository.GetById(id)
                   ?? throw new ApplicationServiceException("Commande non trouvée.");
        }

        private Product GetProduct(Guid id)
        {
            return _productRepository.GetById(id)
                   ?? throw new ApplicationServiceException("Produit non trouvé.");
        }

        private static OrderDto MapToDto(Order order)
        {
            var lineDtos = order.Lines
                .Select(l => new OrderLineDto
                {
                    ProductId = l.ProductId,
                    Quantity = l.Quantity,
                    UnitPrice = l.UnitPrice.Value,
                    LineTotal = l.LineTotal
                })
                .ToList();

            return new OrderDto
            {
                Id = order.Id,
                Status = order.Status.ToString(),
                CreatedAt = order.CreatedAt,
                Total = order.Total,
                CustomerId = order.CustomerId,
                Lines = lineDtos
            };
        }
    }
}

