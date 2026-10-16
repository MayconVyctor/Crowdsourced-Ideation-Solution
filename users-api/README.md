# Users API

A Java REST API demonstrating **Clean Architecture**, **Dependency Inversion**, **Repository Pattern**, and separation between the domain, application, infrastructure, and presentation layers.

The project implements CRUD operations for users and uses **MySQL with JPA/Hibernate** as the initial persistence mechanism.

The architecture is intentionally designed so that the persistence technology can later be replaced by **MongoDB** without modifying the Core Domain or the application services.

---

## 1. Objectives

The project demonstrates how to build a maintainable Java application with:

* Clean Architecture
* Separation of concerns
* Dependency Inversion Principle
* Repository Pattern
* Service interfaces
* Anemic Domain Entities
* Use of interfaces as architectural contracts
* JPA/Hibernate persistence
* MySQL
* REST API
* DTOs
* Dependency Injection
* Validation
* Centralized exception handling
* Technology isolation
* Replaceable persistence implementations

The main architectural objective is:

> **Business logic must not depend on infrastructure or framework-specific technologies.**

---

# 2. Architecture

The project is divided into four modules:

```text
users-core-domain
users-core-application
users-infrastructure
users-presentation
```

The dependency structure is:

```text
                    ┌─────────────────────┐
                    │    Presentation     │
                    │                     │
                    │   Spring Boot       │
                    │   REST Controllers  │
                    │   DTOs              │
                    └──────────┬──────────┘
                               │
                               ▼
                    ┌─────────────────────┐
                    │ Core Application    │
                    │                     │
                    │ UserServiceImpl     │
                    │                     │
                    │ Pure Java           │
                    └──────────┬──────────┘
                               │
                               ▼
                    ┌─────────────────────┐
                    │    Core Domain      │
                    │                     │
                    │ User                │
                    │ UserService         │
                    │ UserRepository      │
                    │                     │
                    │ Pure Java           │
                    └──────────▲──────────┘
                               │
                               │ implements
                               │
                    ┌──────────┴──────────┐
                    │   Infrastructure    │
                    │                     │
                    │ MySQL               │
                    │ JPA / Hibernate     │
                    │ Repository Adapter  │
                    └─────────────────────┘
```

### Dependency rule

The dependency direction is:

```text
Presentation
     ↓
Core Application
     ↓
Core Domain
     ↑
Infrastructure
```

Infrastructure implements interfaces defined by the Core Domain.

The Core Domain does **not** depend on Infrastructure.

---

# 3. Layer Responsibilities

## 3.1 Core Domain

The Core Domain contains the application's fundamental business concepts and contracts.

It contains:

* Domain entities
* Service interfaces
* Repository interfaces
* Domain exceptions

The Core Domain must remain independent of frameworks and infrastructure technologies.

### Core Domain must NOT depend on

```text
Spring Boot
Spring Data
JPA
Hibernate
MySQL
MongoDB
HTTP
REST
JSON
```

---

## 3.2 Core Application

The Core Application contains the implementation of the application services.

It:

* Implements service interfaces
* Coordinates business operations
* Uses repository interfaces
* Contains application-level business rules

The application layer depends on abstractions defined by Core Domain.

For example:

```text
UserServiceImpl
       │
       ▼
UserRepository
```

It does not depend directly on:

```text
MySQL
JPA
Hibernate
MongoDB
```

---

## 3.3 Infrastructure

Infrastructure contains implementations of external technical concerns.

The initial implementation uses:

```text
MySQL
JPA
Hibernate
Spring Data JPA
```

Infrastructure contains:

* Persistence entities
* Spring Data repositories
* Repository implementations
* Persistence mappers
* Database-specific configuration

Infrastructure implements the repository contracts defined in Core Domain.

---

## 3.4 Presentation

Presentation is the only layer that exposes the application through HTTP.

It contains:

* Spring Boot
* REST controllers
* Request DTOs
* Response DTOs
* API mappers
* HTTP exception handling
* Application configuration

The controller communicates with the Core through the `UserService` interface.

The controller should not directly access:

```text
JPA
Hibernate
MySQL
Spring Data repositories
```

---

# 4. Project Structure

