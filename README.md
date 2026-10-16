# Crowdsourced Ideation Solution (CIS) — Enterprise Microservices Ecosystem

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-12.0-239120?logo=csharp)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![Java](https://img.shields.io/badge/Java-21-ED8B00?logo=openjdk)](https://www.oracle.com/java/)
[![Spring Boot](https://img.shields.io/badge/Spring%20Boot-3.x-6DB33F?logo=springboot)](https://spring.io/projects/spring-boot)
[![MySQL](https://img.shields.io/badge/MySQL-8.0-4479A1?logo=mysql)](https://www.mysql.com/)
[![Docker](https://img.shields.io/badge/Docker-Enabled-2496ED?logo=docker)](https://www.docker.com/)
[![OpenAPI](https://img.shields.io/badge/OpenAPI-3.0-6BA539?logo=swagger)](https://swagger.io/)
[![Keycloak](https://img.shields.io/badge/Auth-Keycloak%20OAuth2-4D90FE?logo=redhat)](https://www.keycloak.org/)

---

## Overview

The **Crowdsourced Ideation Solution (CIS)** is an enterprise distributed microservices platform designed to drive collaborative problem-solving, topic curation, idea submission, and community voting within organizations.

Built under the **API-First** paradigm and strict **Clean Architecture / S.O.L.I.D.** principles, the ecosystem demonstrates seamless interoperability between a modern **.NET C#** ideation service and a core **Java Spring Boot** identity/user management system, bound by formal OpenAPI contracts and secured via **Keycloak OAuth2 / OpenID Connect**.

---

## High-Level System Architecture

```text
                               ┌─────────────────────────┐
                               │   API Consumers / CLI   │
                               │  (Postman / users-cli)  │
                               └────────────┬────────────┘
                                            │
                     ┌──────────────────────┴──────────────────────┐
                     │ (HTTP / Bearer JWT)                         │ (HTTP / Bearer JWT)
                     ▼                                             ▼
        ┌─────────────────────────┐                   ┌─────────────────────────┐
        │      CIS API (.NET)     │                   │     Users API (Java)    │
        │   (Port 8002 / Mock)    │                   │       (Port 8001)       │
        │                         │                   │                         │
        │  • ASP.NET Core 8       │                   │  • Spring Boot 3        │
        │  • Clean Architecture   │                   │  • Clean Architecture   │
        │  • Entity Framework Core│                   │  • JPA / Hibernate      │
        │  • Exception Middleware │                   │  • OAuth2 Resource Srvr │
        └────────────┬────────────┘                   └────────────┬────────────┘
                     │                                             │
          ┌──────────┴──────────┐                       ┌──────────┴──────────┐
          ▼                     ▼                       ▼                     ▼
┌──────────────────┐  ┌──────────────────┐    ┌──────────────────┐  ┌──────────────────┐
│  MySQL Database  │  │ Liquibase Runner │    │  MySQL Database  │  │  Keycloak IdP    │
│   (sd3_cisdb)    │  │ (Docker Compose) │    │     (sd3db)      │  │   (Port 8080)    │
└──────────────────┘  └──────────────────┘    └──────────────────┘  └──────────────────┘
```

---

## Repository Structure & Components

This monorepo consolidates all microservices, specifications, and tooling powering the platform:

```text
Crowdsourced-Ideation-Solution/
├── cis-api/             # .NET 8 C# Core Ideation Microservice
├── cis-api-swagger/     # OpenAPI 3.0 Contract & Prism Mock Server for CIS (Port 4020)
├── users-api/           # Java 21 / Spring Boot Identity Microservice (Port 8001)
├── users-api-swagger/   # OpenAPI 3.0 Contract & Prism Mock Server for Users API (Port 4010)
├── users-cli/           # Python/Java CLI Utility for platform administration
└── README.md            # Master Architecture & Getting Started Guide
```

---

### 1. cis-api (.NET C# Backend)
The core ideation microservice built on **.NET 8** and **C#**:
* **Layered Clean Architecture:** Divided into `Cis.Core.Domain`, `Cis.Core.Application`, `Cis.Infrastructure.MySql`, `Cis.Infrastructure.Ioc`, and `Cis.Server` (Web API).
* **Domain Model:** Topics, Ideas, and Votes with strict encapsulation, business validation, and domain events.
* **ORM & Persistence:** **Entity Framework Core (Pomelo MySQL provider)** implementing the Repository Pattern.
* **Database-as-Code:** Schema evolution managed through automated **Liquibase** containers executing repeatable, idempotent SQL migrations against `sd3_cisdb`.
* **Middlewares:** Centralized `ExceptionHandlingMiddleware` for consistent error structures (`RFC 7807`).

### 2. cis-api-swagger (API-First Specification & Mock)
The single source of truth for the CIS service:
* **OpenAPI 3.0.3 Specification (`openapi.yaml`):** Comprehensive schema definition for Topics, Ideas, and Votes, including query filters, error schemas, and Bearer auth.
* **Stoplight Prism Mock Server:** Containerized mock server running on port `4020` allowing immediate zero-code validation and client integration.

### 3. users-api (Java Spring Boot Identity Service)
Enterprise user management service built with **Java 21**:
* **Hexagonal / Clean Architecture:** Completely isolated domain (`domain`, `application`, `infrastructure/mysql`, `presentation`).
* **Security:** **Spring Security OAuth2 Resource Server** validating JWT tokens issued by Keycloak (`local-dev` realm).
* **Persistence:** MySQL persistence with Spring Data JPA / Hibernate and automated timestamp tracking (`createdAt`, `updatedAt`).

### 4. users-api-swagger (Users Service Contract)
The formal OpenAPI contract and Prism simulation environment (port `4010`) for all user lifecycle operations (Create, Read, Update, Delete).

### 5. users-cli (Console Client Tool)
A command-line administration utility supporting developer operations, database bootstrapping, and automated testing against the endpoints.

---

## Key Architectural & Engineering Highlights

| Practice | Implementation in Solution |
| :--- | :--- |
| **API-First Design** | Formal contract specification and mock servers deployed prior to any backend implementation, ensuring contractual alignment across distributed teams. |
| **Clean Architecture** | Core domain models are 100% pure (zero framework dependencies). Infrastructure concerns (JPA, EF Core, HTTP) depend strictly inward. |
| **S.O.L.I.D. Principles** | Single Responsibility per service/controller, Open/Closed for extensible vote calculation, Liskov Substitution across repository interfaces, and Dependency Inversion throughout. |
| **Idempotent Migrations** | Database changes versioned via Liquibase changesets (`runOnChange:true`, `CREATE TABLE IF NOT EXISTS`), eliminating manual SQL scripts. |
| **Zero-Trust Security** | Global asymmetric JWT Bearer authentication enforced via Keycloak IdP with role-based access control (`ROLE_user`). |
| **Automated Testing** | Multi-layered testing strategy combining unit tests (**xUnit**, **Moq**, **FluentAssertions**, **JUnit 5**, **Mockito**), integration testing, and CI/CD quality gates. |

---

## Quick Start Guide

### Prerequisites
* [Docker & Docker Compose](https://www.docker.com/)
* [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
* [Java Development Kit (JDK 21)](https://adoptium.net/)
* [Maven 3.9+](https://maven.apache.org/)

### 1. Launch Keycloak & Local Databases
```bash
# Start MySQL instance and Keycloak Identity Server
cd users-api
docker compose up -d
```
Keycloak will be accessible at `http://localhost:8080` (Realm: `local-dev`).

### 2. Run Database Migrations (Liquibase)
```bash
# Execute automated schema migrations for sd3_cisdb
cd ../cis-api/Data
docker compose up
```

### 3. Run the Mock API Servers
```bash
# Start CIS API mock server (port 4020)
cd ../../cis-api-swagger
docker compose up -d

# Start Users API mock server (port 4010)
cd ../users-api-swagger
docker compose up -d
```

### 4. Run the Backend Services
* **Users API (Java):**
  ```bash
  cd ../users-api
  mvn clean install -DskipTests
  mvn --projects presentation spring-boot:run
  ```
* **CIS API (.NET):**
  ```bash
  cd ../cis-api
  dotnet restore
  dotnet run --project src/Cis.Server
  ```

---

## Author

**Maycon Vyctor**  
*Software Engineering | Distributed Systems & Cloud Architecture*  
* [GitHub Profile](https://github.com/MayconVyctor)

---
*Developed as part of the Software Engineering Capstone Project — Jala University.*
