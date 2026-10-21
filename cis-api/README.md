# CIS API

## 1. Sobre o projeto

A CIS API é uma API REST desenvolvida em C# com ASP.NET Core e .NET 10. O projeto tem como objetivo disponibilizar endpoints para consulta e gerenciamento de tópicos, seguindo uma arquitetura organizada em camadas.

Nesta etapa, foi criada a estrutura inicial da aplicação, configurada a injeção de dependências e implementado o endpoint inicial para consulta de tópicos.

## 2. Tecnologias utilizadas

- C#
- .NET 10
- ASP.NET Core
- Docker
- Docker Compose
- MySQL e Liquibase para a infraestrutura de banco de dados e migrações futuras

## 3. Estrutura do projeto

A solução está organizada nos seguintes projetos:

- **Cis.Core.Domain:** contém as entidades do domínio.
- **Cis.Core.Application:** contém os serviços da aplicação.
- **Cis.Presentation:** contém os controllers e os objetos de resposta da API.
- **Cis.Infrastructure.MySql:** contém os componentes de infraestrutura relacionados ao MySQL.
- **Cis.Infrastructure.Ioc:** centraliza a composição das dependências.
- **Cis.Server:** inicializa e executa a aplicação.

A pasta `Data/` contém os arquivos relacionados às migrações do banco de dados com Liquibase. A pasta `tests/` está reservada para os testes automatizados.

## 4. Requisitos

Para executar o projeto localmente, é necessário ter:

- .NET SDK 10.0.112
- Git

Para executar a aplicação em container, também é necessário ter Docker e Docker Compose instalados.

## 5. Como executar localmente

Na raiz do repositório, execute os seguintes comandos:

```bash
dotnet restore Cis.slnx
dotnet build Cis.slnx
dotnet run --project src/Cis.Server/Cis.Server.csproj --launch-profile http
```

A aplicação estará disponível em:

`http://localhost:9001`

## 6. Endpoint disponível

### Listar tópicos

**Método:** `GET`

**Rota:** `/api/v1/topics`

Exemplo de requisição:

```bash
curl -i http://localhost:9001/api/v1/topics
```

Resposta esperada nesta etapa:

- **Código HTTP:** `200 OK`
- **Corpo JSON:**

```json
[]
```

O endpoint retorna uma lista vazia. A consulta real ao banco de dados ainda não foi implementada.

## 7. Execução com Docker Compose

Na raiz do repositório, crie o arquivo local de variáveis de ambiente:

```bash
cp .env.example .env
```

Revise o arquivo `.env` de acordo com o ambiente em que a aplicação será executada.

Em seguida, construa a imagem e inicie a API:

```bash
docker compose up --build
```

A API estará disponível em:

`http://localhost:9001`

Para encerrar a aplicação, execute:

```bash
docker compose down
```

O arquivo `.env` contém configurações locais e não deve ser enviado ao repositório. O arquivo `.env.example` contém apenas valores ilustrativos.

## 8. Configuração do banco de dados

O banco de dados previsto para o projeto é `sd3_cisdb`. As variáveis de ambiente documentadas para uma futura configuração de conexão são:

- `DB_HOST`: endereço do servidor MySQL.
- `DB_PORT`: porta do servidor MySQL.
- `DB_NAME`: nome do banco de dados.
- `DB_USERNAME`: usuário do banco.
- `DB_PASSWORD`: senha do banco.
- `SERVER_PORT`: porta publicada para acessar a API.

Essas variáveis estão preparadas para configuração externa. Nesta etapa, a aplicação ainda não estabelece uma conexão real com o MySQL nem realiza consultas aos dados.

Quando a API estiver em um container, `DB_HOST=localhost` aponta para o próprio container, e não para o computador host. O endereço deverá ser ajustado conforme a configuração do ambiente.

## 9. Migrações do banco de dados

Os arquivos de migração estão localizados na pasta `Data/` e utilizam Liquibase.

A execução das migrações é independente da inicialização da API. O `docker-compose.yml` da raiz inicia a API, enquanto `Data/compose.yml` mantém a configuração específica das migrações.

## 10. Estado atual do projeto

Nesta etapa, foram implementados:

- Estrutura inicial dos projetos e suas referências.
- Entidade de domínio `Topic`.
- Serviço inicial `TopicQueryService`.
- Repositório inicial `TopicRepository`.
- Registro de dependências por meio de extensões de `IServiceCollection`.
- Controller `TopicQueryController`.
- Endpoint `GET /api/v1/topics`, com resposta HTTP `200 OK` e lista vazia.
- Inicialização da aplicação na porta HTTP `9001`.
- Arquivos iniciais para execução com Docker.

A consulta real ao banco, a persistência completa e outros recursos da API serão implementados em etapas posteriores.