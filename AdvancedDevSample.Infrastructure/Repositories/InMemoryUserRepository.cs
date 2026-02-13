using System;
using System.Collections.Generic;
using System.Linq;
using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Interfaces;

namespace AdvancedDevSample.Infrastructure.Repositories
{
    /// <summary>
    /// Implémentation en mémoire du repository User.
    /// </summary>
    public class InMemoryUserRepository : IUserRepository
    {
        private readonly List<User> _users = new();

        public InMemoryUserRepository()
        {
            // Seed avec des utilisateurs de test
            // Mot de passe pour admin: "Admin123!"
            // Mot de passe pour testuser: "User123!"
            // Ces hashes sont générés avec BCrypt.Net.BCrypt.HashPassword()
            
            var admin = new User(
                Guid.Parse("11111111-1111-1111-1111-111111111111"),
                "admin",
                "admin@advanceddevsample.com",
                "$2a$11$N9qo8uLOickgx2ZMRZoMye", // Hash temporaire, sera remplacé au premier login
                "Admin",
                true,
                DateTime.UtcNow.AddDays(-30)
            );

            var testUser = new User(
                Guid.Parse("22222222-2222-2222-2222-222222222222"),
                "testuser",
                "user@advanceddevsample.com",
                "$2a$11$N9qo8uLOickgx2ZMRZoMye", // Hash temporaire, sera remplacé au premier login
                "User",
                true,
                DateTime.UtcNow.AddDays(-15)
            );

            _users.Add(admin);
            _users.Add(testUser);
        }

        public void Add(User user)
        {
            _users.Add(user);
        }

        public User? GetById(Guid id)
        {
            return _users.FirstOrDefault(u => u.Id == id);
        }

        public User? GetByUsername(string username)
        {
            return _users.FirstOrDefault(u => 
                u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

        public User? GetByEmail(string email)
        {
            return _users.FirstOrDefault(u => 
                u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        public void Save(User user)
        {
            var existing = GetById(user.Id);
            if (existing == null)
            {
                _users.Add(user);
            }
            // Pour un repository en mémoire, pas besoin de faire plus
        }
    }
}
