using System;
using System.Collections.Generic;
using System.Linq;
using AdvancedDevSample.Application.DTOs;
using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Exceptions;
using AdvancedDevSample.Domain.Interfaces;
using AdvancedDevSample.Domain.ValueObjects;

namespace AdvancedDevSample.Domain.Services
{
    /// <summary>
    /// Service applicatif pour gérer les commandes.
    /// Il orchestre le domaine (Order, OrderLine, Price) et la persistance (IOrderRepository).
    /// </summary>
    public class OrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public OrderDto Create(CreateOrderRequest? request = null)
        {
            var order = new Order();
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
            var price = new Price(request.UnitPrice);
            order.AddLine(request.ProductId, request.Quantity, price);
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
                Lines = lineDtos
            };
        }
    }
}

