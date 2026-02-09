using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Execptions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace AdvancedDevSample.Test.Domain.Entities
{
    public class ProductTest
    {
        [Fact]
        public void ChangePrice_Shoulf_Update_Price_When_Product_Is_Active(){
            //Arrange : Je prépare un produit valider
            var product = new Product();

            //Act : Execute une action
            product.ChangePrice(20);

            //Assert : Vérifie le résultat
            Assert.Equal(20, product.Price);

        }

        [Fact]
        public void ChangePrice_Should_Throw_Execption_When_Product_Is_Inactive()
        {
            //Arrange : Je prépare un produit valider
            var product = new Product();
            product.ChangePrice(10); // Prix initial

            // Simulation : produit désactivé (via reconstitution ou méthode dédiée)
            //product.IsActive=true;//Accesseur non accessible
            typeof(Product).GetProperty(nameof(Product.IsActive))!.SetValue(product, false);

            //Act & Assert
            var exception = Assert.Throws<DomainException>(() => product.ChangePrice(20));

            Assert.Equal("Impossible de modifier un produit inactif.", exception.Message);

        }

        [Fact]
        public void ApplyDiscount_Should_Decrease_Price()
        {
            //Arrange
            var product = new Product();
            product.ChangePrice(100); // Prix initial

            //Act
            product.ApplyDiscount(30);

            //Assert
            Assert.Equal(70, product.Price);

        }

        [Fact]
        public void ApplyDiscount_Should_Throw_When_Resulting_Price_Is_Invalid()
        {
            // Arrange
            var product = new Product();
            product.ChangePrice(20); //valeur initiale

            // Act & Assert
           Assert.Throws<DomainException>(() => product.ApplyDiscount(30));

        }
    }
}