```text
users-api/
│
├── pom.xml
│
├── users-core-domain/
│   ├── pom.xml
│   │
│   └── src/main/java/
│       └── com/example/users/domain/
│           │
│           ├── entity/
│           │   └── User.java
│           │
│           ├── repository/
│           │   └── UserRepository.java
│           │
│           └── service/
│               └── UserService.java
│
├── users-core-application/
│   ├── pom.xml
│   │
│   └── src/main/java/
│       └── com/example/users/application/
│           │
│           └── service/
│               └── UserServiceImpl.java
│
├── users-infrastructure/
│   ├── pom.xml
│   │
│   └── src/main/java/
│       └── com/example/users/infrastructure/
│           │
│           ├── mapper/
│           │   └── UserPersistenceMapper.java
│           │
│           └── persistence/
│               └── mysql/
│                   │
│                   ├── entity/
│                   │   └── UserJpaEntity.java
│                   │
│                   ├── repository/
│                   │   └── SpringDataUserRepository.java
│                   │
│                   └── adapter/
│                       └── UserRepositoryImpl.java
│
└── users-presentation/
    ├── pom.xml
    │
    └── src/main/
        │
        ├── java/
        │   └── com/example/users/
        │       │
        │       ├── UsersApplication.java
        │       │
        │       └── presentation/
        │           │
        │           ├── controller/
        │           │   └── UserController.java
        │           │
        │           ├── dto/
        │           │   ├── CreateUserRequest.java
        │           │   ├── UpdateUserRequest.java
        │           │   └── UserResponse.java
        │           │
        │           └── mapper/
        │               └── UserApiMapper.java
        │
        └── resources/
            └── application.yml
```

---

# 5. Core Domain Contracts

The Core Domain defines the application's contracts.

## User Entity

The domain uses an **anemic domain model**.

```java
public class User {

    private UUID id;
    private String name;
    private String login;
    private String password;

    // Constructors and getters/setters
}
```

The domain entity contains data and does not contain persistence annotations.

It must not contain:

```java
@Entity
@Table
@Id
```

---

## User Service

The service contract defines the operations supported by the application.

```java
public interface UserService {

    List<User> getAllUsers();

    User getUser(UUID id);

    User createUser(
            String name,
            String login,
            String password);

    User updateUser(
            UUID id,
            String name,
            String login,
            String password);

    void deleteUser(UUID id);
}
```

The service interface belongs to Core Domain.

---

## User Repository

The repository contract defines persistence operations.

```java
public interface UserRepository {

    List<User> findAll();

    Optional<User> findById(UUID id);

    User create(User user);

    User update(User user);

    void delete(UUID id);

    boolean existsById(UUID id);
}
```

The interface does not specify how the data is stored.

---

# 6. Core Application

The Core Application implements `UserService`.

```text
UserService
     ▲
     │ implements
     │
UserServiceImpl
     │
     │ depends on
     ▼
UserRepository
```

Example:

```java
public class UserServiceImpl
        implements UserService {

    private final UserRepository userRepository;

    public UserServiceImpl(
            UserRepository userRepository) {

        this.userRepository = userRepository;
    }

    // CRUD operations
}
```

This allows the application service to work with any implementation of `UserRepository`.

---

# 7. Infrastructure

The MySQL implementation contains a separate persistence entity:

```text
Core Domain

User
 │
 │ mapper
 ▼
Infrastructure

UserJpaEntity
```

This prevents JPA annotations from leaking into the Core Domain.

The infrastructure repository implements:

```java
public class UserRepositoryImpl
        implements UserRepository {
}
```

Internally it uses:

```text
UserRepositoryImpl
       │
       ▼
SpringDataUserRepository
       │
       ▼
JPA / Hibernate
       │
       ▼
MySQL
```

---

# 8. Presentation

The REST API exposes the following operations.

| Operation      | Method   | Endpoint          |
| -------------- | -------- | ----------------- |
| Read all users | `GET`    | `/api/v1/users`      |
| Read one user  | `GET`    | `/api/v1/users/{id}` |
| Create user    | `POST`   | `/api/v1/users`      |
| Update user    | `PUT`    | `/api/v1/users/{id}` |
| Delete user    | `DELETE` | `/api/v1/users/{id}` |

---

# 9. API Examples

## 9.1 Read All Users

```http
GET /api/v1/users
```

Example response:

```http
200 OK
Content-Type: application/json
```

```json
[
  {
    "id": "3bf71036-e7ef-4890-b79b-91496c14160f",
    "name": "javier",
    "login": "jroca"
  }
]
```

---

## 9.2 Read One User

```http
GET /api/v1/users/3bf71036-e7ef-4890-b79b-91496c14160f
```

Example response:

```http
200 OK
```

```json
{
  "id": "3bf71036-e7ef-4890-b79b-91496c14160f",
  "name": "javier",
  "login": "jroca"
}
```

If the user does not exist:

```http
404 Not Found
```

---

# 10. Create User

The original command:

```text
-config=sd3.xml -create -n javier -l jroca -p pass123
```

is represented by:

```http
POST /api/users
Content-Type: application/json
```

Request:

```json
{
  "name": "javier",
  "login": "jroca",
  "password": "pass123"
}
```

