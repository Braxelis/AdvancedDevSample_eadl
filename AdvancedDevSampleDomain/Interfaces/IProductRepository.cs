using System;
using System.Collections.Generic;
using AdvancedDevSample.Domain.Entities;

namespace AdvancedDevSample.Domain.Interfaces
{
    public interface IProductRepository
    {
        void Add(Product product);
        Product GetById(Guid id);
        IEnumerable<Product> ListAll();
        void Remove(Guid id);
        void Save(Product product);
    }
}