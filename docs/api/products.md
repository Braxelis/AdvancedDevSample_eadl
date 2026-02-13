# API Produits

Documentation complète des endpoints pour la gestion des produits.

## Lister tous les produits

Récupère la liste de tous les produits du catalogue.

**Endpoint** : `GET /api/products`

**Réponse** : `200 OK`

```json
[
  {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "price": 99.99,
    "isActive": true,
    "supplierId": "7b85f64-5717-4562-b3fc-2c963f66afa7"
  },
  {
    "id": "4fa85f64-5717-4562-b3fc-2c963f66afa8",
    "price": 149.99,
    "isActive": true,
    "supplierId": null
  }
]
```

**Exemple avec curl** :

```bash
curl -X GET "http://localhost:5069/api/products" \
  -H "accept: application/json"
```

---

## Obtenir un produit

Récupère un produit spécifique par son ID.

**Endpoint** : `GET /api/products/{id}`

**Paramètres** :
- `id` (Guid, required) - Identifiant unique du produit

**Réponse** : `200 OK`

```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "price": 99.99,
  "isActive": true,
  "supplierId": "7b85f64-5717-4562-b3fc-2c963f66afa7"
}
```

**Erreurs** :
- `404 Not Found` - Produit non trouvé

```json
{
  "title": "Ressource introuvable",
  "detail": "Produit non trouvé."
}
```

**Exemple avec curl** :

```bash
curl -X GET "http://localhost:5069/api/products/3fa85f64-5717-4562-b3fc-2c963f66afa6" \
  -H "accept: application/json"
```

---

## Créer un produit

Crée un nouveau produit dans le catalogue.

**Endpoint** : `POST /api/products`

**Corps de la requête** :

```json
{
  "initialPrice": 99.99,
  "supplierId": "7b85f64-5717-4562-b3fc-2c963f66afa7"
}
```

**Validation** :
- `initialPrice` : Obligatoire, doit être strictement positif (> 0)
- `supplierId` : Optionnel, doit être un GUID valide si fourni

**Réponse** : `201 Created`

```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "price": 99.99,
  "isActive": true,
  "supplierId": "7b85f64-5717-4562-b3fc-2c963f66afa7"
}
```

**Erreurs** :
- `400 Bad Request` - Données invalides

```json
{
  "title": "Erreur métier",
  "detail": "Le prix doit être strictement positif."
}
```

**Exemple avec curl** :

```bash
curl -X POST "http://localhost:5069/api/products" \
  -H "accept: application/json" \
  -H "Content-Type: application/json" \
  -d '{
    "initialPrice": 99.99,
    "supplierId": "7b85f64-5717-4562-b3fc-2c963f66afa7"
  }'
```

---

## Modifier le prix

Met à jour le prix d'un produit existant.

**Endpoint** : `PUT /api/products/{id}/price`

**Paramètres** :
- `id` (Guid, required) - Identifiant du produit

**Corps de la requête** :

```json
{
  "newPrice": 129.99
}
```

**Validation** :
- `newPrice` : Obligatoire, doit être strictement positif (> 0)
- Le produit doit être actif

**Réponse** : `204 No Content`

**Erreurs** :
- `404 Not Found` - Produit non trouvé
- `400 Bad Request` - Prix invalide ou produit inactif

```json
{
  "title": "Erreur métier",
  "detail": "Le produit est inactif."
}
```

**Exemple avec curl** :

```bash
curl -X PUT "http://localhost:5069/api/products/3fa85f64-5717-4562-b3fc-2c963f66afa6/price" \
  -H "accept: application/json" \
  -H "Content-Type: application/json" \
  -d '{
    "newPrice": 129.99
  }'
```

---

## Activer un produit

Active un produit désactivé pour le rendre disponible à la vente.

**Endpoint** : `POST /api/products/{id}/activate`

**Paramètres** :
- `id` (Guid, required) - Identifiant du produit

**Réponse** : `204 No Content`

**Erreurs** :
- `404 Not Found` - Produit non trouvé

**Exemple avec curl** :

```bash
curl -X POST "http://localhost:5069/api/products/3fa85f64-5717-4562-b3fc-2c963f66afa6/activate" \
  -H "accept: application/json"
```

---

## Désactiver un produit

Désactive un produit pour le retirer temporairement de la vente.

**Endpoint** : `POST /api/products/{id}/deactivate`

**Paramètres** :
- `id` (Guid, required) - Identifiant du produit

**Réponse** : `204 No Content`

**Erreurs** :
- `404 Not Found` - Produit non trouvé

**Exemple avec curl** :

```bash
curl -X POST "http://localhost:5069/api/products/3fa85f64-5717-4562-b3fc-2c963f66afa6/deactivate" \
  -H "accept: application/json"
```

---

## Règles métier

### Validation du prix

- Le prix doit toujours être **strictement positif** (> 0)
- Le prix ne peut être modifié que si le produit est **actif**

### États du produit

Un produit peut être dans deux états :
- **Actif** (`isActive: true`) : Disponible à la vente
- **Inactif** (`isActive: false`) : Retiré temporairement

### Relation avec les fournisseurs

- Un produit peut être associé à un fournisseur via `supplierId`
- Cette relation est **optionnelle** (nullable)
- Le `supplierId` doit correspondre à un fournisseur existant

## Exemples d'utilisation

### Scénario 1 : Créer et activer un produit

```bash
# 1. Créer un produit
PRODUCT_ID=$(curl -X POST "http://localhost:5069/api/products" \
  -H "Content-Type: application/json" \
  -d '{"initialPrice": 99.99}' \
  | jq -r '.id')

# 2. Vérifier qu'il est actif par défaut
curl -X GET "http://localhost:5069/api/products/$PRODUCT_ID"

# 3. Modifier le prix
curl -X PUT "http://localhost:5069/api/products/$PRODUCT_ID/price" \
  -H "Content-Type: application/json" \
  -d '{"newPrice": 129.99}'
```

### Scénario 2 : Désactiver un produit

```bash
# 1. Désactiver le produit
curl -X POST "http://localhost:5069/api/products/$PRODUCT_ID/deactivate"

# 2. Tenter de modifier le prix (devrait échouer)
curl -X PUT "http://localhost:5069/api/products/$PRODUCT_ID/price" \
  -H "Content-Type: application/json" \
  -d '{"newPrice": 149.99}'
# Erreur: "Le produit est inactif."

# 3. Réactiver le produit
curl -X POST "http://localhost:5069/api/products/$PRODUCT_ID/activate"
```

## Codes de statut HTTP

| Code | Signification | Utilisation |
|------|---------------|-------------|
| 200 | OK | Requête GET réussie |
| 201 | Created | Produit créé avec succès |
| 204 | No Content | Opération réussie (PUT, POST activate/deactivate) |
| 400 | Bad Request | Données invalides ou règle métier violée |
| 404 | Not Found | Produit non trouvé |
| 500 | Internal Server Error | Erreur serveur |
