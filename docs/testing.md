# Guide des tests

Ce guide explique la stratégie de tests du projet AdvancedDevSample et comment exécuter les tests.

## Stratégie de tests

Le projet utilise une approche de tests en pyramide :

```
        /\
       /  \  Tests E2E (peu)
      /____\
     /      \  Tests d'intégration (moyennement)
    /________\
   /          \  Tests unitaires (beaucoup)
  /__________\
```

### Tests unitaires

Tests rapides et isolés qui vérifient une seule unité de code (méthode, classe).

**Objectifs** :
- Vérifier la logique métier
- Tester les cas limites
- Garantir les invariants

### Tests d'intégration

Tests qui vérifient l'interaction entre plusieurs composants.

**Objectifs** :
- Vérifier les services applicatifs
- Tester les repositories
- Valider les flux complets

## Organisation des tests

```
AdvancedDevSample.Test/
├── Domain/
│   └── Entities/
│       ├── ProductTest.cs
│       ├── OrderTest.cs
│       ├── CustomerTest.cs
│       └── SupplierTest.cs
├── Application/
│   ├── Services/
│   │   ├── ProductServiceTest.cs
│   │   ├── OrderServiceTest.cs
│   │   ├── CustomerServiceTest.cs
│   │   └── SupplierServiceTest.cs
│   └── Fakes/
│       ├── InMemoryProductRepositoryFake.cs
│       ├── InMemoryCustomerRepositoryFake.cs
│       └── InMemorySupplierRepositoryFake.cs
└── API/
    └── Controllers/
        └── (tests d'intégration à venir)
```

## Framework de tests

Le projet utilise **xUnit** comme framework de tests.

### Anatomie d'un test

```csharp
[Fact] // Attribut pour marquer un test
public void MethodName_Scenario_ExpectedBehavior()
{
    // Arrange - Préparer les données
    var product = new Product(new Price(100));
    
    // Act - Exécuter l'action
    product.Deactivate();
    
    // Assert - Vérifier le résultat
    Assert.False(product.IsActive);
}
```

## Exemples de tests

### Test d'entité

```csharp
public class ProductTest
{
    [Fact]
    public void Constructor_WithValidPrice_ShouldCreateProduct()
    {
        // Arrange
        var price = new Price(100);
        
        // Act
        var product = new Product(price);
        
        // Assert
        Assert.NotEqual(Guid.Empty, product.Id);
        Assert.Equal(100, product.Price.Value);
        Assert.True(product.IsActive);
    }
    
    [Fact]
    public void ChangePrice_WithInactiveProduct_ShouldThrowException()
    {
        // Arrange
        var product = new Product(new Price(100));
        product.Deactivate();
        
        // Act & Assert
        var exception = Assert.Throws<DomainException>(
            () => product.ChangePrice(new Price(200))
        );
        Assert.Contains("inactif", exception.Message.ToLower());
    }
}
```

### Test de service

```csharp
public class ProductServiceTest
{
    private readonly InMemoryProductRepositoryFake _repository;
    private readonly ProductService _service;
    
    public ProductServiceTest()
    {
        _repository = new InMemoryProductRepositoryFake();
        _service = new ProductService(_repository);
    }
    
    [Fact]
    public void Create_WithValidRequest_ShouldCreateProduct()
    {
        // Arrange
        var request = new CreateProductRequest { InitialPrice = 100 };
        
        // Act
        var result = _service.Create(request);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(100, result.Price);
        Assert.True(result.IsActive);
        
        // Vérifier que le produit est dans le repository
        var products = _repository.ListAll().ToList();
        Assert.Single(products);
    }
    
    [Fact]
    public void GetById_WithNonExistingProduct_ShouldThrowException()
    {
        // Arrange
        var nonExistingId = Guid.NewGuid();
        
        // Act & Assert
        var exception = Assert.Throws<ApplicationServiceException>(
            () => _service.GetById(nonExistingId)
        );
        Assert.Contains("non trouvé", exception.Message.ToLower());
    }
}
```

### Test avec données paramétrées

