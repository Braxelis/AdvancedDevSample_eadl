# API Clients

Documentation des endpoints pour la gestion des clients.

## Lister tous les clients

Récupère la liste de tous les clients.

**Endpoint** : `GET /api/customers`

**Réponse** : `200 OK`

```json
[
  {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "name": "Jean Dupont",
    "email": "jean.dupont@example.com",
    "phone": "0123456789",
    "address": "123 Rue de Paris, 75001 Paris",
    "isActive": true
  }
]
```

## Obtenir un client

Récupère un client par son ID.

**Endpoint** : `GET /api/customers/{id}`

**Paramètres** :
- `id` (Guid) - Identifiant du client

**Réponse** : `200 OK`

```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "name": "Jean Dupont",
  "email": "jean.dupont@example.com",
  "phone": "0123456789",
  "address": "123 Rue de Paris, 75001 Paris",
  "isActive": true
}
```

**Erreurs** :
- `404 Not Found` - Client non trouvé

## Créer un client

Crée un nouveau client.

**Endpoint** : `POST /api/customers`

**Corps de la requête** :

```json
{
  "name": "Jean Dupont",
  "email": "jean.dupont@example.com",
  "phone": "0123456789",
  "address": "123 Rue de Paris, 75001 Paris"
}
```

**Validation** :
- `name` : Obligatoire, non vide
- `email` : Obligatoire, format email valide
- `phone` : Optionnel
- `address` : Optionnel

**Réponse** : `201 Created`

```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "name": "Jean Dupont",
  "email": "jean.dupont@example.com",
  "phone": "0123456789",
  "address": "123 Rue de Paris, 75001 Paris",
  "isActive": true
}
```

**Erreurs** :
- `400 Bad Request` - Données invalides

## Mettre à jour un client

Met à jour les informations d'un client existant.

**Endpoint** : `PUT /api/customers/{id}`

**Paramètres** :
- `id` (Guid) - Identifiant du client

**Corps de la requête** :

```json
{
  "name": "Jean Dupont",
  "email": "jean.dupont@example.com",
  "phone": "0987654321",
  "address": "456 Avenue des Champs, 75008 Paris"
}
```

**Réponse** : `204 No Content`

**Erreurs** :
- `404 Not Found` - Client non trouvé
- `400 Bad Request` - Données invalides

## Supprimer un client

Supprime un client.

**Endpoint** : `DELETE /api/customers/{id}`

**Paramètres** :
- `id` (Guid) - Identifiant du client

**Réponse** : `204 No Content`

**Erreurs** :
- `404 Not Found` - Client non trouvé

## Activer un client

Active un client désactivé.

**Endpoint** : `POST /api/customers/{id}/activate`

**Paramètres** :
- `id` (Guid) - Identifiant du client

**Réponse** : `204 No Content`

**Erreurs** :
- `404 Not Found` - Client non trouvé

## Désactiver un client

Désactive un client actif.

**Endpoint** : `POST /api/customers/{id}/deactivate`

**Paramètres** :
- `id` (Guid) - Identifiant du client

**Réponse** : `204 No Content`

**Erreurs** :
- `404 Not Found` - Client non trouvé
