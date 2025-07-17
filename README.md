# 🏋️‍♂️ GymManagement - Système de Gestion de Salle de Sport

## 📋 Vue d'ensemble

GymManagement est une plateforme complète de gestion de salle de sport développée avec **.NET 9**, utilisant une architecture moderne et scalable. Le projet comprend une application web (Blazor), une API REST, une application mobile (.NET MAUI), et une architecture microservices orchestrée avec **Microsoft Aspire**.

## 🏗️ Architecture du Système

### Architecture Générale

Le projet suit les principes de **Clean Architecture** et **Domain-Driven Design (DDD)** avec une séparation claire des responsabilités :

```
┌─────────────────────────────────────────────────────────────┐
│                    PRESENTATION LAYER                       │
├─────────────────┬─────────────────┬─────────────────────────┤
│   Blazor WebApp │   Mobile App    │      REST APIs          │
│   (Admin UI)    │   (.NET MAUI)   │   (Main + Auth)         │
└─────────────────┴─────────────────┴─────────────────────────┘
┌─────────────────────────────────────────────────────────────┐
│                   APPLICATION LAYER                         │
├─────────────────┬───────────────────────────────────────────┤
│    Services     │              Interfaces                   │
│  (Use Cases)    │             (Contracts)                   │
└─────────────────┴───────────────────────────────────────────┘
┌─────────────────────────────────────────────────────────────┐
│                     DOMAIN LAYER                            │
│            Entities • Value Objects • Rules                 │
└─────────────────────────────────────────────────────────────┘
┌─────────────────────────────────────────────────────────────┐
│                  INFRASTRUCTURE LAYER                       │
├─────────────────┬───────────────────────────────────────────┤
│   Persistence   │             Repositories                  │
│ (EntityFramework│            (Data Access)                  │
│   + PostgreSQL) │                                           │
└─────────────────┴───────────────────────────────────────────┘
```

## 📁 Structure des Projets

### 🎯 Couche de Présentation

#### **GymManagement.Presentation.WebApp** (Blazor Server)
- **Technologie** : Blazor Server avec .NET 9
- **Rôle** : Interface d'administration web
- **Fonctionnalités** :
  - Dashboard administrateur
  - Gestion des utilisateurs, clubs, plans d'abonnement
  - Interface coach (calendrier, gestion des coachings)
  - Authentification et autorisation
- **Composants clés** :
  - Pages Razor (Home, Users, Clubs, etc.)
  - Composants réutilisables (UserComponent, ClubComponent)
  - Gestion des popups et modales

#### **GymManagement.Presentation.MobileApp** (.NET MAUI)
- **Technologie** : .NET MAUI multi-plateforme
- **Plateformes** : iOS, Android, Windows, macOS
- **Rôle** : Application mobile pour les membres
- **Fonctionnalités** :
  - Inscription et gestion de profil
  - Réservation de coachings
  - Suivi des abonnements
  - Système de paiement intégré

#### **GymManagement.Presentation.Api** (API REST Principale)
- **Technologie** : ASP.NET Core Web API
- **Rôle** : API principale pour la gestion métier
- **Endpoints** :
  - `/api/users` - Gestion des utilisateurs
  - `/api/clubs` - Gestion des clubs
  - `/api/memberships` - Gestion des abonnements
  - `/api/coachings` - Gestion des séances de coaching

#### **GymManagement.Presentation.AuthApi** (API d'Authentification)
- **Technologie** : ASP.NET Core Web API
- **Rôle** : Service d'authentification dédié
- **Fonctionnalités** :
  - Authentification JWT
  - Gestion des tokens
  - Contrôle d'accès basé sur les rôles

### 🚀 Couche Application

#### **GymManagement.Application.Services**
- **Rôle** : Implémentation des cas d'usage métier
- **Contenus** :
  - Services applicatifs
  - Logique de validation
  - Orchestration des opérations

#### **GymManagement.Application.Interfaces**
- **Rôle** : Contrats et interfaces
- **Contenus** :
  - DTOs (Data Transfer Objects)
  - Interfaces de services
  - Contrats de repositoires

### 🏛️ Couche Domaine

