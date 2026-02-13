# AdvancedDevSample - Documentation

Bienvenue dans la documentation du projet **AdvancedDevSample**, un système de gestion de catalogue de produits avec commandes, clients et fournisseurs, construit selon les principes du **Domain-Driven Design (DDD)**.

## 🎯 Vue d'ensemble

AdvancedDevSample est une application de démonstration qui illustre les bonnes pratiques de développement en .NET avec une architecture en couches propre et maintenable.

### Fonctionnalités principales

- **Gestion des produits** : Créer, modifier, activer/désactiver des produits avec gestion des prix
- **Gestion des commandes** : Créer des commandes, ajouter/modifier des lignes, confirmer ou annuler
- **Gestion des clients** : CRUD complet pour les clients avec validation des données
- **Gestion des fournisseurs** : CRUD complet pour les fournisseurs

### Technologies utilisées

- **.NET 10.0** - Framework principal
- **ASP.NET Core** - API REST
- **Swagger/OpenAPI** - Documentation API interactive
- **xUnit** - Framework de tests unitaires
- **MkDocs Material** - Documentation technique

## 🏗️ Architecture

Le projet suit une architecture en couches basée sur les principes DDD :

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
- Un éditeur de code (Visual Studio, VS Code, Rider)

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

- [Guide de démarrage](getting-started.md) - Instructions détaillées pour démarrer
- [Architecture](architecture.md) - Explication de l'architecture DDD
- [Modèle de domaine](domain-model.md) - Entités et règles métier
- [Référence API](api-reference.md) - Documentation complète de l'API REST
- [Guide de développement](development.md) - Bonnes pratiques de développement
- [Tests](testing.md) - Guide des tests unitaires et d'intégration

## 🤝 Contribution

Ce projet est un exemple pédagogique pour l'apprentissage du DDD et des bonnes pratiques .NET.

## 📝 Licence

Projet éducatif - EADL1
