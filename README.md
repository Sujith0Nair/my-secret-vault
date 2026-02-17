# My Secret Vault

## Project Overview

"My Secret Vault" is an ASP.NET Core Web API project designed to provide a secure and manageable text storage service. Users can store sensitive text information, which will be encrypted at rest in the database. The system allows users to share access to their stored secrets with other users, granting granular permissions (view, edit, all, which includes delete). The owner of a secret retains full CRUD (Create, Read, Update, Delete) access.

## Core Features

*   **Secure API Endpoints:** For storing, retrieving, updating, and deleting secrets.
*   **User Authentication:** Secure user registration, login, and session management.
*   **Data Encryption:** Cryptographic encryption of user secrets before storage in the database.
*   **Access Control & Sharing:**
    *   Owners can share secrets with other users via email.
    *   Configurable access levels: View, Edit, All (View, Edit, Delete).
    *   Shared users can perform actions based on granted permissions.
*   **Owner Privileges:** Full CRUD operations for owned secrets.

## Architectural Design: Clean / Onion Architecture

The project adheres to the Clean/Onion Architecture principles, ensuring a clear separation of concerns, high maintainability, and testability. The architecture is structured into distinct layers, with dependencies strictly pointing inwards towards the core Domain.

### 1. `SecretVault.Domain` (Core Business Entities & Rules)
*   **Responsibility:** Contains the foundational business entities and core domain rules. It is entirely independent, with no dependencies on other layers.
*   **Contents:**
    *   `Secret` entity (Id, OwnerUserId, EncryptedContent, CreatedAt, etc.)
    *   `User` entity (base for ASP.NET Core Identity's `ApplicationUser`)
    *   `SharePermission` entity (SecretId, SharedWithUserId, AccessLevel)
    *   `AccessLevel` enum (View, Edit, Owner)

### 2. `SecretVault.Application` (Application-Specific Business Logic & Use Cases)
*   **Responsibility:** Orchestrates domain entities to implement specific application use cases. Defines interfaces for infrastructure concerns. Depends only on `SecretVault.Domain`.
*   **Contents:**
    *   **Interfaces:** `ISecretRepository`, `ISharingRepository`, `IEncryptionService`, `IUnitOfWork`.
    *   **Services/Use Cases:** `SecretService`, `SharingService`.
    *   **DTOs:** For input/output of use cases (e.g., `CreateSecretDto`, `ShareSecretDto`).
    *   **Validation:** Input validation logic.

### 3. `SecretVault.Infrastructure` (External Details & Implementations)
*   **Responsibility:** Provides concrete implementations for interfaces defined in the `Application` layer. Handles external concerns like databases, external services, and authentication. Depends on `SecretVault.Application`.
*   **Contents:**
    *   **Persistence:** Entity Framework Core DbContext (`SecretVaultDbContext`), concrete repository implementations (`SecretRepository`).
    *   **Services:** Implementation of `IEncryptionService` (e.g., using `System.Security.Cryptography`).
    *   **Identity:** Configuration and integration with ASP.NET Core Identity.

### 4. `SecretVault.Api` (Presentation Layer / Entry Point)
*   **Responsibility:** The ASP.NET Core Web API project. Handles HTTP requests, performs model binding and validation, and returns HTTP responses. Configures dependency injection and middleware. Depends on `SecretVault.Application` and `SecretVault.Infrastructure` (for DI setup).
*   **Contents:**
    *   **Controllers:** API endpoints (e.g., `SecretsController`).
    *   **Middleware:** Custom middleware.
    *   **Configuration:** `Program.cs` for service registration, middleware pipeline setup, and authentication/authorization.

## Key Technology Decisions

*   **Backend Framework:** ASP.NET Core Web API
*   **Architecture:** Clean / Onion Architecture
*   **Database:** (To be determined, likely SQL Server with Entity Framework Core)
*   **Authentication:** ASP.NET Core Identity with JWT Bearer Tokens
*   **JWT Key Storage:** `.NET User Secrets` will be used for storing sensitive JWT signing keys during development, and appropriate production-grade secrets management (e.g., Azure Key Vault, AWS Secrets Manager) will be implemented for deployment.

## Getting Started

*(Placeholder for future instructions on setup, building, and running the project.)*
