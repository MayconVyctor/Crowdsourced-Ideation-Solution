# Users API — OpenAPI Mock

## Objective

Create a **zero-code Users API mock** using an OpenAPI YAML definition and Docker.

The project demonstrates how an API can be designed and tested **before implementing the real backend application**.

The API contract is defined entirely in `openapi.yaml`. Stoplight Prism reads this contract and exposes a mock HTTP API.

No Java or C# code, controllers, database, or application implementation is required.

---

## Technologies

* OpenAPI 3
* YAML
* Docker
* Docker Compose
* Stoplight Prism

---

## Project Structure

```text
users-api/
│
├── openapi.yaml
└── docker-compose.yml
```

### Files

| File                 | Responsibility              |
| -------------------- | --------------------------- |
| `openapi.yaml`       | Defines the API contract    |
| `docker-compose.yml` | Runs the mock API in Docker |

---

# 1. OpenAPI Definition

Create a file named:

```text
openapi.yaml
```

Use the following definition:

```yaml
openapi: 3.0.3

info:
  title: Users API
  description: Zero-code mock Users API
  version: 1.0.0

servers:
  - url: http://localhost:4010

paths:

  /health:
    get:
      summary: Check Users API health
      description: Returns the current health status of the Users API.
      operationId: getHealth

      responses:

        '200':
          description: Users API is healthy
          content:
            application/json:
              schema:
                $ref: '#/components/schemas/HealthResponse'
              example:
                status: Healthy
                service: Users API
                version: 1.0.0

        '503':
          description: Users API is unhealthy
          content:
            application/json:
              schema:
                $ref: '#/components/schemas/HealthResponse'
              example:
                status: Unhealthy
                service: Users API
                version: 1.0.0

components:

  schemas:

    HealthResponse:
      type: object

      required:
        - status
        - service
        - version

      properties:

        status:
          type: string
          example: Healthy

        service:
          type: string
          example: Users API

        version:
          type: string
          example: 1.0.0
```

The YAML defines:

```text
HTTP Method: GET
Endpoint:    /health
Success:     200 OK
Failure:     503 Service Unavailable
Content:     application/json
```

---

# 2. Docker Compose

Create:

```text
docker-compose.yml
```

Add:

```yaml
services:

  users-api:
    image: stoplight/prism:5.16.0

    command:
      - mock
      - -h
      - 0.0.0.0
      - /openapi/openapi.yaml

    ports:
      - "4010:4010"

    volumes:
      - ./openapi.yaml:/openapi/openapi.yaml:ro
```

The Docker container does not contain any Users API application code.

Instead:

```text
openapi.yaml
     │
     ▼
Docker Volume
     │
     ▼
Stoplight Prism
     │
     ▼
HTTP Mock API
```

---

# 3. Start the API

Open a terminal in the project directory:

```bash
docker compose up
```

Docker downloads the Prism image if it is not already available locally.

The mock API will be available at:

```text
http://localhost:4010
```

---

# 4. Test the Health Endpoint

Use:

```bash
curl http://localhost:4010/health
```

Expected response:

```json
{
  "status": "Healthy",
  "service": "Users API",
  "version": "1.0.0"
}
```

The HTTP response is:

```text
HTTP/1.1 200 OK
Content-Type: application/json
```

---

# 5. Health Endpoint Contract

The endpoint is defined in OpenAPI as:

```text
GET /health
```

### Request

```http
GET /health
```

No request body is required.

### Successful Response

```text
Status Code: 200 OK
```

```json
{
  "status": "Healthy",
  "service": "Users API",
  "version": "1.0.0"
}
```

### Unhealthy Response

```text
Status Code: 503 Service Unavailable
```

```json
{
  "status": "Unhealthy",
  "service": "Users API",
  "version": "1.0.0"
}
```

---

# 6. OpenAPI as the API Contract

The important concept in this project is that the API is described **before it is implemented**.

The `openapi.yaml` file defines:

* Endpoint
* HTTP method
* Request information
* Response
* HTTP status codes
* Content type
* Response schema
* Example response

For example:

```yaml
/health:
  get:
    responses:
      '200':
        description: Users API is healthy
```

This is the API contract.

Prism uses the contract to provide a working mock endpoint.

---

# 7. Swagger / OpenAPI Documentation

The `openapi.yaml` file can also be opened with an OpenAPI/Swagger-compatible documentation tool.

The documentation will describe:

```text
Users API

GET /health

Responses

200 — Users API is healthy
503 — Users API is unhealthy
```

This allows frontend developers, testers, or other teams to understand the API without requiring the real backend implementation.

---

# 8. Docker Image

The project uses the Prism Docker image:

```yaml
image: stoplight/prism:5.16.0
```

There is no `Dockerfile` because the project does not need to build an application.

Docker Compose starts the existing Prism image and mounts the OpenAPI definition:

```yaml
volumes:
  - ./openapi.yaml:/openapi/openapi.yaml:ro
```

The `:ro` means that the container can **read** the OpenAPI file but cannot modify it.

---

# 9. Stop the API

To stop the container:

```bash
docker compose down
```

---

# 10. Verify the Container

Start the API:

```bash
docker compose up -d
```

Check running containers:

```bash
docker compose ps
```

You should see the `users-api` container running.

Test the endpoint:

```bash
curl http://localhost:4010/health
```

---

# 11. No Application Code

This project intentionally contains **zero application code**.

There is no:

```text
Controller
Service
Repository
Database
C# project
.NET project
Business logic
Dockerfile
```

The only project artifacts are:

```text
openapi.yaml
docker-compose.yml
```

The API behavior is defined by the OpenAPI contract and provided by the mock server.

---

# 12. Next Step

Once the API contract has been validated, the mock API can later be replaced by a real implementation.

For example:

```text
             Current project

          openapi.yaml
                │
                ▼
        ┌───────────────┐
        │ Prism Mock API│
        └───────────────┘
                │
                ▼
           GET /health


             Future project

          openapi.yaml
                │
                ▼
        ┌───────────────┐
        │  Users API    │
        │     Java      │
        └───────────────┘
                │
                ▼
           GET /health
```

The important principle is:

> **Define the API contract first, then implement the API.**

This approach allows API consumers and backend developers to agree on the endpoint, request, response, and status codes before application development begins.
