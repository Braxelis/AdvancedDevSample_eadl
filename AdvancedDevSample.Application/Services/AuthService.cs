using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AdvancedDevSample.Application.DTOs;
using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Exceptions;
using AdvancedDevSample.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using BCrypt.Net;

namespace AdvancedDevSample.Application.Services
{
    /// <summary>
    /// Service d'authentification gérant l'inscription, la connexion et la génération de tokens JWT.
    /// </summary>
    public class AuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        /// <summary>
        /// Inscrit un nouvel utilisateur.
        /// </summary>
        public AuthResponse Register(RegisterRequest request)
        {
            // Vérifier si l'utilisateur existe déjà
            if (_userRepository.GetByUsername(request.Username) != null)
            {
                throw new ApplicationServiceException("Ce nom d'utilisateur est déjà utilisé.");
            }

            if (_userRepository.GetByEmail(request.Email) != null)
            {
                throw new ApplicationServiceException("Cet email est déjà utilisé.");
            }

            // Hasher le mot de passe
            var passwordHash = HashPassword(request.Password);

            // Créer l'utilisateur
            var user = new User(request.Username, request.Email, passwordHash);
            _userRepository.Add(user);

            // Générer le token JWT
            return GenerateAuthResponse(user);
        }

        /// <summary>
        /// Authentifie un utilisateur et retourne un token JWT.
        /// </summary>
        public AuthResponse Login(LoginRequest request)
        {
            // Chercher l'utilisateur par username ou email
            var user = _userRepository.GetByUsername(request.UsernameOrEmail)
                      ?? _userRepository.GetByEmail(request.UsernameOrEmail);

            if (user == null)
            {
                throw new ApplicationServiceException("Nom d'utilisateur ou mot de passe incorrect.");
            }

            // Vérifier le mot de passe
            if (!VerifyPassword(request.Password, user.PasswordHash))
            {
                throw new ApplicationServiceException("Nom d'utilisateur ou mot de passe incorrect.");
            }

            // Vérifier que l'utilisateur est actif
            if (!user.IsActive)
            {
                throw new ApplicationServiceException("Ce compte est désactivé.");
            }

            // Générer le token JWT
            return GenerateAuthResponse(user);
        }

        /// <summary>
        /// Récupère les informations de l'utilisateur actuel.
        /// </summary>
        public UserDto GetCurrentUser(Guid userId)
        {
            var user = _userRepository.GetById(userId);
            if (user == null)
            {
                throw new ApplicationServiceException("Utilisateur non trouvé.");
            }

            return MapToDto(user);
        }

        // --------- Méthodes privées ---------

        private AuthResponse GenerateAuthResponse(User user)
        {
            var token = GenerateJwtToken(user);
            var expiresInMinutes = int.Parse(_configuration["Jwt:ExpiresInMinutes"] ?? "60");

            return new AuthResponse
            {
                Token = token,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role,
                ExpiresAt = DateTime.UtcNow.AddMinutes(expiresInMinutes)
            };
        }

        private string GenerateJwtToken(User user)
        {
            var key = _configuration["Jwt:Key"]
                     ?? throw new InvalidOperationException("JWT Key non configurée.");
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var expiresInMinutes = int.Parse(_configuration["Jwt:ExpiresInMinutes"] ?? "60");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiresInMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private static bool VerifyPassword(string password, string passwordHash)
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(password, passwordHash);
            }
            catch
            {
                return false;
            }
        }

        private static UserDto MapToDto(User user) =>
            new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt
            };
    }
}
