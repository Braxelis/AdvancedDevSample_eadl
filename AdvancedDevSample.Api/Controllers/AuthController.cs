using System;
using System.Security.Claims;
using AdvancedDevSample.Application.DTOs;
using AdvancedDevSample.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdvancedDevSample.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Inscrit un nouvel utilisateur.
        /// </summary>
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequest request)
        {
            var response = _authService.Register(request);
            return CreatedAtAction(nameof(GetCurrentUser), new { }, response);
        }

        /// <summary>
        /// Authentifie un utilisateur et retourne un token JWT.
        /// </summary>
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var response = _authService.Login(request);
            return Ok(response);
        }

        /// <summary>
        /// Récupère les informations de l'utilisateur actuellement connecté.
        /// </summary>
        [HttpGet("me")]
        [Authorize]
        public IActionResult GetCurrentUser()
        {
            // Récupérer l'ID de l'utilisateur depuis le token JWT
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)
                           ?? User.FindFirst("sub");

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized(new { message = "Token invalide." });
            }

            var user = _authService.GetCurrentUser(userId);
            return Ok(user);
        }
    }
}
