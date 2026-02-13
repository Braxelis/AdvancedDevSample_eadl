# API Fournisseurs

Documentation des endpoints pour la gestion des fournisseurs.

## Lister tous les fournisseurs

Récupère la liste de tous les fournisseurs.

**Endpoint** : `GET /api/suppliers`

**Réponse** : `200 OK`

```json
[
  {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "name": "Fournisseur ABC",
    "email": "contact@abc.com",
    "phone": "0987654321",
    "address": "456 Avenue des Champs, 75008 Paris",
    "isActive": true
  }
]
```

## Obtenir un fournisseur

Récupère un fournisseur par son ID.

**Endpoint** : `GET /api/suppliers/{id}`

**Paramètres** :
- `id` (Guid) - Identifiant du fournisseur

**Réponse** : `200 OK`

```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "name": "Fournisseur ABC",
  "email": "contact@abc.com",
  "phone": "0987654321",
  "address": "456 Avenue des Champs, 75008 Paris",
  "isActive": true
}
```

**Erreurs** :
- `404 Not Found` - Fournisseur non trouvé

## Créer un fournisseur

Crée un nouveau fournisseur.

**Endpoint** : `POST /api/suppliers`

**Corps de la requête** :

```json
{
  "name": "Fournisseur ABC",
  "email": "contact@abc.com",
  "phone": "0987654321",
  "address": "456 Avenue des Champs, 75008 Paris"
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
  "name": "Fournisseur ABC",
  "email": "contact@abc.com",
  "phone": "0987654321",
  "address": "456 Avenue des Champs, 75008 Paris",
  "isActive": true
}
```

**Erreurs** :
- `400 Bad Request` - Données invalides

## Mettre à jour un fournisseur

Met à jour les informations d'un fournisseur existant.

**Endpoint** : `PUT /api/suppliers/{id}`

**Paramètres** :
- `id` (Guid) - Identifiant du fournisseur

**Corps de la requête** :

```json
{
  "name": "Fournisseur XYZ",
  "email": "contact@xyz.com",
  "phone": "0123456789",
  "address": "789 Boulevard Saint-Germain, 75006 Paris"
}
```

**Réponse** : `204 No Content`

**Erreurs** :
- `404 Not Found` - Fournisseur non trouvé
- `400 Bad Request` - Données invalides

## Supprimer un fournisseur

Supprime un fournisseur.

**Endpoint** : `DELETE /api/suppliers/{id}`

**Paramètres** :
- `id` (Guid) - Identifiant du fournisseur

**Réponse** : `204 No Content`

**Erreurs** :
- `404 Not Found` - Fournisseur non trouvé

## Activer un fournisseur

Active un fournisseur désactivé.

**Endpoint** : `POST /api/suppliers/{id}/activate`

**Paramètres** :
- `id` (Guid) - Identifiant du fournisseur

**Réponse** : `204 No Content`

**Erreurs** :
- `404 Not Found` - Fournisseur non trouvé

## Désactiver un fournisseur

Désactive un fournisseur actif.

**Endpoint** : `POST /api/suppliers/{id}/deactivate`

**Paramètres** :
- `id` (Guid) - Identifiant du fournisseur

**Réponse** : `204 No Content`

**Erreurs** :
- `404 Not Found` - Fournisseur non trouvé
