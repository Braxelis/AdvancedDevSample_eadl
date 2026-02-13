using System;
using System.Text.RegularExpressions;
using AdvancedDevSample.Domain.Exceptions;

namespace AdvancedDevSample.Domain.Entities
{
    /// <summary>
    /// Entité représentant un utilisateur du système.
    /// </summary>
    public class User
    {
        public Guid Id { get; private set; }
        public string Username { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
        public string Role { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime CreatedAt { get; private set; }

        // Constructeur pour créer un nouvel utilisateur
        public User(string username, string email, string passwordHash, string role = "User")
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new DomainException("Le nom d'utilisateur ne peut pas être vide.");

            if (username.Length < 3)
                throw new DomainException("Le nom d'utilisateur doit contenir au moins 3 caractères.");

            if (string.IsNullOrWhiteSpace(email))
                throw new DomainException("L'email ne peut pas être vide.");

            if (!IsValidEmail(email))
                throw new DomainException("L'email n'est pas valide.");

            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new DomainException("Le hash du mot de passe ne peut pas être vide.");

            if (string.IsNullOrWhiteSpace(role))
                throw new DomainException("Le rôle ne peut pas être vide.");

            Id = Guid.NewGuid();
            Username = username;
            Email = email.ToLowerInvariant();
            PasswordHash = passwordHash;
            Role = role;
            IsActive = true;
            CreatedAt = DateTime.UtcNow;
        }

        // Constructeur pour la reconstruction depuis la persistence
        public User(Guid id, string username, string email, string passwordHash, string role, bool isActive, DateTime createdAt)
        {
            Id = id;
            Username = username;
            Email = email;
            PasswordHash = passwordHash;
            Role = role;
            IsActive = isActive;
            CreatedAt = createdAt;
        }

        /// <summary>
        /// Change le mot de passe de l'utilisateur.
        /// </summary>
        public void ChangePassword(string newPasswordHash)
        {
            if (string.IsNullOrWhiteSpace(newPasswordHash))
                throw new DomainException("Le hash du mot de passe ne peut pas être vide.");

            if (!IsActive)
                throw new DomainException("Impossible de changer le mot de passe d'un utilisateur inactif.");

            PasswordHash = newPasswordHash;
        }

        /// <summary>
        /// Active l'utilisateur.
        /// </summary>
        public void Activate()
        {
            IsActive = true;
        }

        /// <summary>
        /// Désactive l'utilisateur.
        /// </summary>
        public void Deactivate()
        {
            IsActive = false;
        }

        /// <summary>
        /// Change le rôle de l'utilisateur.
        /// </summary>
        public void ChangeRole(string newRole)
        {
            if (string.IsNullOrWhiteSpace(newRole))
                throw new DomainException("Le rôle ne peut pas être vide.");

            Role = newRole;
        }

        private static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Pattern simple pour valider l'email
                var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                return Regex.IsMatch(email, emailPattern, RegexOptions.IgnoreCase);
            }
            catch
            {
                return false;
            }
        }
    }
}
