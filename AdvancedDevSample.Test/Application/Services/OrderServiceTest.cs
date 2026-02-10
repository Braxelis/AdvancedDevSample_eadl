using System;
using AdvancedDevSample.Application.DTOs;
using AdvancedDevSample.Domain.Services;
using AdvancedDevSample.Test.Application.Fakes;

namespace AdvancedDevSample.Test.Application.Services
{
    public class OrderServiceTest
    {
        private readonly InMemoryOrderRepositoryFake _repo;
        private readonly InMemoryProductRepositoryFake _productRepo;
        private readonly OrderService _service;

        public OrderServiceTest()
        {
            _repo = new InMemoryOrderRepositoryFake();
            _productRepo = new InMemoryProductRepositoryFake();
            _service = new OrderService(_repo, _productRepo);
        }

        [Fact]
        public void Create_Should_Return_Draft_OrderDto()
        {
            // Act
            var dto = _service.Create(new CreateOrderRequest());

            // Assert
            Assert.NotEqual(Guid.Empty, dto.Id);
            Assert.Equal("Draft", dto.Status);
            Assert.Equal(0m, dto.Total);
        }

        [Fact]
        public void AddLine_Should_Update_Total()
        {
            // Arrange
            var dto = _service.Create(new CreateOrderRequest());
            var orderId = dto.Id;
            var product = _productRepo.SeedActiveProduct(10m);

            // Act
            _service.AddLine(orderId, new AddOrderLineRequest
            {
                ProductId = product.Id,
                Quantity = 2
            });

            var updated = _service.GetById(orderId);

            // Assert
            Assert.Equal(20m, updated.Total);
            Assert.Single(updated.Lines);
        }

        [Fact]
        public void Confirm_Should_Change_Status_To_Confirmed()
        {
            // Arrange
            var dto = _service.Create(new CreateOrderRequest());
            var orderId = dto.Id;
            var product = _productRepo.SeedActiveProduct(5m);

            _service.AddLine(orderId, new AddOrderLineRequest
            {
                ProductId = product.Id,
                Quantity = 1
            });

            // Act
            _service.Confirm(orderId);
            var updated = _service.GetById(orderId);

            // Assert
            Assert.Equal("Confirmed", updated.Status);
        }
    }
}

