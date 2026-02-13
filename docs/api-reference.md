# Référence API

Cette page documente tous les endpoints de l'API REST AdvancedDevSample.

## Base URL

```
http://localhost:5069/api
```

## Endpoints disponibles

### Produits

- [GET /products](api/products.md#lister-tous-les-produits) - Lister tous les produits
- [GET /products/{id}](api/products.md#obtenir-un-produit) - Obtenir un produit par ID
- [POST /products](api/products.md#créer-un-produit) - Créer un nouveau produit
- [PUT /products/{id}/price](api/products.md#modifier-le-prix) - Modifier le prix d'un produit
- [POST /products/{id}/activate](api/products.md#activer-un-produit) - Activer un produit
- [POST /products/{id}/deactivate](api/products.md#désactiver-un-produit) - Désactiver un produit

### Commandes

- [GET /orders/{id}](api/orders.md#obtenir-une-commande) - Obtenir une commande par ID
- [POST /orders](api/orders.md#créer-une-commande) - Créer une nouvelle commande
- [POST /orders/{id}/lines](api/orders.md#ajouter-une-ligne) - Ajouter une ligne à une commande
- [PUT /orders/{id}/lines](api/orders.md#modifier-la-quantité) - Modifier la quantité d'une ligne
- [DELETE /orders/{id}/lines/{productId}](api/orders.md#supprimer-une-ligne) - Supprimer une ligne
- [POST /orders/{id}/confirm](api/orders.md#confirmer-une-commande) - Confirmer une commande
- [POST /orders/{id}/cancel](api/orders.md#annuler-une-commande) - Annuler une commande

### Clients

- [GET /customers](api/customers.md#lister-tous-les-clients) - Lister tous les clients
- [GET /customers/{id}](api/customers.md#obtenir-un-client) - Obtenir un client par ID
- [POST /customers](api/customers.md#créer-un-client) - Créer un nouveau client
- [PUT /customers/{id}](api/customers.md#mettre-à-jour-un-client) - Mettre à jour un client
- [DELETE /customers/{id}](api/customers.md#supprimer-un-client) - Supprimer un client
- [POST /customers/{id}/activate](api/customers.md#activer-un-client) - Activer un client
- [POST /customers/{id}/deactivate](api/customers.md#désactiver-un-client) - Désactiver un client

### Fournisseurs

- [GET /suppliers](api/suppliers.md#lister-tous-les-fournisseurs) - Lister tous les fournisseurs
- [GET /suppliers/{id}](api/suppliers.md#obtenir-un-fournisseur) - Obtenir un fournisseur par ID
- [POST /suppliers](api/suppliers.md#créer-un-fournisseur) - Créer un nouveau fournisseur
- [PUT /suppliers/{id}](api/suppliers.md#mettre-à-jour-un-fournisseur) - Mettre à jour un fournisseur
- [DELETE /suppliers/{id}](api/suppliers.md#supprimer-un-fournisseur) - Supprimer un fournisseur
- [POST /suppliers/{id}/activate](api/suppliers.md#activer-un-fournisseur) - Activer un fournisseur
- [POST /suppliers/{id}/deactivate](api/suppliers.md#désactiver-un-fournisseur) - Désactiver un fournisseur

## Codes de statut HTTP

| Code | Signification | Utilisation |
|------|---------------|-------------|
| 200 | OK | Requête réussie (GET, PUT) |
| 201 | Created | Ressource créée (POST) |
| 204 | No Content | Opération réussie sans contenu de retour |
| 400 | Bad Request | Données invalides ou violation de règle métier |
| 404 | Not Found | Ressource non trouvée |
| 500 | Internal Server Error | Erreur serveur |

## Format des erreurs

Toutes les erreurs retournent un objet JSON avec les champs suivants :

```json
{
  "title": "Type d'erreur",
  "detail": "Message d'erreur détaillé"
}
```

### Exemples

**Erreur de validation (400)** :
```json
{
  "title": "Erreur métier",
  "detail": "Le prix doit être strictement positif."
}
```

**Ressource non trouvée (404)** :
```json
{
  "title": "Ressource introuvable",
  "detail": "Produit non trouvé."
}
```

## Authentification

!!! note "Note"
    Cette version de l'API ne nécessite pas d'authentification. C'est une application de démonstration.

## Swagger UI

L'API est documentée avec Swagger/OpenAPI. Accédez à l'interface interactive à :

```
http://localhost:5069/swagger
```

Swagger UI vous permet de :
- Visualiser tous les endpoints
- Tester les API directement
- Voir les schémas de requêtes et réponses
- Télécharger la spécification OpenAPI
