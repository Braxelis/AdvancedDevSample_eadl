using System;
using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Exceptions;
using Xunit;

namespace AdvancedDevSample.Test.Domain.Entities
{
    public class CustomerTest
    {
        [Fact]
        public void Constructor_WithValidData_ShouldCreateCustomer()
        {
            // Arrange
            var name = "Jean Dupont";
            var email = "jean.dupont@example.com";
            var phone = "0123456789";
            var address = "123 Rue de Paris";

            // Act
            var customer = new Customer(name, email, phone, address);

            // Assert
            Assert.NotEqual(Guid.Empty, customer.Id);
            Assert.Equal(name, customer.Name);
            Assert.Equal(email, customer.Email);
            Assert.Equal(phone, customer.Phone);
            Assert.Equal(address, customer.Address);
            Assert.True(customer.IsActive);
        }

        [Fact]
        public void Constructor_WithEmptyName_ShouldThrowDomainException()
        {
            // Arrange
            var email = "jean.dupont@example.com";

            // Act & Assert
            var exception = Assert.Throws<DomainException>(() => new Customer("", email));
            Assert.Contains("nom", exception.Message.ToLower());
        }

        [Fact]
        public void Constructor_WithNullName_ShouldThrowDomainException()
        {
            // Arrange
            var email = "jean.dupont@example.com";

            // Act & Assert
            var exception = Assert.Throws<DomainException>(() => new Customer(null!, email));
            Assert.Contains("nom", exception.Message.ToLower());
        }

        [Fact]
        public void Constructor_WithEmptyEmail_ShouldThrowDomainException()
        {
            // Arrange
            var name = "Jean Dupont";

            // Act & Assert
            var exception = Assert.Throws<DomainException>(() => new Customer(name, ""));
            Assert.Contains("email", exception.Message.ToLower());
        }

        [Fact]
        public void Constructor_WithInvalidEmail_ShouldThrowDomainException()
        {
            // Arrange
            var name = "Jean Dupont";
            var invalidEmail = "not-an-email";

            // Act & Assert
            var exception = Assert.Throws<DomainException>(() => new Customer(name, invalidEmail));
            Assert.Contains("email", exception.Message.ToLower());
        }

        [Fact]
        public void UpdateContactInfo_WithValidData_ShouldUpdateCustomer()
        {
            // Arrange
            var customer = new Customer("Jean Dupont", "jean@example.com");
            var newName = "Jean Martin";
            var newEmail = "jean.martin@example.com";
            var newPhone = "0987654321";
            var newAddress = "456 Avenue des Champs";

            // Act
            customer.UpdateContactInfo(newName, newEmail, newPhone, newAddress);

            // Assert
            Assert.Equal(newName, customer.Name);
            Assert.Equal(newEmail, customer.Email);
            Assert.Equal(newPhone, customer.Phone);
            Assert.Equal(newAddress, customer.Address);
        }

        [Fact]
        public void UpdateContactInfo_WithInvalidEmail_ShouldThrowDomainException()
        {
            // Arrange
            var customer = new Customer("Jean Dupont", "jean@example.com");

            // Act & Assert
            var exception = Assert.Throws<DomainException>(() => 
                customer.UpdateContactInfo("Jean Martin", "invalid-email", null, null));
            Assert.Contains("email", exception.Message.ToLower());
        }

        [Fact]
        public void Activate_ShouldSetIsActiveToTrue()
        {
            // Arrange
            var customer = new Customer("Jean Dupont", "jean@example.com");
            customer.Deactivate();

            // Act
            customer.Activate();

            // Assert
            Assert.True(customer.IsActive);
        }

        [Fact]
        public void Deactivate_ShouldSetIsActiveToFalse()
        {
            // Arrange
            var customer = new Customer("Jean Dupont", "jean@example.com");

            // Act
            customer.Deactivate();

            // Assert
            Assert.False(customer.IsActive);
        }

        [Fact]
        public void Constructor_WithOptionalParameters_ShouldCreateCustomerWithNullValues()
        {
            // Arrange & Act
            var customer = new Customer("Jean Dupont", "jean@example.com");

            // Assert
            Assert.Null(customer.Phone);
            Assert.Null(customer.Address);
        }
    }
}
