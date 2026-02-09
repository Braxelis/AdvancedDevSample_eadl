using System;
using System.Collections.Generic;
using System.Text;
using AdvancedDevSample.Domain.Entities;
namespace AdvancedDevSample.Domain.Interfaces
{
    public interface IProductRepository
    {
        public void Save(Product product);

        public Product GetById(Guid id);
    }
    public interface IProductRepositoryAsync
    {
        public void SaveAsync(Product product);

        public Product GetByIdAsync(Guid id);
    }
}
