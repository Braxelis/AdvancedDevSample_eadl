# AdvancedDevSample

Système de gestion de catalogue de produits avec commandes, clients et fournisseurs, construit selon les principes du **Domain-Driven Design (DDD)**.

## 🎯 Fonctionnalités

- ✅ **Gestion des produits** : CRUD complet avec gestion des prix et activation/désactivation
- ✅ **Gestion des commandes** : Création, modification de lignes, confirmation et annulation
- ✅ **Gestion des clients** : CRUD complet avec validation des données
- ✅ **Gestion des fournisseurs** : CRUD complet avec validation des données
- ✅ **API REST** : Endpoints documentés avec Swagger/OpenAPI
- ✅ **Architecture DDD** : Séparation claire des couches (Domain, Application, Infrastructure, API)
- ✅ **Tests unitaires** : Couverture des entités et services
- ✅ **Documentation complète** : MkDocs avec Material theme

## 🏗️ Architecture

```
AdvancedDevSample/
├── AdvancedDevSample.Domain/       # Entités métier, Value Objects, Interfaces
├── AdvancedDevSample.Application/  # Services applicatifs, DTOs
├── AdvancedDevSample.Infrastructure/ # Repositories, Persistence
├── AdvancedDevSample.Api/          # Contrôleurs REST, Middlewares
└── AdvancedDevSample.Test/         # Tests unitaires et d'intégration
```

## 🚀 Démarrage rapide

### Prérequis

- .NET 10.0 SDK ou supérieur
- Git

### Installation

```bash
# Cloner le repository
git clone https://github.com/Braxelis/AdvancedDevSample_eadl.git
cd AdvancedDevSample_eadl

# Restaurer les dépendances
dotnet restore

# Compiler le projet
dotnet build

# Lancer l'application
dotnet run --project AdvancedDevSample.Api/AdvancedDevSample.Api.csproj
```

L'API sera accessible sur `http://localhost:5069` et Swagger UI sur `http://localhost:5069/swagger`.

## 📚 Documentation

La documentation complète est disponible avec MkDocs :

```bash
# Installer MkDocs et le thème Material
pip install mkdocs mkdocs-material

# Servir la documentation localement
mkdocs serve
```

Puis ouvrez `http://127.0.0.1:8000` dans votre navigateur.

### Documentation disponible

- **Guide de démarrage** : Instructions détaillées pour démarrer
- **Architecture** : Explication de l'architecture DDD
- **Modèle de domaine** : Entités et règles métier
- **Référence API** : Documentation complète de l'API REST
- **Guide de développement** : Bonnes pratiques de développement
- **Tests** : Guide des tests unitaires et d'intégration

## 🧪 Tests

```bash
# Exécuter tous les tests
dotnet test

# Avec couverture de code
dotnet test --collect:"XPlat Code Coverage"
```

## 🛠️ Technologies

- **.NET 10.0** - Framework principal
- **ASP.NET Core** - API REST
- **Swagger/OpenAPI** - Documentation API
- **xUnit** - Tests unitaires
- **MkDocs Material** - Documentation technique

## 📖 Principes DDD appliqués

- **Entités** : Product, Order, Customer, Supplier
- **Value Objects** : Price, OrderLine
- **Agrégats** : Order (agrégat racine avec OrderLines)
- **Repositories** : Interfaces dans le Domain, implémentations dans Infrastructure
- **Services applicatifs** : Orchestration des cas d'usage
- **Invariants métier** : Garantis par les entités

## 🤝 Contribution

Projet éducatif pour l'apprentissage du DDD et des bonnes pratiques .NET.

## 📝 Licence

Projet éducatif - EADL1