#### **GymManagement.Domain**
- **Rôle** : Cœur métier de l'application
- **Entités principales** :
  - `User` - Utilisateurs (membres, coachs, admins)
  - `Club` - Salles de sport
  - `Membership` - Abonnements
  - `MembershipPlan` - Plans d'abonnement
  - `Coaching` - Séances de coaching
  - `CoachingPlan` - Plans de coaching
  - `PaymentDetail` - Détails de paiement
  - `Attendance` - Présences
- **Caractéristiques** :
  - Logique métier pure
  - Validation des règles business
  - Aucune dépendance externe

### 🔧 Couche Infrastructure

#### **GymManagement.Infrastructure.Persistence**
- **Technologie** : Entity Framework Core + PostgreSQL
- **Rôle** : Persistance des données
- **Contenus** :
  - `BaseGymDbContext` - Contexte principal
  - Modèles de données (UserModel, ClubModel, etc.)
  - Migrations Entity Framework
  - Configuration des relations

#### **GymManagement.Infrastructure.Repositories**
- **Rôle** : Implémentation des repositoires
- **Pattern** : Repository Pattern + Unit of Work
- **Fonctionnalités** :
  - Accès aux données abstrait
  - Requêtes optimisées
  - Gestion des transactions

### 🌐 Couche Partagée

#### **GymManagement.Shared.Core**
- **Rôle** : Éléments partagés entre toutes les couches
- **Contenus** :
  - `AuthManager` - Gestion de l'authentification
  - `AuthenticatedUser` - Modèle utilisateur connecté
  - Énumérations (`Role`, `Gender`, `PaymentMethod`, etc.)
  - Utilitaires et helpers

#### **GymManagement.Shared.Web.Core**
- **Rôle** : Éléments spécifiques au web
- **Contenus** :
  - Configurations de sécurité
  - Middlewares
  - Extensions web

### ☁️ Orchestration

#### **GymManagement.Aspire.AppHost**
- **Technologie** : Microsoft Aspire
- **Rôle** : Orchestration des microservices
- **Fonctionnalités** :
  - Configuration centralisée
  - Gestion des secrets (Azure Key Vault)
  - Déploiement coordonné

#### **GymManagement.Aspire.ServiceDefaults**
- **Rôle** : Configurations par défaut pour les services
- **Contenus** :
  - Logging standardisé
  - Monitoring et télémétrie
  - Configurations communes

## 🛠️ Technologies et Frameworks

### Frontend
- **Blazor Server** - Interface web administrative
- **.NET MAUI** - Application mobile cross-platform
- **Radzen Blazor** - Composants UI avancés
- **CSS3 + HTML5** - Styles personnalisés

### Backend
- **ASP.NET Core 9** - APIs REST
- **Entity Framework Core** - ORM
- **PostgreSQL** - Base de données principale
- **JWT** - Authentification stateless

### Infrastructure & DevOps
- **Microsoft Aspire** - Orchestration cloud-native
- **Azure Key Vault** - Gestion des secrets
- **Docker** - Conteneurisation
- **OpenAPI/Swagger** - Documentation API

## 🔐 Sécurité et Authentification

### Architecture d'Authentification
```
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│   Client App    │──-▶│   Auth API      │───▶│   Key Vault     │
│                 │    │   (JWT)         │    │   (Secrets)     │
└─────────────────┘    └─────────────────┘    └─────────────────┘
         │                       │                       │
         │                       ▼                       │
         │              ┌─────────────────┐              │
         └─────────────▶│   Main API      │◄─────────────┘
                        │  (Business)     │
                        └─────────────────┘
```

### Gestion des Rôles
- **Admin** : Accès complet au système
- **Manager** : Gestion d'un club spécifique
- **Coach** : Gestion des coachings et calendrier
- **Member** : Accès aux fonctionnalités membres (app mobile)

## 🎯 Fonctionnalités Principales

### Pour les Administrateurs
- 👥 **Gestion des utilisateurs** : CRUD complet, assignation de rôles
- 🏢 **Gestion des clubs** : Création, modification, assignation de managers
- 📋 **Plans d'abonnement** : Configuration des offres
- 📊 **Tableau de bord** : Statistiques et métriques
- 🎯 **Supervision des coachings** : Vue globale des activités

