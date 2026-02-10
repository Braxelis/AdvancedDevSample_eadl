using System;
using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Exceptions;
using AdvancedDevSample.Domain.ValueObjects;

namespace AdvancedDevSample.Test.Domain.Entities
{
    public class OrderTest
    {
        [Fact]
        public void AddLine_Should_Increase_Total()
        {
            // Arrange
            var order = new Order();

            // Act
            order.AddLine(Guid.NewGuid(), quantity: 2, unitPrice: new Price(10m));

            // Assert
            Assert.Equal(20m, order.Total);
            Assert.Single(order.Lines);
        }

        [Fact]
        public void Confirm_Empty_Order_Should_Throw_DomainException()
        {
            // Arrange
            var order = new Order();

            // Act & Assert
            var ex = Assert.Throws<DomainException>(() => order.Confirm());
            Assert.Equal("Impossible de confirmer une commande vide.", ex.Message);
        }

        [Fact]
        public void ChangeQuantity_Should_Update_Line_And_Total()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var order = new Order();
            order.AddLine(productId, 1, new Price(15m));

            // Act
            order.ChangeQuantity(productId, 3);

            // Assert
            Assert.Equal(45m, order.Total);
        }

        [Fact]
        public void Confirm_Should_Set_Status_To_Confirmed()
        {
            // Arrange
            var order = new Order();
            order.AddLine(Guid.NewGuid(), 1, new Price(5m));

            // Act
            order.Confirm();

            // Assert
            Assert.Equal(OrderStatus.Confirmed, order.Status);
        }
    }
}

