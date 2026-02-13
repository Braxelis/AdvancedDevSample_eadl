# Guide de démarrage

Ce guide vous aidera à démarrer rapidement avec le projet AdvancedDevSample.

## Prérequis

Avant de commencer, assurez-vous d'avoir installé :

- **.NET 10.0 SDK** ou supérieur - [Télécharger](https://dotnet.microsoft.com/download)
- **Git** - [Télécharger](https://git-scm.com/)
- Un éditeur de code :
    - [Visual Studio 2022](https://visualstudio.microsoft.com/)
    - [Visual Studio Code](https://code.visualstudio.com/)
    - [JetBrains Rider](https://www.jetbrains.com/rider/)

## Installation

### 1. Cloner le repository

```bash
git clone https://github.com/Braxelis/AdvancedDevSample_eadl.git
cd AdvancedDevSample_eadl
```

### 2. Restaurer les dépendances

```bash
dotnet restore
```

### 3. Compiler le projet

```bash
dotnet build
```

Si la compilation réussit, vous verrez :
```
Génération a réussi dans X.Xs
```

## Lancer l'application

### Mode développement

```bash
dotnet run --project AdvancedDevSample.Api/AdvancedDevSample.Api.csproj
```

L'application démarrera et affichera :
```
Now listening on: http://localhost:5069
Application started. Press Ctrl+C to shut down.
```

### Accéder à Swagger UI

Ouvrez votre navigateur et accédez à :
```
http://localhost:5069/swagger
```

Swagger UI vous permet de :
- Visualiser tous les endpoints disponibles
- Tester les API directement depuis le navigateur
- Voir les schémas de requêtes et réponses

## Premiers pas avec l'API

### 1. Créer un client

```bash
curl -X POST "http://localhost:5069/api/customers" \\
  -H "Content-Type: application/json" \\
  -d '{
    "name": "Jean Dupont",
    "email": "jean.dupont@example.com",
    "phone": "0123456789",
    "address": "123 Rue de Paris, 75001 Paris"
  }'
```

### 2. Créer un fournisseur

```bash
curl -X POST "http://localhost:5069/api/suppliers" \\
  -H "Content-Type: application/json" \\
  -d '{
    "name": "Fournisseur ABC",
    "email": "contact@abc.com",
    "phone": "0987654321",
    "address": "456 Avenue des Champs, 75008 Paris"
  }'
```

### 3. Créer un produit

```bash
curl -X POST "http://localhost:5069/api/products" \\
  -H "Content-Type: application/json" \\
  -d '{
    "initialPrice": 29.99
  }'
```

### 4. Créer une commande

```bash
curl -X POST "http://localhost:5069/api/orders" \\
  -H "Content-Type: application/json" \\
  -d '{}'
```

## Exécuter les tests

### Tous les tests

```bash
dotnet test
```

### Tests d'un projet spécifique

```bash
dotnet test AdvancedDevSample.Test/AdvancedDevSample.Test.csproj
```

### Avec couverture de code

```bash
dotnet test --collect:"XPlat Code Coverage"
```

## Structure du projet

```
AdvancedDevSample/
├── AdvancedDevSample.Domain/          # Couche domaine
│   ├── Entities/                      # Entités métier
│   ├── ValueObjects/                  # Value Objects
│   ├── Interfaces/                    # Interfaces de repositories
│   └── Exceptions/                    # Exceptions métier
├── AdvancedDevSample.Application/     # Couche application
│   ├── Services/                      # Services applicatifs
│   ├── DTOs/                          # Data Transfer Objects
│   └── Exceptions/                    # Exceptions applicatives
├── AdvancedDevSample.Infrastructure/  # Couche infrastructure
│   └── Repositories/                  # Implémentations des repositories
├── AdvancedDevSample.Api/             # Couche présentation
│   ├── Controllers/                   # Contrôleurs REST
│   └── Middlewares/                   # Middlewares personnalisés
└── AdvancedDevSample.Test/            # Tests
    ├── Domain/                        # Tests du domaine
    ├── Application/                   # Tests des services
    └── API/                           # Tests d'intégration
```

## Prochaines étapes

- Consultez l'[Architecture](architecture.md) pour comprendre la structure du projet
- Explorez le [Modèle de domaine](domain-model.md) pour comprendre les entités métier
- Lisez la [Référence API](api-reference.md) pour connaître tous les endpoints disponibles
- Suivez le [Guide de développement](development.md) pour contribuer au projet