Response:

```http
201 Created
```

```json
{
  "id": "3bf71036-e7ef-4890-b79b-91496c14160f",
  "name": "javier",
  "login": "jroca"
}
```

The password must not be returned by the API.

---

# 11. Update User

The original command:

```text
-config=sd3.xml -update
-i 3bf71036-e7ef-4890-b79b-91496c14160f
-n javier2
-l jroca2
-p pwd321
```

is represented by:

```http
PUT /api/v1/users/3bf71036-e7ef-4890-b79b-91496c14160f
Content-Type: application/json
```

Request:

```json
{
  "name": "javier2",
  "login": "jroca2",
  "password": "pwd321"
}
```

Response:

```http
200 OK
```

```json
{
  "id": "3bf71036-e7ef-4890-b79b-91496c14160f",
  "name": "javier2",
  "login": "jroca2"
}
```

---

# 12. Delete User

The original command:

```text
-config=sd3.xml -delete aab5d5fd-70c1-11e5-a4fb-b026b977eb28
```

is represented by:

```http
DELETE /api/v1/users/aab5d5fd-70c1-11e5-a4fb-b026b977eb28
```

Successful response:

```http
204 No Content
```

If the user does not exist:

```http
404 Not Found
```

---

# 13. Configuration

The application configuration is located in:

```text
users-presentation/src/main/resources/application.yml
```

Example:

```yaml
spring:

  application:
    name: users-api

  datasource:
    url: ${DB_URL:jdbc:mysql://localhost:3306/users}
    username: ${DB_USERNAME:users}
    password: ${DB_PASSWORD:users}
    driver-class-name: com.mysql.cj.jdbc.Driver

  jpa:
    hibernate:
      ddl-auto: update

    show-sql: false

    properties:
      hibernate:
        format_sql: true

server:
  port: ${SERVER_PORT:8080}
```

Environment variables can override the default configuration.

---

# 14. Database

The initial persistence implementation uses MySQL.

Expected database:

```text
Database: users
Table: users
```

The application expects the following logical fields:

```text
id
name
login
password
```

For production environments, database credentials should be supplied through environment variables or a secrets-management solution.

---

# 15. Dependency Injection

Dependency Injection is configured in the Presentation composition root.

The application assembles the dependencies:

```text
UserController
      │
      ▼
UserService
      │
      ▼
UserServiceImpl
      │
      ▼
UserRepository
      │
      ▼
UserRepositoryImpl
      │
      ▼
SpringDataUserRepository
      │
      ▼
MySQL
```

The controller receives the abstraction:

```java
UserService
```

and the application service receives:

```java
UserRepository
```

Neither component needs to know which concrete infrastructure implementation is being used.

---

# 16. Switching from MySQL to MongoDB

One of the primary architectural goals is persistence technology independence.

The current implementation is:

```text
             UserRepository
                   ▲
                   │
                   │ implements
                   │
          UserRepositoryImpl
                   │
                   ▼
              Spring Data
                   │
                   ▼
                MySQL
```

A MongoDB implementation can later be added:

```text
             UserRepository
                   ▲
                   │
                   │ implements
                   │
       UserMongoRepositoryImpl
                   │
                   ▼
          Spring Data MongoDB
                   │
                   ▼
                MongoDB
```

The following components should remain unchanged:

```text
User
UserService
UserServiceImpl
UserRepository
UserController
API DTOs
```

Only the Infrastructure implementation and configuration need to change.

---

# 17. Why Separate Domain and JPA Entities?

The project intentionally has two representations of a user.

### Domain

```text
User
```

### Persistence

```text
UserJpaEntity
```

This prevents infrastructure concerns from leaking into the domain.

Avoid:

```java
@Entity
public class User {
}
```

inside Core Domain.

Instead:

```java
public class User {
}
```

and:

```java
@Entity
public class UserJpaEntity {
}
```

inside Infrastructure.

This allows the domain to remain independent of the persistence framework.

---

# 18. Technology Dependency Rules

The following rules must be maintained.

### Core Domain

Allowed:

```text
Java Standard Library
```

Not allowed:

```text
Spring
Spring Boot
JPA
Hibernate
MySQL
MongoDB
```

### Core Application

Allowed:

```text
Core Domain
Java Standard Library
```

Not allowed:

```text
Spring
JPA
Hibernate
MySQL
MongoDB
```

### Infrastructure

Allowed:

```text
Core Domain
JPA
Hibernate
Spring Data
MySQL
MongoDB
```

### Presentation

Allowed:

```text
Core Domain
Core Application
Spring Boot
Spring MVC
Validation
HTTP
JSON
```

---

# 19. Build

From the root directory:

```bash
mvn clean install
```

