# API Authentification

Documentation des endpoints d'authentification JWT.

## Inscription

Crée un nouveau compte utilisateur.

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
- `username` : Requis, minimum 3 caractères
- `email` : Requis, format email valide
- `password` : Requis, minimum 6 caractères

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

**Erreurs** :
- `400 Bad Request` - Données invalides
- `409 Conflict` - Username ou email déjà utilisé

**Exemple avec curl** :
```bash
curl -X POST "http://localhost:5069/api/auth/register" \
  -H "Content-Type: application/json" \
  -d '{
    "username": "johndoe",
    "email": "john@example.com",
    "password": "SecurePassword123!"
  }'
```

---

## Connexion

Authentifie un utilisateur et retourne un token JWT.

**Endpoint** : `POST /api/auth/login`

**Corps de la requête** :
```json
{
  "usernameOrEmail": "johndoe",
  "password": "SecurePassword123!"
}
```

**Paramètres** :
- `usernameOrEmail` : Nom d'utilisateur OU email
- `password` : Mot de passe

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

**Erreurs** :
- `400 Bad Request` - Nom d'utilisateur ou mot de passe incorrect
- `400 Bad Request` - Compte désactivé

**Exemple avec curl** :
```bash
curl -X POST "http://localhost:5069/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{
    "usernameOrEmail": "johndoe",
    "password": "SecurePassword123!"
  }'
```

---

## Obtenir l'utilisateur actuel

Récupère les informations de l'utilisateur actuellement connecté.

**Endpoint** : `GET /api/auth/me`

**Authentification** : Requise (Bearer Token)

**En-tête** :
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

**Erreurs** :
- `401 Unauthorized` - Token manquant ou invalide

**Exemple avec curl** :
```bash
# Récupérer le token
TOKEN=$(curl -X POST "http://localhost:5069/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{"usernameOrEmail": "johndoe", "password": "SecurePassword123!"}' \
  | jq -r '.token')

# Utiliser le token
curl -X GET "http://localhost:5069/api/auth/me" \
  -H "Authorization: Bearer $TOKEN"
```

---

## Utilisation du token JWT

Une fois connecté, le token doit être inclus dans l'en-tête `Authorization` de toutes les requêtes protégées.

### Format de l'en-tête

```
Authorization: Bearer {token}
```

### Exemple complet

```bash
# 1. Connexion
RESPONSE=$(curl -X POST "http://localhost:5069/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{
    "usernameOrEmail": "admin",
    "password": "Admin123!"
  }')

# 2. Extraire le token
TOKEN=$(echo $RESPONSE | jq -r '.token')

# 3. Utiliser le token pour accéder aux produits
curl -X GET "http://localhost:5069/api/products" \
  -H "Authorization: Bearer $TOKEN"

# 4. Créer un produit
curl -X POST "http://localhost:5069/api/products" \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{"initialPrice": 99.99}'
```

---

## Rôles et permissions

### Rôles disponibles

- **Admin** : Accès complet à toutes les ressources
- **User** : Accès standard aux ressources

### Endpoints par rôle

| Endpoint | Anonyme | User | Admin |
|----------|---------|------|-------|
| POST /api/auth/register | ✅ | ✅ | ✅ |
| POST /api/auth/login | ✅ | ✅ | ✅ |
| GET /api/auth/me | ❌ | ✅ | ✅ |
| GET /api/products | ❌ | ✅ | ✅ |
| POST /api/products | ❌ | ✅ | ✅ |
| DELETE /api/products/{id} | ❌ | ❌ | ✅ |

---

## Gestion des erreurs

### Erreurs d'authentification

**401 Unauthorized** :
```json
{
  "title": "Unauthorized",
  "detail": "Token invalide ou expiré."
}
```

**403 Forbidden** :
```json
{
  "title": "Forbidden",
  "detail": "Vous n'avez pas les permissions nécessaires."
}
```

### Erreurs de validation

**400 Bad Request** :
```json
{
  "title": "Erreur de validation",
  "detail": "Le mot de passe doit contenir au moins 6 caractères."
}
```

**409 Conflict** :
```json
{
  "title": "Conflit",
  "detail": "Ce nom d'utilisateur est déjà utilisé."
}
```

---

## Sécurité

Pour plus d'informations sur la sécurité et l'authentification JWT, consultez la [page Sécurité](../security.md).

### Points clés

- ✅ Les mots de passe sont hashés avec BCrypt
- ✅ Les tokens expirent après 60 minutes
- ✅ Les tokens sont signés avec HMAC-SHA256
- ✅ Validation stricte des entrées utilisateur
