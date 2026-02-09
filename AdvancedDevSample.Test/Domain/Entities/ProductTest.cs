using System;
using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Exceptions;
using AdvancedDevSample.Domain.ValueObjects;
using Xunit;

namespace AdvancedDevSample.Test.Domain.Entities
{
    public class ProductTest
    {
        [Fact]
        public void ChangePrice_Should_Update_Price_When_Product_Is_Active()
        {
            // Arrange : produit actif avec un prix initial
            var product = new Product(new Price(10m));

            // Act : changement de prix
            product.ChangePrice(20m);

            // Assert : le prix a bien été mis à jour
            Assert.Equal(20m, product.Price.Value);
        }

        [Fact]
        public void ChangePrice_Should_Throw_Exception_When_Product_Is_Inactive()
        {
            // Arrange : produit inactif
            var product = new Product(Guid.NewGuid(), new Price(10m), isActive: false);

            // Act & Assert : le changement de prix doit lever une DomainException
            var exception = Assert.Throws<DomainException>(() => product.ChangePrice(20m));

            Assert.Equal("Le produit est inactif.", exception.Message);
        }
    }
}