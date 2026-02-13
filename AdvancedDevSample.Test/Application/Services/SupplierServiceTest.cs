using System;
using System.Linq;
using AdvancedDevSample.Application.DTOs;
using AdvancedDevSample.Application.Services;
using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Exceptions;
using AdvancedDevSample.Test.Application.Fakes;
using Xunit;

namespace AdvancedDevSample.Test.Application.Services
{
    public class SupplierServiceTest
    {
        private readonly InMemorySupplierRepositoryFake _repository;
        private readonly SupplierService _service;

        public SupplierServiceTest()
        {
            _repository = new InMemorySupplierRepositoryFake();
            _service = new SupplierService(_repository);
        }

        [Fact]
        public void GetAll_WithNoSuppliers_ShouldReturnEmptyList()
        {
            // Act
            var result = _service.GetAll();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void GetAll_WithSuppliers_ShouldReturnAllSuppliers()
        {
            // Arrange
            var supplier1 = new Supplier("Fournisseur ABC", "contact@abc.com");
            var supplier2 = new Supplier("Fournisseur XYZ", "contact@xyz.com");
            _repository.Seed(supplier1, supplier2);

            // Act
            var result = _service.GetAll();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, s => s.Name == "Fournisseur ABC");
            Assert.Contains(result, s => s.Name == "Fournisseur XYZ");
        }

        [Fact]
        public void GetById_WithExistingSupplier_ShouldReturnSupplier()
        {
            // Arrange
            var supplier = new Supplier("Fournisseur ABC", "contact@abc.com");
            _repository.Seed(supplier);

            // Act
            var result = _service.GetById(supplier.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(supplier.Id, result.Id);
            Assert.Equal("Fournisseur ABC", result.Name);
        }

        [Fact]
        public void GetById_WithNonExistingSupplier_ShouldThrowException()
        {
            // Arrange
            var nonExistingId = Guid.NewGuid();

            // Act & Assert
            var exception = Assert.Throws<ApplicationServiceException>(() => _service.GetById(nonExistingId));
            Assert.Contains("non trouvé", exception.Message.ToLower());
        }

        [Fact]
        public void Create_WithValidRequest_ShouldCreateSupplier()
        {
            // Arrange
            var request = new CreateSupplierRequest
            {
                Name = "Fournisseur ABC",
                Email = "contact@abc.com",
                Phone = "0987654321",
                Address = "456 Avenue des Champs"
            };

            // Act
            var result = _service.Create(request);

            // Assert
            Assert.NotNull(result);
            Assert.NotEqual(Guid.Empty, result.Id);
            Assert.Equal("Fournisseur ABC", result.Name);
            Assert.Equal("contact@abc.com", result.Email);
            Assert.Equal("0987654321", result.Phone);
            Assert.Equal("456 Avenue des Champs", result.Address);
            Assert.True(result.IsActive);

            // Vérifier que le fournisseur est bien dans le repository
            var suppliers = _repository.ListAll().ToList();
            Assert.Single(suppliers);
        }

        [Fact]
        public void Update_WithExistingSupplier_ShouldUpdateSupplier()
        {
            // Arrange
            var supplier = new Supplier("Fournisseur ABC", "contact@abc.com");
            _repository.Seed(supplier);

            var updateRequest = new UpdateSupplierRequest
            {
                Name = "Fournisseur XYZ",
                Email = "contact@xyz.com",
                Phone = "0123456789",
                Address = "789 Boulevard Saint-Germain"
            };

            // Act
            _service.Update(supplier.Id, updateRequest);

            // Assert
            var updated = _repository.GetById(supplier.Id);
            Assert.NotNull(updated);
            Assert.Equal("Fournisseur XYZ", updated.Name);
            Assert.Equal("contact@xyz.com", updated.Email);
            Assert.Equal("0123456789", updated.Phone);
            Assert.Equal("789 Boulevard Saint-Germain", updated.Address);
        }

        [Fact]
        public void Update_WithNonExistingSupplier_ShouldThrowException()
        {
            // Arrange
            var nonExistingId = Guid.NewGuid();
            var updateRequest = new UpdateSupplierRequest
            {
                Name = "Fournisseur XYZ",
                Email = "contact@xyz.com"
            };

            // Act & Assert
            var exception = Assert.Throws<ApplicationServiceException>(() => 
                _service.Update(nonExistingId, updateRequest));
            Assert.Contains("non trouvé", exception.Message.ToLower());
        }

        [Fact]
        public void Delete_WithExistingSupplier_ShouldRemoveSupplier()
        {
            // Arrange
            var supplier = new Supplier("Fournisseur ABC", "contact@abc.com");
            _repository.Seed(supplier);

            // Act
            _service.Delete(supplier.Id);

            // Assert
            var suppliers = _repository.ListAll().ToList();
            Assert.Empty(suppliers);
        }

        [Fact]
        public void Activate_WithExistingSupplier_ShouldActivateSupplier()
        {
            // Arrange
            var supplier = new Supplier("Fournisseur ABC", "contact@abc.com");
            supplier.Deactivate();
            _repository.Seed(supplier);

            // Act
            _service.Activate(supplier.Id);

            // Assert
            var activated = _repository.GetById(supplier.Id);
            Assert.NotNull(activated);
            Assert.True(activated.IsActive);
        }

        [Fact]
        public void Deactivate_WithExistingSupplier_ShouldDeactivateSupplier()
        {
            // Arrange
            var supplier = new Supplier("Fournisseur ABC", "contact@abc.com");
            _repository.Seed(supplier);

            // Act
            _service.Deactivate(supplier.Id);

            // Assert
            var deactivated = _repository.GetById(supplier.Id);
            Assert.NotNull(deactivated);
            Assert.False(deactivated.IsActive);
        }
    }
}
