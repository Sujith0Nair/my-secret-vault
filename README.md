# My Secret Vault

## Project Overview

"My Secret Vault" is a secure, professional-grade ASP.NET Core Web API designed for storing and sharing sensitive text information. Built with a focus on security and high-level architecture, it ensures that your data remains private and accessible only to authorized users.

## Core Features

*   **AES-256 Encryption:** All secrets are cryptographically encrypted before being stored in the database.
*   **Granular Access Control:** 
    *   **Owner:** Full control (CRUD + sharing management).
    *   **Delete:** Can view, edit, and delete the secret.
    *   **Edit:** Can view and modify the secret.
    *   **View:** Can only read the decrypted content.
*   **Secure Authentication:** Built on ASP.NET Core Identity with JWT Bearer tokens for stateless, secure API access.
*   **Input Validation:** Two-layered validation using FluentValidation (API layer) and Domain Guard Clauses (Domain layer).
*   **Standardized Error Handling:** Global Exception Handling using `IExceptionHandler` returning RFC 7807 compliant `ProblemDetails`.

## Architectural Design: Clean / Onion Architecture

The project follows Clean Architecture principles to ensure the core business logic is isolated from external frameworks and infrastructure concerns.

### 1. `SecretVault.Domain`
*   **Core Entities:** `Secret`, `User`, `SharePermission`.
*   **Rich Domain Model:** Business rules (invariants) are enforced within the entities themselves using the "Aggregate Root" pattern.
*   **Guard Clauses:** Custom validation utility to ensure the domain is always in a valid state.

### 2. `SecretVault.Application`
*   **Use Cases:** `SecretService` and `SharingService` orchestrate the application logic.
*   **Interfaces:** Defines contracts for Infrastructure (Repositories, Encryption, Identity).
*   **Validation:** FluentValidation rules for all incoming DTOs.

### 3. `SecretVault.Infrastructure`
*   **Persistence:** EF Core with SQL Server. Implements Repositories and a Unit of Work.
*   **Security Implementation:** Concrete `EncryptionService` (AES-256) and `IdentityService`.
*   **Identity:** Integration with ASP.NET Core Identity using GUID primary keys.

### 4. `SecretVault.Api`
*   **Entry Point:** RESTful Controllers and Middleware.
*   **Modern UI:** Scalar API Reference for superior developer experience and documentation.
*   **Configuration:** Clean `Program.cs` utilizing extension methods for service registration.

## Key Technology Stack

*   **Framework:** .NET 9.0 (ASP.NET Core)
*   **Database:** SQL Server via Entity Framework Core
*   **Security:** AES-256 Symmetric Encryption, JWT Bearer Tokens
*   **Validation:** FluentValidation
*   **Documentation:** Scalar (OpenAPI 3.1)

## Getting Started

### Prerequisites
*   [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
*   [SQL Server Express](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) or LocalDB
*   `dotnet-ef` tool: `dotnet tool install --global dotnet-ef`

### Setup
1.  **Clone the repository.**
2.  **Initialize User Secrets:**
    ```powershell
    dotnet user-secrets init --project SecretVault.Api/SecretVault.Api.csproj
    ```
3.  **Configure Secrets:**
    ```powershell
    # Connection String
    dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=(localdb)\MSSQLLocalDB;Database=SecretVaultDb;Trusted_Connection=True;MultipleActiveResultSets=True" --project SecretVault.Api/SecretVault.Api.csproj

    # Encryption Key (32-char random string)
    dotnet user-secrets set "Encryption:SecretKey" "YOUR_RANDOM_32_CHAR_STRING_HERE" --project SecretVault.Api/SecretVault.Api.csproj

    # JWT Key (32-char random string)
    dotnet user-secrets set "Jwt:Key" "YOUR_RANDOM_JWT_SIGNING_KEY_HERE" --project SecretVault.Api/SecretVault.Api.csproj
    dotnet user-secrets set "Jwt:Issuer" "https://localhost:7095" --project SecretVault.Api/SecretVault.Api.csproj
    dotnet user-secrets set "Jwt:Audience" "https://localhost:7095" --project SecretVault.Api/SecretVault.Api.csproj
    ```
4.  **Apply Database Migrations:**
    ```powershell
    dotnet ef database update --project SecretVault.Infrastructure/SecretVault.Infrastructure.csproj --startup-project SecretVault.Api/SecretVault.Api.csproj
    ```

### Running the Application
1.  Start the project: `dotnet run --project SecretVault.Api/SecretVault.Api.csproj`
2.  Open the Scalar documentation: `https://localhost:7095/scalar/v1`
3.  **Register** a user, **Login** to get a token, and start vaulting!