### Pour les Coachs
- 📅 **Calendrier personnel** : Planification des séances
- 🎯 **Gestion des coachings** : Suivi des clients
- 📝 **Plans de coaching** : Création de programmes personnalisés

### Pour les Membres (Mobile)
- 👤 **Profil personnel** : Gestion des informations
- 💳 **Abonnements** : Souscription et renouvellement
- 🎯 **Réservation coaching** : Booking de séances
- 💰 **Paiements** : Système de paiement intégré

## 🚀 Démarrage Rapide

### Prérequis
- .NET 9 SDK
- PostgreSQL
- Visual Studio 2022 ou VS Code
- Azure CLI (pour le Key Vault)

### Installation

1. **Cloner le repository**
```bash
git clone <repository-url>
cd GymManagement
```

2. **Configurer la base de données**
```bash
# Mettre à jour la chaîne de connexion dans appsettings.json
# Appliquer les migrations
dotnet ef database update --project GymManagement.Infrastructure.Persistence
```

3. **Configurer Azure Key Vault**
```bash
# Créer un Key Vault et configurer les clés :
# - GymDbConnection
# - JwtSecretKey
# - JwtIssuer
# - JwtAudience
```

4. **Lancer l'application**
```bash
# Via Aspire (recommandé)
dotnet run --project GymManagement.Aspire.AppHost

# Ou manuellement
dotnet run --project GymManagement.Presentation.Api
dotnet run --project GymManagement.Presentation.AuthApi
dotnet run --project GymManagement.Presentation.WebApp
```

### URLs par défaut
- **Web App** : https://localhost:7001
- **Main API** : https://localhost:7114
- **Auth API** : https://localhost:7090
- **Swagger Documentation** : https://localhost:7114/swagger

## 📊 Base de Données

### Schéma Principal
```sql
Users ──┬── Addresses
        ├── Memberships ──┬── MembershipPlans
        │                 └── PaymentDetails
        ├── Attendances ──── Clubs ──┬── Addresses
        └── Coachings ──── CoachingPlans ──┤
                                          └── PaymentDetails
```

### Tables Principales
- **users** : Informations utilisateurs
- **clubs** : Données des salles de sport
- **memberships** : Abonnements actifs
- **membership_plans** : Types d'abonnements
- **coachings** : Séances de coaching
- **coaching_plans** : Plans de coaching
- **attendances** : Présences en salle
- **payment_details** : Informations de paiement
- **addresses** : Adresses géographiques

## 🔄 Flux de Données

### Authentification
1. L'utilisateur se connecte via l'interface
2. Les credentials sont envoyées à l'AuthAPI
3. Un token JWT est généré et retourné
4. Le token est utilisé pour les appels API suivants

### Gestion des Coachings (Mobile)
1. Le membre parcourt les plans disponibles
2. Sélection d'un coach et créneau
3. Validation et paiement
4. Création du coaching en base
5. Notification au coach

## 📱 Application Mobile

### Flux Utilisateur
```
Login ──▶ Profile ──▶ Memberships ──▶ Coaching Flow
  │                     │                │
  │                     ▼                ▼
  └──▶ Settings    Subscription     Payment
                       Flow           Flow
```

### Pages Principales
- **MainPage** : Tableau de bord membre
- **ProfilePage** : Gestion du profil
- **MembershipsPage** : Abonnements actifs
- **CoachingsPage** : Réservations coaching
- **SubscriptionFlow** : Processus d'abonnement
- **CoachingFlow** : Réservation de coaching

## 🏷️ Patterns et Bonnes Pratiques

### Patterns Utilisés
- **Clean Architecture** : Séparation claire des couches
- **Repository Pattern** : Abstraction de l'accès aux données
- **Dependency Injection** : Inversion de contrôle
- **CQRS** : Séparation lecture/écriture (partiel)
- **Result Pattern** : Gestion des erreurs typée

### Conventions de Code
- **C# 13** avec nullable reference types
- **Async/await** pour toutes les opérations I/O
- **Extension methods** pour la réutilisabilité
- **Primary constructors** quand approprié