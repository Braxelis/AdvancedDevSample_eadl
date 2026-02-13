# Guide de développement

Ce guide présente les bonnes pratiques et conventions pour contribuer au projet AdvancedDevSample.

## Configuration de l'environnement

### Prérequis

- **.NET SDK 10.0** ou supérieur
- **Visual Studio 2022** ou **VS Code** avec l'extension C#
- **Git** pour le contrôle de version
- **MkDocs** pour la documentation (optionnel)

### Installation

```bash
# Cloner le repository
git clone https://github.com/Braxelis/AdvancedDevSample_eadl.git
cd AdvancedDevSample_eadl

# Restaurer les dépendances
dotnet restore

# Compiler le projet
dotnet build

# Exécuter les tests
dotnet test
```

## Structure du projet

```
AdvancedDevSample/
├── AdvancedDevSample.Domain/       # Logique métier pure
├── AdvancedDevSample.Application/  # Cas d'usage et orchestration
├── AdvancedDevSample.Infrastructure/ # Accès aux données
├── AdvancedDevSample.Api/          # Points d'entrée HTTP
└── AdvancedDevSample.Test/         # Tests unitaires
```

## Standards de code

### Conventions de nommage

- **Classes** : PascalCase (`ProductService`, `Customer`)
- **Méthodes** : PascalCase (`GetById`, `CreateOrder`)
- **Variables locales** : camelCase (`productId`, `orderTotal`)
- **Constantes** : PascalCase (`MaxQuantity`)
- **Interfaces** : Préfixe `I` (`IProductRepository`)

### Organisation des fichiers

- **Un fichier par classe**
- **Nom du fichier = Nom de la classe** (`Product.cs` pour `class Product`)
- **Regroupement par fonctionnalité** dans les dossiers

### Commentaires

```csharp
/// <summary>
/// Crée une nouvelle commande pour un client.
/// </summary>
/// <param name="customerId">Identifiant du client</param>
/// <returns>La commande créée</returns>
public Order CreateOrder(Guid customerId)
{
    // Logique métier
}
```

## Principes DDD appliqués

### Entités

Les entités ont une identité unique et encapsulent la logique métier :

```csharp
public class Product
{
    public Guid Id { get; private set; }
    
    // Constructeur garantit les invariants
    public Product(Price price)
    {
        Id = Guid.NewGuid();
        Price = price ?? throw new ArgumentNullException(nameof(price));
    }
    
    // Méthodes métier
    public void ChangePrice(Price newPrice)
    {
        if (!IsActive)
            throw new DomainException("Le produit est inactif.");
        Price = newPrice;
    }
}
```

### Value Objects

Les Value Objects n'ont pas d'identité et sont immuables :

```csharp
public readonly struct Price
{
    public decimal Value { get; }
    
    public Price(decimal value)
    {
        if (value <= 0)
            throw new DomainException("Le prix doit être positif.");
        Value = value;
    }
}
```

### Repositories

Les repositories abstraient la persistence :

```csharp
public interface IProductRepository
{
    void Add(Product product);
    Product? GetById(Guid id);
    IEnumerable<Product> ListAll();
}
```

## Workflow de développement

### 1. Créer une branche

```bash
git checkout -b feature/nom-de-la-fonctionnalite
```

### 2. Développer avec TDD

1. **Red** : Écrire un test qui échoue
2. **Green** : Écrire le code minimal pour passer le test
3. **Refactor** : Améliorer le code

```csharp
[Fact]
public void ChangePrice_WithInactiveProduct_ShouldThrowException()
{
    // Arrange
    var product = new Product(new Price(100));
    product.Deactivate();
    
    // Act & Assert
    Assert.Throws<DomainException>(() => product.ChangePrice(new Price(200)));
}
```

### 3. Commiter régulièrement

```bash
git add .
git commit -m "feat: ajouter validation du prix pour les produits"
```

### 4. Pousser et créer une Pull Request

```bash
git push origin feature/nom-de-la-fonctionnalite
```

## Conventions de commits

Utiliser le format [Conventional Commits](https://www.conventionalcommits.org/) :

- `feat:` Nouvelle fonctionnalité
- `fix:` Correction de bug
- `docs:` Documentation
- `test:` Ajout de tests
- `refactor:` Refactoring sans changement de fonctionnalité
- `chore:` Tâches de maintenance

**Exemples** :
```
feat: ajouter l'entité Customer
fix: corriger la validation de l'email
docs: mettre à jour le guide d'architecture
test: ajouter tests pour OrderService
```

## Gestion des erreurs

### Exceptions métier

```csharp
public class DomainException : Exception
{
    public DomainException(string message) : base(message) { }
}
```

### Exceptions applicatives

```csharp
public class ApplicationServiceException : Exception
{
    public int StatusCode { get; set; }
    
    public ApplicationServiceException(string message) : base(message)
    {
        StatusCode = 404;
    }
}
```

### Middleware de gestion des erreurs

Le middleware `ExceptionHandlingMiddleware` capture toutes les exceptions et retourne des réponses HTTP appropriées.

## Bonnes pratiques

### ✅ À faire

- Valider les données à l'entrée (DTOs)
- Encapsuler la logique métier dans les entités
- Utiliser des Value Objects pour les concepts métier
- Écrire des tests pour chaque fonctionnalité
- Documenter les méthodes publiques

### ❌ À éviter

- Logique métier dans les contrôleurs
- Accès direct à la base de données depuis les services
- Exceptions génériques (`throw new Exception()`)
- Propriétés publiques avec setters sur les entités
- Tests qui dépendent de l'ordre d'exécution

## Outils de développement

### Exécution locale

```bash
# Lancer l'API
dotnet run --project AdvancedDevSample.Api

# Accéder à Swagger
# http://localhost:5069/swagger
```

### Tests

```bash
# Tous les tests
dotnet test

# Tests d'un projet spécifique
dotnet test AdvancedDevSample.Test

# Avec couverture de code
dotnet test /p:CollectCoverage=true
```

### Documentation

```bash
# Servir la documentation MkDocs
python -m mkdocs serve

# Générer le site statique
python -m mkdocs build
```

## Ressources

- [Domain-Driven Design](https://martinfowler.com/bliki/DomainDrivenDesign.html)
- [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [.NET Best Practices](https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/)
