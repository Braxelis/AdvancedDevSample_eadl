using System;
using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Exceptions;
using Xunit;

namespace AdvancedDevSample.Test.Domain.Entities
{
    public class SupplierTest
    {
        [Fact]
        public void Constructor_WithValidData_ShouldCreateSupplier()
        {
            // Arrange
            var name = "Fournisseur ABC";
            var email = "contact@abc.com";
            var phone = "0987654321";
            var address = "456 Avenue des Champs";

            // Act
            var supplier = new Supplier(name, email, phone, address);

            // Assert
            Assert.NotEqual(Guid.Empty, supplier.Id);
            Assert.Equal(name, supplier.Name);
            Assert.Equal(email, supplier.Email);
            Assert.Equal(phone, supplier.Phone);
            Assert.Equal(address, supplier.Address);
            Assert.True(supplier.IsActive);
        }

        [Fact]
        public void Constructor_WithEmptyName_ShouldThrowDomainException()
        {
            // Arrange
            var email = "contact@abc.com";

            // Act & Assert
            var exception = Assert.Throws<DomainException>(() => new Supplier("", email));
            Assert.Contains("nom", exception.Message.ToLower());
        }

        [Fact]
        public void Constructor_WithNullName_ShouldThrowDomainException()
        {
            // Arrange
            var email = "contact@abc.com";

            // Act & Assert
            var exception = Assert.Throws<DomainException>(() => new Supplier(null!, email));
            Assert.Contains("nom", exception.Message.ToLower());
        }

        [Fact]
        public void Constructor_WithEmptyEmail_ShouldThrowDomainException()
        {
            // Arrange
            var name = "Fournisseur ABC";

            // Act & Assert
            var exception = Assert.Throws<DomainException>(() => new Supplier(name, ""));
            Assert.Contains("email", exception.Message.ToLower());
        }

        [Fact]
        public void Constructor_WithInvalidEmail_ShouldThrowDomainException()
        {
            // Arrange
            var name = "Fournisseur ABC";
            var invalidEmail = "not-an-email";

            // Act & Assert
            var exception = Assert.Throws<DomainException>(() => new Supplier(name, invalidEmail));
            Assert.Contains("email", exception.Message.ToLower());
        }

        [Fact]
        public void UpdateContactInfo_WithValidData_ShouldUpdateSupplier()
        {
            // Arrange
            var supplier = new Supplier("Fournisseur ABC", "contact@abc.com");
            var newName = "Fournisseur XYZ";
            var newEmail = "contact@xyz.com";
            var newPhone = "0123456789";
            var newAddress = "789 Boulevard Saint-Germain";

            // Act
            supplier.UpdateContactInfo(newName, newEmail, newPhone, newAddress);

            // Assert
            Assert.Equal(newName, supplier.Name);
            Assert.Equal(newEmail, supplier.Email);
            Assert.Equal(newPhone, supplier.Phone);
            Assert.Equal(newAddress, supplier.Address);
        }

        [Fact]
        public void UpdateContactInfo_WithInvalidEmail_ShouldThrowDomainException()
        {
            // Arrange
            var supplier = new Supplier("Fournisseur ABC", "contact@abc.com");

            // Act & Assert
            var exception = Assert.Throws<DomainException>(() => 
                supplier.UpdateContactInfo("Fournisseur XYZ", "invalid-email", null, null));
            Assert.Contains("email", exception.Message.ToLower());
        }

        [Fact]
        public void Activate_ShouldSetIsActiveToTrue()
        {
            // Arrange
            var supplier = new Supplier("Fournisseur ABC", "contact@abc.com");
            supplier.Deactivate();

            // Act
            supplier.Activate();

            // Assert
            Assert.True(supplier.IsActive);
        }

        [Fact]
        public void Deactivate_ShouldSetIsActiveToFalse()
        {
            // Arrange
            var supplier = new Supplier("Fournisseur ABC", "contact@abc.com");

            // Act
            supplier.Deactivate();

            // Assert
            Assert.False(supplier.IsActive);
        }

        [Fact]
        public void Constructor_WithOptionalParameters_ShouldCreateSupplierWithNullValues()
        {
            // Arrange & Act
            var supplier = new Supplier("Fournisseur ABC", "contact@abc.com");

            // Assert
            Assert.Null(supplier.Phone);
            Assert.Null(supplier.Address);
        }
    }
}
