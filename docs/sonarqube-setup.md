# Guide d'installation et configuration SonarQube

Ce guide vous accompagne dans l'installation de SonarQube et l'analyse de votre projet .NET.

## Prérequis

- Java 17 ou supérieur
- .NET SDK 10.0
- Docker (optionnel, recommandé pour une installation facile)

## Option 1 : Installation avec Docker (Recommandé)

### Étape 1 : Installer Docker Desktop

1. Téléchargez Docker Desktop depuis [docker.com](https://www.docker.com/products/docker-desktop/)
2. Installez et démarrez Docker Desktop

### Étape 2 : Lancer SonarQube

```bash
# Télécharger et démarrer SonarQube
docker run -d --name sonarqube -p 9000:9000 sonarqube:lts-community

# Vérifier que le conteneur fonctionne
docker ps
```

### Étape 3 : Accéder à SonarQube

1. Ouvrez votre navigateur : `http://localhost:9000`
2. Connexion par défaut :
   - **Username** : `admin`
   - **Password** : `admin`
3. Changez le mot de passe lors de la première connexion

---

## Option 2 : Installation manuelle (Windows)

### Étape 1 : Installer Java

```powershell
# Vérifier si Java est installé
java -version

# Si Java n'est pas installé, téléchargez-le depuis :
# https://adoptium.net/temurin/releases/
```

### Étape 2 : Télécharger SonarQube

1. Téléchargez SonarQube Community Edition : [sonarqube.org/downloads](https://www.sonarqube.org/downloads/)
2. Extrayez l'archive dans `C:\SonarQube`

### Étape 3 : Démarrer SonarQube

```powershell
# Naviguer vers le dossier SonarQube
cd C:\SonarQube\bin\windows-x86-64

# Démarrer SonarQube
.\StartSonar.bat
```

### Étape 4 : Accéder à SonarQube

1. Attendez quelques minutes que SonarQube démarre
2. Ouvrez `http://localhost:9000`
3. Connexion : `admin` / `admin`

---

## Configuration du projet .NET

### Étape 1 : Installer le scanner SonarQube

```powershell
# Installer le scanner global .NET
dotnet tool install --global dotnet-sonarscanner

# Vérifier l'installation
dotnet sonarscanner --version
```

### Étape 2 : Créer un projet dans SonarQube

1. Connectez-vous à SonarQube (`http://localhost:9000`)
2. Cliquez sur **"Create Project"**
3. Choisissez **"Manually"**
4. Remplissez :
   - **Project key** : `AdvancedDevSample`
   - **Display name** : `AdvancedDevSample`
5. Cliquez sur **"Set Up"**
6. Choisissez **"Locally"**
7. Générez un **token** :
   - Token name : `AdvancedDevSample-Token`
   - Copiez le token généré (vous en aurez besoin)

### Étape 3 : Analyser le projet

```powershell
# Naviguer vers le dossier du projet
cd "c:\DATA\EADL1\Developpement Avancé\Projets\AdvancedDevSample_eadl"

# Commencer l'analyse
dotnet sonarscanner begin /k:"AdvancedDevSample" /d:sonar.host.url="http://localhost:9000" /d:sonar.token="VOTRE_TOKEN_ICI"

# Build du projet
dotnet build

# Terminer l'analyse
dotnet sonarscanner end /d:sonar.token="VOTRE_TOKEN_ICI"
```

> [!IMPORTANT]
> Remplacez `VOTRE_TOKEN_ICI` par le token généré à l'étape 2.

---

## Analyse avec couverture de code

Pour inclure la couverture de code dans l'analyse :

```powershell
# Installer l'outil de couverture
dotnet tool install --global dotnet-coverage

# Commencer l'analyse SonarQube
dotnet sonarscanner begin /k:"AdvancedDevSample" /d:sonar.host.url="http://localhost:9000" /d:sonar.token="VOTRE_TOKEN" /d:sonar.cs.vscoveragexml.reportsPaths=coverage.xml

# Build
dotnet build

# Exécuter les tests avec couverture
dotnet-coverage collect "dotnet test" -f xml -o "coverage.xml"

# Terminer l'analyse
dotnet sonarscanner end /d:sonar.token="VOTRE_TOKEN"
```

---

## Interpréter les résultats

### Métriques principales

Une fois l'analyse terminée, SonarQube affiche :

1. **Bugs** : Erreurs de code qui peuvent causer des problèmes
2. **Vulnerabilities** : Failles de sécurité potentielles
3. **Code Smells** : Problèmes de maintenabilité
4. **Coverage** : Pourcentage de code couvert par les tests
5. **Duplications** : Code dupliqué

### Quality Gate

Le **Quality Gate** détermine si votre code passe les critères de qualité :
- ✅ **Passed** : Le code respecte tous les critères
- ❌ **Failed** : Des problèmes doivent être corrigés

---

## Bonnes pratiques

### 1. Analyser régulièrement

Intégrez l'analyse SonarQube dans votre workflow :

```powershell
# Créer un script d'analyse
# analyze.ps1

$token = "VOTRE_TOKEN"
$projectKey = "AdvancedDevSample"

dotnet sonarscanner begin /k:$projectKey /d:sonar.host.url="http://localhost:9000" /d:sonar.token=$token
dotnet build
dotnet test
dotnet sonarscanner end /d:sonar.token=$token
```

### 2. Corriger les problèmes critiques en priorité

Ordre de priorité :
1. **Bugs** (surtout critiques et bloquants)
2. **Vulnerabilities** (sécurité)
3. **Code Smells** (maintenabilité)

### 3. Maintenir une bonne couverture de tests

Objectif recommandé : **80% de couverture**

---

## Intégration CI/CD

### GitHub Actions

Créez `.github/workflows/sonarqube.yml` :

```yaml
name: SonarQube Analysis

on:
  push:
    branches: [ main ]
  pull_request:
    branches: [ main ]

jobs:
  sonarqube:
    runs-on: windows-latest
    steps:
      - uses: actions/checkout@v3
      
      - name: Setup .NET
        uses: actions/setup-dotnet@v3
        with:
          dotnet-version: '10.0.x'
      
      - name: Install SonarScanner
        run: dotnet tool install --global dotnet-sonarscanner
      
      - name: Begin SonarQube Analysis
        run: |
          dotnet sonarscanner begin /k:"AdvancedDevSample" /d:sonar.host.url="${{ secrets.SONAR_HOST_URL }}" /d:sonar.token="${{ secrets.SONAR_TOKEN }}"
      
      - name: Build
        run: dotnet build
      
      - name: Test
        run: dotnet test
      
      - name: End SonarQube Analysis
        run: dotnet sonarscanner end /d:sonar.token="${{ secrets.SONAR_TOKEN }}"
```

---

## Dépannage

### Problème : "Java not found"

```powershell
# Vérifier l'installation de Java
java -version

# Si Java n'est pas trouvé, ajoutez-le au PATH
$env:JAVA_HOME = "C:\Program Files\Eclipse Adoptium\jdk-17.0.x"
$env:PATH += ";$env:JAVA_HOME\bin"
```

### Problème : "SonarQube server not found"

```powershell
# Vérifier que SonarQube fonctionne
curl http://localhost:9000

# Si Docker, vérifier le conteneur
docker ps
docker logs sonarqube
```

### Problème : "Authentication failed"

- Vérifiez que le token est correct
- Régénérez un nouveau token si nécessaire
- Vérifiez que l'utilisateur a les permissions nécessaires

---

## Ressources

- [Documentation SonarQube](https://docs.sonarqube.org/latest/)
- [SonarQube pour .NET](https://docs.sonarqube.org/latest/analyzing-source-code/languages/csharp/)
- [Quality Gates](https://docs.sonarqube.org/latest/user-guide/quality-gates/)

---

## Prochaines étapes

1. ✅ Installer SonarQube
2. ✅ Configurer le projet
3. ✅ Lancer la première analyse
4. 📊 Consulter les résultats dans `http://localhost:9000`
5. 🔧 Corriger les problèmes identifiés
6. 📝 Documenter les métriques dans `quality.md`
