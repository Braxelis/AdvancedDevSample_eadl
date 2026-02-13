# Guide d'intégration SonarCloud

SonarCloud est une plateforme d'analyse de code en ligne, gratuite pour les projets open source. Ce guide vous accompagne dans l'intégration de SonarCloud avec votre projet GitHub.

## Avantages de SonarCloud

- ✅ **Gratuit** pour les projets publics
- ✅ **Pas d'installation** locale nécessaire
- ✅ **Intégration GitHub** native
- ✅ **Analyse automatique** sur chaque PR
- ✅ **Badges** de qualité pour votre README

---

## Étape 1 : Créer un compte SonarCloud

1. Allez sur [sonarcloud.io](https://sonarcloud.io)
2. Cliquez sur **"Sign up"**
3. Choisissez **"Sign up with GitHub"**
4. Autorisez SonarCloud à accéder à votre compte GitHub

---

## Étape 2 : Importer votre projet

1. Une fois connecté, cliquez sur **"+"** puis **"Analyze new project"**
2. Sélectionnez votre organisation GitHub
3. Choisissez le repository **`AdvancedDevSample_eadl`**
4. Cliquez sur **"Set Up"**

### Configuration du projet

- **Project Key** : `Braxelis_AdvancedDevSample_eadl`
- **Organization** : Votre nom d'utilisateur GitHub
- **Main Branch** : `main` (ou `master`)

---

## Étape 3 : Configurer GitHub Actions

SonarCloud s'intègre parfaitement avec GitHub Actions pour une analyse automatique.

### 3.1 Créer le workflow

Créez le fichier `.github/workflows/sonarcloud.yml` :

```yaml
name: SonarCloud Analysis

on:
  push:
    branches:
      - main
  pull_request:
    types: [opened, synchronize, reopened]

jobs:
  sonarcloud:
    name: SonarCloud
    runs-on: windows-latest
    
    steps:
      - name: Checkout code
        uses: actions/checkout@v3
        with:
          fetch-depth: 0  # Shallow clones should be disabled for better analysis
      
      - name: Setup .NET
        uses: actions/setup-dotnet@v3
        with:
          dotnet-version: '10.0.x'
      
      - name: Cache SonarCloud packages
        uses: actions/cache@v3
        with:
          path: ~\sonar\cache
          key: ${{ runner.os }}-sonar
          restore-keys: ${{ runner.os }}-sonar
      
      - name: Cache SonarCloud scanner
        id: cache-sonar-scanner
        uses: actions/cache@v3
        with:
          path: .\.sonar\scanner
          key: ${{ runner.os }}-sonar-scanner
          restore-keys: ${{ runner.os }}-sonar-scanner
      
      - name: Install SonarCloud scanner
        if: steps.cache-sonar-scanner.outputs.cache-hit != 'true'
        shell: powershell
        run: |
          New-Item -Path .\.sonar\scanner -ItemType Directory
          dotnet tool update dotnet-sonarscanner --tool-path .\.sonar\scanner
      
      - name: Build and analyze
        env:
          GITHUB_TOKEN: ${{ secrets.GITHUB_TOKEN }}
          SONAR_TOKEN: ${{ secrets.SONAR_TOKEN }}
        shell: powershell
        run: |
          .\.sonar\scanner\dotnet-sonarscanner begin /k:"Braxelis_AdvancedDevSample_eadl" /o:"votre-organisation" /d:sonar.token="${{ secrets.SONAR_TOKEN }}" /d:sonar.host.url="https://sonarcloud.io"
          dotnet build
          dotnet test --no-build --verbosity normal
          .\.sonar\scanner\dotnet-sonarscanner end /d:sonar.token="${{ secrets.SONAR_TOKEN }}"
```

> [!IMPORTANT]
> Remplacez `votre-organisation` par votre nom d'organisation SonarCloud.

### 3.2 Ajouter le token SonarCloud

1. Dans SonarCloud, allez dans **My Account** → **Security**
2. Générez un nouveau token :
   - Name : `GitHub Actions`
   - Type : `User Token`
   - Copiez le token généré

3. Dans GitHub, allez dans votre repository :
   - **Settings** → **Secrets and variables** → **Actions**
   - Cliquez sur **"New repository secret"**
   - Name : `SONAR_TOKEN`
   - Value : Collez le token SonarCloud
   - Cliquez sur **"Add secret"**

---

## Étape 4 : Ajouter la configuration SonarCloud

Créez le fichier `sonar-project.properties` à la racine du projet :

```properties
# Project identification
sonar.projectKey=Braxelis_AdvancedDevSample_eadl
sonar.organization=votre-organisation

# Project metadata
sonar.projectName=AdvancedDevSample
sonar.projectVersion=1.0

# Source code
sonar.sources=AdvancedDevSample.Domain,AdvancedDevSample.Application,AdvancedDevSample.Infrastructure,AdvancedDevSample.Api
sonar.tests=AdvancedDevSample.Test

# Exclusions
sonar.exclusions=**/bin/**,**/obj/**,**/*.Designer.cs,**/Migrations/**
sonar.test.exclusions=**/bin/**,**/obj/**

# Code coverage (optionnel)
sonar.cs.vscoveragexml.reportsPaths=coverage.xml

# Language
sonar.language=cs
```

---

## Étape 5 : Ajouter un badge de qualité

Ajoutez ce badge dans votre `README.md` :

```markdown
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=Braxelis_AdvancedDevSample_eadl&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=Braxelis_AdvancedDevSample_eadl)

[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=Braxelis_AdvancedDevSample_eadl&metric=bugs)](https://sonarcloud.io/summary/new_code?id=Braxelis_AdvancedDevSample_eadl)
[![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=Braxelis_AdvancedDevSample_eadl&metric=code_smells)](https://sonarcloud.io/summary/new_code?id=Braxelis_AdvancedDevSample_eadl)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=Braxelis_AdvancedDevSample_eadl&metric=coverage)](https://sonarcloud.io/summary/new_code?id=Braxelis_AdvancedDevSample_eadl)
```

---

## Étape 6 : Lancer la première analyse

### Option 1 : Via GitHub Actions (Recommandé)

1. Commitez et pushez vos changements :
   ```bash
   git add .github/workflows/sonarcloud.yml sonar-project.properties
   git commit -m "Add SonarCloud integration"
   git push origin main
   ```

2. L'analyse se lancera automatiquement
3. Consultez les résultats sur [sonarcloud.io](https://sonarcloud.io)

### Option 2 : En local

```powershell
# Installer le scanner
dotnet tool install --global dotnet-sonarscanner

# Lancer l'analyse
dotnet sonarscanner begin /k:"Braxelis_AdvancedDevSample_eadl" /o:"votre-organisation" /d:sonar.host.url="https://sonarcloud.io" /d:sonar.token="VOTRE_TOKEN"

dotnet build

dotnet test

dotnet sonarscanner end /d:sonar.token="VOTRE_TOKEN"
```

---

## Analyse avec couverture de code

Pour inclure la couverture de code :

### 1. Installer dotnet-coverage

```powershell
dotnet tool install --global dotnet-coverage
```

### 2. Modifier le workflow GitHub Actions

Ajoutez cette étape avant `dotnet-sonarscanner end` :

```yaml
- name: Collect coverage
  shell: powershell
  run: |
    dotnet-coverage collect "dotnet test" -f xml -o "coverage.xml"
```

### 3. Mettre à jour sonar-project.properties

```properties
sonar.cs.vscoveragexml.reportsPaths=coverage.xml
```

---

## Configuration du Quality Gate

### Quality Gate par défaut

SonarCloud applique automatiquement un Quality Gate :

- ✅ **Coverage** : Nouveau code ≥ 80%
- ✅ **Duplications** : Nouveau code < 3%
- ✅ **Maintainability Rating** : A
- ✅ **Reliability Rating** : A
- ✅ **Security Rating** : A

### Personnaliser le Quality Gate

1. Dans SonarCloud, allez dans **Quality Gates**
2. Créez un nouveau Quality Gate ou modifiez l'existant
3. Ajoutez vos propres conditions

---

## Intégration avec les Pull Requests

SonarCloud analyse automatiquement chaque PR et ajoute :

- ✅ **Commentaires** sur les problèmes détectés
- ✅ **Status check** (pass/fail)
- ✅ **Décoration** de la PR avec les métriques

### Bloquer les PR qui échouent

Dans GitHub :
1. **Settings** → **Branches** → **Branch protection rules**
2. Sélectionnez `main`
3. Activez **"Require status checks to pass"**
4. Cochez **"SonarCloud Code Analysis"**

---

## Métriques disponibles

SonarCloud fournit :

| Métrique | Description |
|----------|-------------|
| **Bugs** | Erreurs de code |
| **Vulnerabilities** | Failles de sécurité |
| **Code Smells** | Problèmes de maintenabilité |
| **Coverage** | Couverture de tests |
| **Duplications** | Code dupliqué |
| **Technical Debt** | Dette technique estimée |
| **Maintainability** | Note de maintenabilité (A-E) |
| **Reliability** | Note de fiabilité (A-E) |
| **Security** | Note de sécurité (A-E) |

---

## Bonnes pratiques

### 1. Analyser à chaque commit

Le workflow GitHub Actions analyse automatiquement :
- ✅ Chaque push sur `main`
- ✅ Chaque Pull Request

### 2. Corriger les problèmes critiques

Priorité :
1. **Bugs** (surtout bloquants)
2. **Vulnerabilities** (sécurité)
3. **Code Smells** (maintenabilité)

### 3. Maintenir une bonne couverture

Objectif : **≥ 80%** de couverture de code

### 4. Surveiller la dette technique

Limitez la dette technique à **< 5%**

---

## Dépannage

### Problème : "Authentication failed"

- Vérifiez que le `SONAR_TOKEN` est correct dans GitHub Secrets
- Régénérez un nouveau token si nécessaire

### Problème : "Project not found"

- Vérifiez que le `projectKey` correspond exactement
- Format : `organisation_repository`

### Problème : "Coverage not displayed"

- Vérifiez que `coverage.xml` est généré
- Vérifiez le chemin dans `sonar-project.properties`

---

## Ressources

- [Documentation SonarCloud](https://docs.sonarcloud.io/)
- [SonarCloud pour .NET](https://docs.sonarcloud.io/advanced-setup/languages/csharp/)
- [GitHub Actions](https://docs.github.com/en/actions)
- [Quality Gates](https://docs.sonarcloud.io/improving/quality-gates/)

---

## Résumé des étapes

1. ✅ Créer un compte SonarCloud
2. ✅ Importer le projet GitHub
3. ✅ Créer le workflow `.github/workflows/sonarcloud.yml`
4. ✅ Ajouter le `SONAR_TOKEN` dans GitHub Secrets
5. ✅ Créer `sonar-project.properties`
6. ✅ Ajouter les badges dans README.md
7. ✅ Pusher et vérifier l'analyse
8. 📊 Consulter les résultats sur SonarCloud

---

## Prochaines étapes

1. Configurer SonarCloud selon ce guide
2. Lancer la première analyse
3. Corriger les problèmes identifiés
4. Mettre à jour `quality.md` avec les métriques réelles
5. Configurer le Quality Gate personnalisé
