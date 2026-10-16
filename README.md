# Crowdsourced Ideation Solution (CIS) — Ecossistema de Microsserviços

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-12.0-239120?logo=csharp)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![Java](https://img.shields.io/badge/Java-21-ED8B00?logo=openjdk)](https://www.oracle.com/java/)
[![Spring Boot](https://img.shields.io/badge/Spring%20Boot-3.x-6DB33F?logo=springboot)](https://spring.io/projects/spring-boot)
[![MySQL](https://img.shields.io/badge/MySQL-8.0-4479A1?logo=mysql)](https://www.mysql.com/)
[![Docker](https://img.shields.io/badge/Docker-Enabled-2496ED?logo=docker)](https://www.docker.com/)
[![OpenAPI](https://img.shields.io/badge/OpenAPI-3.0-6BA539?logo=swagger)](https://swagger.io/)
[![Keycloak](https://img.shields.io/badge/Auth-Keycloak%20OAuth2-4D90FE?logo=redhat)](https://www.keycloak.org/)

---

## Visão Geral

O **Crowdsourced Ideation Solution (CIS)** é uma plataforma corporativa distribuída em microsserviços, projetada para impulsionar a resolução colaborativa de problemas, curadoria de tópicos, submissão de ideias e votação comunitária.

Construído sob o paradigma **API-First** e princípios rigorosos de **Clean Architecture / S.O.L.I.D.**, o ecossistema demonstra interoperabilidade consistente entre um microsserviço de ideação em **.NET C#** e um serviço central de gestão de identidades e usuários em **Java Spring Boot**, integrados por contratos OpenAPI formais e protegidos via **Keycloak OAuth2 / OpenID Connect**.

> **Contexto do Projeto:** Este ecossistema é desenvolvido de forma colaborativa em dupla para a matéria de **Desenvolvimento de Software 3**, com o fluxo de trabalho e versionamento principal distribuído em repositórios dedicados no GitLab. Este repositório no GitHub atua como uma consolidação em monorepo portando e unificando todos os módulos para demonstração técnica e portfólio.

---

## Arquitetura do Sistema

```text
                                ┌─────────────────────────┐
                                │   Clientes API / CLI    │
                                │  (Postman / users-cli)  │
                                └────────────┬────────────┘
                                             │
                      ┌──────────────────────┴──────────────────────┐
                      │ (HTTP / Bearer JWT)                         │ (HTTP / Bearer JWT)
                      ▼                                             ▼
         ┌─────────────────────────┐                   ┌─────────────────────────┐
         │      CIS API (.NET)     │                   │     Users API (Java)    │
         │   (Porta 8002 / Mock)   │                   │       (Porta 8001)      │
         │                         │                   │                         │
         │  • ASP.NET Core 8       │                   │  • Spring Boot 3        │
         │  • Clean Architecture   │                   │  • Clean Architecture   │
         │  • Entity Framework Core│                   │  • JPA / Hibernate      │
         │  • Middleware de Erros  │                   │  • OAuth2 Resource Srvr │
         └────────────┬────────────┘                   └────────────┬────────────┘
                      │                                             │
           ┌──────────┴──────────┐                       ┌──────────┴──────────┐
           ▼                     ▼                       ▼                     ▼
 ┌──────────────────┐  ┌──────────────────┐    ┌──────────────────┐  ┌──────────────────┐
 │  Banco de Dados  │  │ Runner Liquibase │    │  Banco de Dados  │  │   IdP Keycloak   │
 │ MySQL(sd3_cisdb) │  │ (Docker Compose) │    │  MySQL (sd3db)   │  │   (Porta 8080)   │
 └──────────────────┘  └──────────────────┘    └──────────────────┘  └──────────────────┘
```

---

## Estrutura do Repositório e Componentes

Este monorepo consolida todos os microsserviços, especificações e ferramentas que compõem o ecossistema:

```text
Crowdsourced-Ideation-Solution/
├── cis-api/             # Microsserviço de ideação em .NET 8 C#
├── cis-api-swagger/     # Contrato OpenAPI 3.0 e servidor de Mock Prism da CIS API (Porta 4020)
├── users-api/           # Microsserviço de usuários em Java 21 / Spring Boot (Porta 8001)
├── users-api-swagger/   # Contrato OpenAPI 3.0 e servidor de Mock Prism da Users API (Porta 4010)
├── users-cli/           # Utilitário CLI para administração da plataforma
└── README.md            # Arquitetura e guia de inicialização do ecossistema
```

---

### 1. cis-api (Backend .NET C#)
Microsserviço principal de ideação desenvolvido com **.NET 8** e **C#**:
* **Clean Architecture em Camadas:** Organizado em `Cis.Core.Domain`, `Cis.Core.Application`, `Cis.Infrastructure.MySql`, `Cis.Infrastructure.Ioc` e `Cis.Server` (Web API).
* **Modelo de Domínio:** Tópicos, Ideias e Votos com encapsulamento estrito, regras de negócio e eventos de domínio.
* **ORM e Persistência:** **Entity Framework Core (provedor Pomelo MySQL)** implementando o padrão Repository.
* **Database-as-Code:** Evolução de esquema controlada através de containers automatizados do **Liquibase**, executando migrações SQL idempotentes contra a base `sd3_cisdb`.
* **Middlewares:** Centralização de erros e exceções via `ExceptionHandlingMiddleware` com retorno em formato padronizado (`RFC 7807`).

### 2. cis-api-swagger (Especificação API-First e Mock)
Fonte única da verdade contratual para o serviço de ideação:
* **Especificação OpenAPI 3.0.3 (`openapi.yaml`):** Definição rigorosa de esquemas para Tópicos, Ideias e Votos, filtros de paginação, esquemas de erro e autenticação Bearer.
* **Servidor de Mock Stoplight Prism:** Mock conteinerizado rodando na porta `4020`, permitindo testes e integração entre serviços antes mesmo do deploy do backend.

### 3. users-api (Serviço de Identidade Java Spring Boot)
Microsserviço de gerenciamento de identidades e usuários desenvolvido com **Java 21**:
* **Arquitetura Limpa / Hexagonal:** Isolamento completo do domínio em relação a frameworks (`domain`, `application`, `infrastructure/mysql`, `presentation`).
* **Segurança:** **Spring Security OAuth2 Resource Server** com validação de tokens JWT emitidos pelo Keycloak (realm `local-dev`).
* **Persistência:** Persistência MySQL via Spring Data JPA / Hibernate com auditoria temporal automática (`createdAt`, `updatedAt`).

### 4. users-api-swagger (Contrato da Users API)
Contrato formal em OpenAPI e ambiente simulado via Prism (porta `4010`) para operações de ciclo de vida de usuários (Criação, Leitura, Atualização, Exclusão).

### 5. users-cli (Ferramenta de Linha de Comando)
Utilitário de linha de comando voltado para operações de desenvolvimento, carga inicial de dados e testes diretos contra os endpoints.

---

## Destaques de Engenharia e Arquitetura

| Prática | Implementação no Projeto |
| :--- | :--- |
| **Design API-First** | Especificações formais e servidores de mock criados antes da implementação dos backends, garantindo alinhamento contratual imediato entre serviços. |
| **Clean Architecture** | Modelos de domínio puros sem acoplamento a bibliotecas externas. Camadas de infraestrutura (JPA, EF Core, HTTP) apontam exclusivamente para dentro. |
| **Princípios S.O.L.I.D.** | Responsabilidade única por serviço/controller, Open/Closed no cálculo de votos, Substituição de Liskov nos repositórios e Inversão de Dependência total. |
| **Migrações Idempotentes** | Controle de alterações de banco versionado via Liquibase changesets (`runOnChange:true`, `CREATE TABLE IF NOT EXISTS`), sem scripts manuais. |
| **Segurança Zero-Trust** | Autenticação global por tokens JWT assinados pelo Keycloak IdP com controle de permissões por roles (`ROLE_user`). |
| **Testes Automatizados** | Estratégia de testes combinando testes unitários (**xUnit**, **Moq**, **FluentAssertions**, **JUnit 5**, **Mockito**), testes de integração e validação de contratos. |

---

## Guia de Inicialização Rápida

### Pré-requisitos
* [Docker e Docker Compose](https://www.docker.com/)
* [SDK do .NET 8](https://dotnet.microsoft.com/download/dotnet/8.0)
* [Java Development Kit (JDK 21)](https://adoptium.net/)
* [Maven 3.9+](https://maven.apache.org/)

### 1. Iniciar o Keycloak e Banco de Dados de Usuários
```bash
# Inicia MySQL e Keycloak Identity Server
cd users-api
docker compose up -d
```
O Keycloak estará disponível em `http://localhost:8080` (Realm: `local-dev`).

### 2. Executar Migrações do Banco de Dados CIS (Liquibase)
```bash
# Executa migrações automáticas de esquema para sd3_cisdb
cd ../cis-api/Data
docker compose up
```

### 3. Iniciar os Servidores Mock das APIs
```bash
# Inicia o servidor mock da CIS API (porta 4020)
cd ../../cis-api-swagger
docker compose up -d

# Inicia o servidor mock da Users API (porta 4010)
cd ../users-api-swagger
docker compose up -d
```

### 4. Executar os Serviços Backend
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