```csharp
[Theory]
[InlineData(0)]
[InlineData(-10)]
[InlineData(-0.01)]
public void Constructor_WithInvalidPrice_ShouldThrowException(decimal invalidPrice)
{
    // Act & Assert
    Assert.Throws<DomainException>(() => new Price(invalidPrice));
}
```

## Fake Repositories

Les fake repositories simulent le comportement des repositories réels pour les tests :

```csharp
public class InMemoryProductRepositoryFake : IProductRepository
{
    private readonly List<Product> _products = new();
    
    public void Add(Product product)
    {
        _products.Add(product);
    }
    
    public Product? GetById(Guid id)
    {
        return _products.FirstOrDefault(p => p.Id == id);
    }
    
    public IEnumerable<Product> ListAll()
    {
        return _products.ToList();
    }
    
    // Méthode utilitaire pour les tests
    public void Seed(params Product[] products)
    {
        foreach (var product in products)
        {
            _products.Add(product);
        }
    }
}
```

## Exécution des tests

### Tous les tests

```bash
dotnet test
```

**Sortie** :
```
Récapitulatif du test : total : 50; échec : 0; réussi : 50; ignoré : 0
```

### Tests d'un projet spécifique

```bash
dotnet test AdvancedDevSample.Test
```

### Tests avec verbosité

```bash
dotnet test --verbosity normal
```

### Tests d'une classe spécifique

```bash
dotnet test --filter "FullyQualifiedName~ProductTest"
```

### Tests avec couverture de code

```bash
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
```

## Couverture de code

### Objectifs de couverture

- **Entités du domaine** : > 90%
- **Services applicatifs** : > 80%
- **Contrôleurs** : > 70%
- **Global** : > 75%

### Visualiser la couverture

Avec l'extension **Coverage Gutters** dans VS Code :
1. Exécuter les tests avec couverture
2. Ouvrir un fichier de code
3. Les lignes couvertes apparaissent en vert

## Bonnes pratiques

### ✅ À faire

- **Tester les cas nominaux et les cas d'erreur**
- **Un test = un seul concept testé**
- **Noms de tests descriptifs** : `Method_Scenario_ExpectedResult`
- **Arrange-Act-Assert** : Structure claire
- **Tests indépendants** : Pas de dépendances entre tests
- **Utiliser des fakes** plutôt que des mocks complexes

### ❌ À éviter

- Tests qui dépendent de l'ordre d'exécution
- Tests avec logique complexe
- Tests qui testent le framework
- Trop de mocks (préférer les fakes)
- Tests sans assertions

## Assertions courantes

```csharp
// Égalité
Assert.Equal(expected, actual);
Assert.NotEqual(expected, actual);

// Booléens
Assert.True(condition);
Assert.False(condition);

// Nullabilité
Assert.Null(value);
Assert.NotNull(value);

// Collections
Assert.Empty(collection);
Assert.NotEmpty(collection);
Assert.Single(collection);
Assert.Contains(item, collection);

// Exceptions
Assert.Throws<ExceptionType>(() => action());

// Chaînes
Assert.Contains("substring", fullString);
Assert.StartsWith("prefix", fullString);
```

## Tests asynchrones

```csharp
[Fact]
public async Task GetByIdAsync_WithExistingProduct_ShouldReturnProduct()
{
    // Arrange
    var product = new Product(new Price(100));
    await _repository.AddAsync(product);
    
    // Act
    var result = await _service.GetByIdAsync(product.Id);
    
    // Assert
    Assert.NotNull(result);
    Assert.Equal(product.Id, result.Id);
}
```

## Résultats actuels

**État actuel du projet** :
- ✅ **50 tests** passent avec succès
- ✅ **0 échec**
- ✅ Couverture estimée : **~75%**

**Tests par catégorie** :
- Tests d'entités : 20 tests
- Tests de services : 20 tests
- Tests de Value Objects : 10 tests

## Prochaines étapes

1. Ajouter des tests d'intégration pour les contrôleurs
2. Augmenter la couverture de code à 80%
3. Ajouter des tests de performance
4. Implémenter des tests E2E avec WebApplicationFactory

## Ressources

- [xUnit Documentation](https://xunit.net/)
- [Testing Best Practices](https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices)
- [Test-Driven Development](https://martinfowler.com/bliki/TestDrivenDevelopment.html)
