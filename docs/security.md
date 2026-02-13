# Sécurité et Authentification

Ce document décrit le système d'authentification JWT implémenté dans AdvancedDevSample.

## Vue d'ensemble

L'application utilise **JWT (JSON Web Tokens)** pour l'authentification et l'autorisation des utilisateurs. Les tokens sont générés lors de la connexion et doivent être inclus dans l'en-tête `Authorization` de chaque requête protégée.

## Architecture d'authentification

![Architecture d'authentification JWT](images/security-architecture.jpg)

L'architecture d'authentification repose sur JWT (JSON Web Tokens) pour sécuriser l'accès aux ressources de l'API.

## Flux d'authentification

### Flux d'inscription

![Flux d'inscription](images/security-register-flow.jpg)

Ce diagramme illustre le processus complet d'inscription d'un nouvel utilisateur avec validation et hashing du mot de passe.

### Flux de connexion

![Flux de connexion](images/security-login-flow.jpg)

Ce diagramme montre le processus d'authentification avec vérification du mot de passe et génération du token JWT.

### Accès aux ressources protégées

![Accès aux ressources protégées](images/security-protected-access.jpg)

Ce diagramme illustre comment le middleware JWT valide les tokens pour protéger l'accès aux ressources.

## Endpoints d'authentification

### Inscription

**Endpoint** : `POST /api/auth/register`

**Corps de la requête** :
```json
{
  "username": "johndoe",
  "email": "john@example.com",
  "password": "SecurePassword123!"
}
```

**Validation** :
- `username` : Minimum 3 caractères
- `email` : Format email valide
- `password` : Minimum 6 caractères

**Réponse** : `201 Created`
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "username": "johndoe",
  "email": "john@example.com",
  "role": "User",
  "expiresAt": "2024-02-13T11:30:00Z"
}
```

### Connexion

**Endpoint** : `POST /api/auth/login`

**Corps de la requête** :
```json
{
  "usernameOrEmail": "johndoe",
  "password": "SecurePassword123!"
}
```

**Réponse** : `200 OK`
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "username": "johndoe",
  "email": "john@example.com",
  "role": "User",
  "expiresAt": "2024-02-13T11:30:00Z"
}
```

### Utilisateur actuel

**Endpoint** : `GET /api/auth/me`

**En-tête requis** :
```
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

**Réponse** : `200 OK`
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "username": "johndoe",
  "email": "john@example.com",
  "role": "User",
  "isActive": true,
  "createdAt": "2024-01-15T10:00:00Z"
}
```

## Configuration JWT

La configuration JWT se trouve dans `appsettings.json` :

```json
{
  "Jwt": {
    "Key": "VotreCleSecreteTresLonguePourJWT2024AdvancedDevSample!",
    "Issuer": "AdvancedDevSample",
    "Audience": "AdvancedDevSampleUsers",
    "ExpiresInMinutes": "60"
  }
}
```

> [!WARNING]
> **En production** : Ne jamais stocker la clé JWT dans `appsettings.json`. Utilisez Azure Key Vault, AWS Secrets Manager, ou des variables d'environnement.

## Structure du token JWT

Un token JWT contient les claims suivants :

```json
{
  "sub": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "unique_name": "johndoe",
  "email": "john@example.com",
  "role": "User",
  "jti": "unique-token-id",
  "exp": 1707825000,
  "iss": "AdvancedDevSample",
  "aud": "AdvancedDevSampleUsers"
}
```

## Utilisation du token

### Avec curl

```bash
# 1. S'inscrire
RESPONSE=$(curl -X POST "http://localhost:5069/api/auth/register" \
  -H "Content-Type: application/json" \
  -d '{
    "username": "johndoe",
    "email": "john@example.com",
    "password": "SecurePassword123!"
  }')

# 2. Extraire le token
TOKEN=$(echo $RESPONSE | jq -r '.token')

# 3. Utiliser le token pour accéder aux ressources protégées
curl -X GET "http://localhost:5069/api/products" \
  -H "Authorization: Bearer $TOKEN"
```

### Avec Postman

1. **Connexion** :
   - POST `http://localhost:5069/api/auth/login`
   - Body (JSON) : `{"usernameOrEmail": "admin", "password": "Admin123!"}`
   - Copier le `token` de la réponse

2. **Utiliser le token** :
   - Dans l'onglet "Authorization"
   - Type : "Bearer Token"
   - Coller le token

## Sécurité du mot de passe

### Hashing avec BCrypt

Les mots de passe sont hashés avec **BCrypt** avant d'être stockés :

```csharp
// Hashing
var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

// Vérification
var isValid = BCrypt.Net.BCrypt.Verify(password, passwordHash);
```

**Avantages de BCrypt** :
- ✅ Résistant aux attaques par force brute
- ✅ Salt automatique et unique pour chaque mot de passe
- ✅ Coût de calcul ajustable

## Rôles et autorisations

### Rôles disponibles

- **Admin** : Accès complet à toutes les ressources
- **User** : Accès standard aux ressources

### Protection des endpoints

```csharp
[Authorize] // Nécessite un token valide
public IActionResult GetAll() { }

[Authorize(Roles = "Admin")] // Nécessite le rôle Admin
public IActionResult Delete(Guid id) { }

[AllowAnonymous] // Accessible sans authentification
public IActionResult Login() { }
```

## Utilisateurs de test

L'application est préchargée avec deux utilisateurs de test :

| Username | Email | Password | Rôle |
|----------|-------|----------|------|
| admin | admin@advanceddevsample.com | *À définir au premier login* | Admin |
| testuser | user@advanceddevsample.com | *À définir au premier login* | User |

> [!NOTE]
> Pour des raisons de sécurité, les mots de passe de test doivent être définis lors de la première utilisation via l'endpoint `/api/auth/register`.

## Bonnes pratiques de sécurité

### ✅ À faire

- **Utiliser HTTPS** en production
- **Stocker les secrets** dans un coffre-fort sécurisé
- **Définir une expiration courte** pour les tokens (60 minutes recommandé)
- **Valider toutes les entrées** utilisateur
- **Implémenter un refresh token** pour les sessions longues
- **Logger les tentatives de connexion** échouées
- **Implémenter un rate limiting** pour prévenir les attaques par force brute

### ❌ À éviter

- Stocker les tokens dans le localStorage (préférer les cookies HttpOnly)
- Utiliser des mots de passe faibles
- Exposer la clé JWT dans le code source
- Ignorer les erreurs de validation de token
- Permettre des tentatives de connexion illimitées

## Gestion des erreurs

| Code | Erreur | Cause |
|------|--------|-------|
| 400 | Bad Request | Données invalides (email, mot de passe trop court, etc.) |
| 401 | Unauthorized | Token manquant, invalide ou expiré |
| 403 | Forbidden | Token valide mais rôle insuffisant |
| 409 | Conflict | Username ou email déjà utilisé |

## Exemple complet

```bash
# 1. Inscription
curl -X POST "http://localhost:5069/api/auth/register" \
  -H "Content-Type: application/json" \
  -d '{
    "username": "alice",
    "email": "alice@example.com",
    "password": "SecurePass123!"
  }'

# Réponse :
# {
#   "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
#   "username": "alice",
#   "email": "alice@example.com",
#   "role": "User",
#   "expiresAt": "2024-02-13T11:30:00Z"
# }

# 2. Connexion
TOKEN=$(curl -X POST "http://localhost:5069/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{
    "usernameOrEmail": "alice",
    "password": "SecurePass123!"
  }' | jq -r '.token')

# 3. Accéder à une ressource protégée
curl -X GET "http://localhost:5069/api/products" \
  -H "Authorization: Bearer $TOKEN"

# 4. Vérifier l'utilisateur actuel
curl -X GET "http://localhost:5069/api/auth/me" \
  -H "Authorization: Bearer $TOKEN"
```

## Prochaines étapes

Pour améliorer la sécurité :

1. **Refresh Tokens** : Implémenter un système de refresh tokens
2. **Two-Factor Authentication** : Ajouter l'authentification à deux facteurs
3. **Password Reset** : Implémenter la réinitialisation de mot de passe
4. **Account Lockout** : Bloquer les comptes après plusieurs tentatives échouées
5. **Audit Logging** : Logger toutes les actions sensibles