To package the application:

```bash
mvn clean package
```

The executable application is produced by the Presentation module.

---

# 20. Run

Configure the database connection and run:

```bash
mvn spring-boot:run -pl users-presentation
```

The API will be available at:

```text
http://localhost:8080
```

---

# 21. CRUD Workflow

A typical workflow is:

```text
POST /api/users
        │
        ▼
UserController
        │
        ▼
UserService
        │
        ▼
UserServiceImpl
        │
        ▼
UserRepository
        │
        ▼
UserRepositoryImpl
        │
        ▼
JPA / Hibernate
        │
        ▼
MySQL
```

The same flow applies to:

```text
GET
PUT
DELETE
```

The controller never directly communicates with MySQL.

---

# 22. Design Principles

This project follows several fundamental software engineering principles.

## Single Responsibility Principle

Each layer has a focused responsibility:

```text
Domain          → business concepts and contracts
Application     → application operations
Infrastructure  → technical implementations
Presentation    → HTTP communication
```

## Dependency Inversion Principle

High-level application logic depends on abstractions:

```text
UserServiceImpl
       │
       ▼
UserRepository
```

rather than concrete database implementations.

## Separation of Concerns

HTTP, business logic, and persistence are separated.

## Open/Closed Principle

New persistence implementations can be added without modifying the Core Domain.

For example:

```text
MySQL
MongoDB
PostgreSQL
InMemory
```

can all implement:

```java
UserRepository
```

---

# 23. Expected Result

After completing the project, the following architecture should exist:

```text
                    REST API
                       │
                       ▼
              ┌─────────────────┐
              │ UserController  │
              └────────┬────────┘
                       │
                       ▼
              ┌─────────────────┐
              │  UserService    │
              │   interface     │
              └────────┬────────┘
                       ▲
                       │ implements
                       │
              ┌────────┴────────┐
              │ UserServiceImpl │
              └────────┬────────┘
                       │
                       ▼
              ┌─────────────────┐
              │ UserRepository  │
              │   interface     │
              └────────┬────────┘
                       ▲
                       │ implements
                       │
              ┌────────┴────────┐
              │UserRepositoryImpl│
              └────────┬────────┘
                       │
                       ▼
                 JPA/Hibernate
                       │
                       ▼
                    MySQL
```

The key architectural principle is:

> **The Core defines the contracts. Application implements the services. Infrastructure implements persistence. Presentation exposes the application through HTTP.**

This keeps the business logic independent of the technologies used to expose or persist it.

## 22. Keycloak - Local Development

> **DEV ONLY:** This Keycloak configuration is intended only for local development and testing. It uses development mode and default credentials. Do not use these settings in production.

### Starting Keycloak

The project includes a local Keycloak instance configured through Docker Compose. The realm, client, roles, and test user are imported automatically when the container starts.

Start Keycloak with:

```bash
docker compose up
```

The Keycloak server will be available at:

```text
http://localhost:8080
```

The configuration uses the `local-dev` realm and the `users-api` client.

### Test User

A test user is preconfigured for local development:

| Property | Value         |
| -------- | ------------- |
| Realm    | `local-dev`   |
| Client   | `users-api`   |
| Username | `testuser`    |
| Password | `testpass123` |
| Role     | `user`        |

These credentials are intentionally simple because this configuration is only for local development.

### Getting a JWT

With Keycloak running, request a token using:

```bash
curl -s -X POST \
  http://localhost:8080/realms/local-dev/protocol/openid-connect/token \
  -H 'Content-Type: application/x-www-form-urlencoded' \
  -d 'client_id=users-api' \
  -d 'username=testuser' \
  -d 'password=testpass123' \
  -d 'grant_type=password'
```

The response contains an `access_token`:

```json
{
  "access_token": "<JWT>",
  "token_type": "Bearer",
  "expires_in": 300
}
```

The access token is a JWT issued by the `local-dev` realm.

### JWT Claims and Roles

The JWT generated for `testuser` contains the client identifier and the user's realm role.

Relevant claims include:

```json
{
  "iss": "http://localhost:8080/realms/local-dev",
  "azp": "users-api",
  "realm_access": {
    "roles": [
      "user"
    ]
  },
  "preferred_username": "testuser"
}
```

The `realm_access.roles` claim is where the `user` role is included. The API authentication configuration can use this claim when JWT authorization is implemented.

### Stopping Keycloak

To stop the Keycloak container, press:

```text
Ctrl + C
```

when `docker compose up` is running.

To remove the container and its Compose network, use:

```bash
docker compose down
```

The realm configuration is version-controlled in:

```text
keycloak/local-dev-realm.json
```

This makes the local Keycloak environment reproducible for other developers.
.