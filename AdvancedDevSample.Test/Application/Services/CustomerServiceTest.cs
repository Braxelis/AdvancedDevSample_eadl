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
    public class CustomerServiceTest
    {
        private readonly InMemoryCustomerRepositoryFake _repository;
        private readonly CustomerService _service;

        public CustomerServiceTest()
        {
            _repository = new InMemoryCustomerRepositoryFake();
            _service = new CustomerService(_repository);
        }

        [Fact]
        public void GetAll_WithNoCustomers_ShouldReturnEmptyList()
        {
            // Act
            var result = _service.GetAll();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void GetAll_WithCustomers_ShouldReturnAllCustomers()
        {
            // Arrange
            var customer1 = new Customer("Jean Dupont", "jean@example.com");
            var customer2 = new Customer("Marie Martin", "marie@example.com");
            _repository.Seed(customer1, customer2);

            // Act
            var result = _service.GetAll();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, c => c.Name == "Jean Dupont");
            Assert.Contains(result, c => c.Name == "Marie Martin");
        }

        [Fact]
        public void GetById_WithExistingCustomer_ShouldReturnCustomer()
        {
            // Arrange
            var customer = new Customer("Jean Dupont", "jean@example.com");
            _repository.Seed(customer);

            // Act
            var result = _service.GetById(customer.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(customer.Id, result.Id);
            Assert.Equal("Jean Dupont", result.Name);
        }

        [Fact]
        public void GetById_WithNonExistingCustomer_ShouldThrowException()
        {
            // Arrange
            var nonExistingId = Guid.NewGuid();

            // Act & Assert
            var exception = Assert.Throws<ApplicationServiceException>(() => _service.GetById(nonExistingId));
            Assert.Contains("non trouvé", exception.Message.ToLower());
        }

        [Fact]
        public void Create_WithValidRequest_ShouldCreateCustomer()
        {
            // Arrange
            var request = new CreateCustomerRequest
            {
                Name = "Jean Dupont",
                Email = "jean@example.com",
                Phone = "0123456789",
                Address = "123 Rue de Paris"
            };

            // Act
            var result = _service.Create(request);

            // Assert
            Assert.NotNull(result);
            Assert.NotEqual(Guid.Empty, result.Id);
            Assert.Equal("Jean Dupont", result.Name);
            Assert.Equal("jean@example.com", result.Email);
            Assert.Equal("0123456789", result.Phone);
            Assert.Equal("123 Rue de Paris", result.Address);
            Assert.True(result.IsActive);

            // Vérifier que le client est bien dans le repository
            var customers = _repository.ListAll().ToList();
            Assert.Single(customers);
        }

        [Fact]
        public void Update_WithExistingCustomer_ShouldUpdateCustomer()
        {
            // Arrange
            var customer = new Customer("Jean Dupont", "jean@example.com");
            _repository.Seed(customer);

            var updateRequest = new UpdateCustomerRequest
            {
                Name = "Jean Martin",
                Email = "jean.martin@example.com",
                Phone = "0987654321",
                Address = "456 Avenue des Champs"
            };

            // Act
            _service.Update(customer.Id, updateRequest);

            // Assert
            var updated = _repository.GetById(customer.Id);
            Assert.NotNull(updated);
            Assert.Equal("Jean Martin", updated.Name);
            Assert.Equal("jean.martin@example.com", updated.Email);
            Assert.Equal("0987654321", updated.Phone);
            Assert.Equal("456 Avenue des Champs", updated.Address);
        }

        [Fact]
        public void Update_WithNonExistingCustomer_ShouldThrowException()
        {
            // Arrange
            var nonExistingId = Guid.NewGuid();
            var updateRequest = new UpdateCustomerRequest
            {
                Name = "Jean Martin",
                Email = "jean.martin@example.com"
            };

            // Act & Assert
            var exception = Assert.Throws<ApplicationServiceException>(() => 
                _service.Update(nonExistingId, updateRequest));
            Assert.Contains("non trouvé", exception.Message.ToLower());
        }

        [Fact]
        public void Delete_WithExistingCustomer_ShouldRemoveCustomer()
        {
            // Arrange
            var customer = new Customer("Jean Dupont", "jean@example.com");
            _repository.Seed(customer);

            // Act
            _service.Delete(customer.Id);

            // Assert
            var customers = _repository.ListAll().ToList();
            Assert.Empty(customers);
        }

        [Fact]
        public void Activate_WithExistingCustomer_ShouldActivateCustomer()
        {
            // Arrange
            var customer = new Customer("Jean Dupont", "jean@example.com");
            customer.Deactivate();
            _repository.Seed(customer);

            // Act
            _service.Activate(customer.Id);

            // Assert
            var activated = _repository.GetById(customer.Id);
            Assert.NotNull(activated);
            Assert.True(activated.IsActive);
        }

        [Fact]
        public void Deactivate_WithExistingCustomer_ShouldDeactivateCustomer()
        {
            // Arrange
            var customer = new Customer("Jean Dupont", "jean@example.com");
            _repository.Seed(customer);

            // Act
            _service.Deactivate(customer.Id);

            // Assert
            var deactivated = _repository.GetById(customer.Id);
            Assert.NotNull(deactivated);
            Assert.False(deactivated.IsActive);
        }
    }
}
