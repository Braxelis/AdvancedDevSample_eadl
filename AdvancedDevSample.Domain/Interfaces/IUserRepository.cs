using System;
using AdvancedDevSample.Domain.Entities;

namespace AdvancedDevSample.Domain.Interfaces
{
    /// <summary>
    /// Contrat de persistance pour les utilisateurs.
    /// </summary>
    public interface IUserRepository
    {
        void Add(User user);
        User? GetById(Guid id);
        User? GetByUsername(string username);
        User? GetByEmail(string email);
        void Save(User user);
    }
}
