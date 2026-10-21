# Controller-based APIs vs Minimal APIs no ASP.NET Core

## 1. Introdução

O desenvolvimento de APIs RESTful no ASP.NET Core oferece duas abordagens arquiteturais principais para a criação de endpoints HTTP: **Controller-based APIs** (baseada em controladores clássicos) e **Minimal APIs** (introduzida no .NET 6 e aprimorada nas versões subsequentes).

O objetivo desta investigação técnica (*Spike*) é comparar os conceitos fundamentais, características, padrões de desenvolvimento, desempenho e manutenibilidade de ambas as abordagens. Este documento serve como guia de referência técnica para a equipe de desenvolvimento da **CIS API**, fundamentando as decisões de design arquitetural e facilitando o entendimento de desenvolvedores e consumidores da API.

---

## 2. Visão Geral das Abordagens

### 2.1 Controller-based APIs

A abordagem baseada em controladores é o modelo tradicional do ecossistema ASP.NET Core MVC. Os endpoints são organizados dentro de classes C# que herdam da classe base `ControllerBase` (ou `Controller`) e são decoradas com atributos de roteamento e comportamento como `[ApiController]` e `[Route]`.

**Principais características:**
* **Orientação a Classes:** Cada recurso da aplicação costuma ser encapsulado em sua própria classe controladora (ex.: `TopicQueryController`, `TopicCommandController`).
* **Atributos Declarativos:** Utilização intensa de atributos de metadados para definição de verbos HTTP (`[HttpGet]`, `[HttpPost]`), rotas e políticas de validação.
* **Pipeline MVC Completo:** Acesso nativo ao pipeline de execução do MVC, incluindo filtros de ação (`Action Filters`), filtros de exceção e model binders customizados.
* **Validação Automática:** Integração automática com validação de modelos (`Data Annotations`) gerando respostas HTTP `400 Bad Request` automáticas quando `ModelState.IsValid` for falso.

### 2.2 Minimal APIs

As Minimal APIs foram projetadas para criar APIs HTTP com o mínimo de código e cerimônia estrutural. Em vez de exigir classes controladoras completas, os endpoints são registrados diretamente na instância do aplicativo (`WebApplication`) ou em grupos de rotas (`RouteGroupBuilder`) utilizando métodos de extensão e expressões lambda.

**Principais características:**
* **Baixa Sobrecarga e Cerimônia:** Elimina a necessidade de criar arquivos de controladores, construtores formais e classes base pesadas para operações simples.
* **Roteamento Direto:** Uso de métodos como `app.MapGet()`, `app.MapPost()`, `app.MapDelete()` diretamente na inicialização do servidor.
* **Compatibilidade com Native AOT:** Como minimiza o uso de reflexão em tempo de execução (*runtime reflection*), é ideal para compilação estática antecipada (*Ahead-Of-Time*), gerando inicializações quase instantâneas e consumo reduzido de memória.
* **Resultados Fortemente Tipados:** Suporte nativo a `TypedResults` (ex.: `TypedResults.Ok()`, `TypedResults.NotFound()`), melhorando a inferência de esquemas OpenAPI e facilitando testes unitários sem mocks complexos.

---

## 3. Comparação de Código Prático

Para ilustrar as diferenças sintáticas e estruturais, considere o endpoint de consulta de tópicos da CIS API (`GET /api/v1/topics`).

### 3.1 Implementação com Controller-based API

Na abordagem baseada em controlador, o código é estruturado em uma classe isolada na camada de apresentação (`Cis.Presentation`):

```csharp
using Cis.Core.Application.Services;
using Cis.Presentation.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Cis.Presentation.Controllers;

[ApiController]
[Route("api/v1/topics")]
public class TopicQueryController : ControllerBase
{
    private readonly TopicQueryService _topicQueryService;

    public TopicQueryController(TopicQueryService topicQueryService)
    {
        _topicQueryService = topicQueryService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<TopicResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<TopicResponse>>> ReadTopicsAsync()
    {
        var topics = await _topicQueryService.ReadTopicsAsync();
        return Ok(topics);
    }
}
```

### 3.2 Implementação com Minimal API

Na abordagem Minimal API, o mesmo endpoint pode ser registrado por meio de métodos de extensão modulares:

```csharp
using Cis.Core.Application.Services;
using Cis.Presentation.Responses;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Cis.Presentation.Endpoints;

public static class TopicEndpoints
{
    public static IEndpointRouteBuilder MapTopicEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/topics");

        group.MapGet("/", async (TopicQueryService topicQueryService) =>
        {
            var topics = await topicQueryService.ReadTopicsAsync();
            return TypedResults.Ok(topics);
        })
        .WithName("ReadTopics")
        .Produces<List<TopicResponse>>(StatusCodes.Status200OK);

        return app;
    }
}
```

---

## 4. Matriz Comparativa Detalhada

A tabela a seguir confronta os aspectos técnicos e práticos de ambas as abordagens:

| Critério de Avaliação | Controller-based APIs | Minimal APIs |
| :--- | :--- | :--- |
| **Estrutura de Código** | Baseada em classes e herança (`ControllerBase`). | Baseada em funções, lambdas ou métodos de extensão. |
| **Curva de Aprendizado** | Baixa para quem já conhece ASP.NET MVC, Spring Boot (Java) ou NestJS. | Baixa para scripts ou APIs simples; requer disciplina arquitetural em projetos grandes. |
| **Desempenho e Startup** | Ligeiramente mais lento no startup devido à descoberta de controladores via reflexão. | Desempenho superior de throughput e inicialização mais rápida; menor overhead por requisição. |
| **Consumo de Memória** | Maior alocação inicial de memória (carregamento de assemblies e metadados MVC). | Menor footprint de memória; alta compatibilidade com containers leves e Native AOT. |
| **Injeção de Dependências** | Tipicamente injetada via construtor da classe controladora. | Pode ser injetada diretamente como parâmetro no método da rota (`delegate`). |
| **Filtros e Interceptação** | Amplo ecossistema: `ActionFilter`, `ExceptionFilter`, `ResultFilter`. | `EndpointFilter` (disponível a partir do .NET 7), mais conciso porém com menos recursos legados. |
| **Validação de Entrada** | Automática via `DataAnnotations` e `ModelState`. | Exige validação manual ou integração explícita com bibliotecas como `FluentValidation`. |
| **Documentação OpenAPI** | Automática via Swashbuckle / Microsoft.OpenApi com base nos atributos do controller. | Totalmente suportada via `TypedResults`, `.WithName()`, `.Produces()` e `.WithOpenApi()`. |
| **Testes Unitários** | Simples: instanciação da classe do controller e mock direto de dependências. | Testes unitários diretos em lambdas podem ser verbosos; incentiva testes de integração com `WebApplicationFactory`. |

---

## 5. Considerações Arquiteturais e de Manutenibilidade

Ao avaliar o impacto de cada abordagem no ciclo de vida de uma aplicação corporativa como a CIS API, foram identificados os seguintes pontos:

### 5.1 Aderência à Clean Architecture e Multi-Projeto
* **Controllers:** Se integram de forma natural à separação multi-projeto. O projeto `Cis.Presentation` pode conter exclusivamente pastas como `Controllers/`, `Requests/` e `Responses/`, mantendo os limites físicos rígidos e garantindo que o `Cis.Server` apenas hospede a execução.
* **Minimal APIs:** Podem se tornar caóticas se forem concentradas dentro do `Program.cs`. Para manter a Clean Architecture, exige a criação de classes estáticas de extensão (ex.: `TopicEndpoints.cs`) para agrupar as rotas fora do arquivo principal.

### 5.2 Escalabilidade da Base de Código
* Em sistemas com dezenas de entidades e endpoints (como o sistema CIS, que abrangerá tópicos, ideias, votos, comentários e moderação), a divisão em classes controladoras individuais facilita a navegação, a resolução de conflitos de merge no Git e a distribuição de tarefas entre múltiplos desenvolvedores.
* As Minimal APIs brilham em microsserviços hiperfocados ou serviços com poucas operações CRUD, onde a cerimônia de múltiplos controladores gera burocracia desnecessária.

### 5.3 Validação e Tratamento de Erros
* Os controladores oferecem uma padronização madura para APIs corporativas, onde filtros globais e validação em camada tratam erros de forma previsível e geram payloads de erro padronizados (`ProblemDetails` - RFC 7807).
* Nas Minimal APIs, a ausência de validação automática por `ModelState` exige a configuração de filtros de endpoint (`AddEndpointFilter`) ou middleware customizado para capturar inconsistências antes da execução do handler.

---

## 6. Decisão Arquitetural da Equipe

Com base nas análises efetuadas durante este Spike, a equipe tomou a seguinte decisão técnica para o projeto **CIS API**:

> **Decisão:** A equipe adotará **Controller-based APIs** como o padrão arquitetural predominante para a camada de apresentação da `cis-api`.

### Justificativas da Decisão:
1. **Conformidade com a Clean Architecture:** A separação física dos controladores na biblioteca de classes `Cis.Presentation` isola de forma rigorosa as regras de tráfego HTTP das regras de negócio (`Cis.Core.Application`), atendendo plenamente aos critérios de arquitetura multi-projeto definidos para a disciplina.
2. **Consistência de Paradigma no Sistema CIS:** O projeto irmão `users-api` foi construído em Java com Spring Boot utilizando o padrão MVC Controller. A adoção de controladores C# no `cis-api` garante consistência conceitual para a equipe e simplifica o intercâmbio de código e revisões.
3. **Organização e Divisão de Tarefas:** Cada entidade (Tópicos, Ideias, Votos) possui seu próprio controlador e arquivo de resposta em pastas organizadas, evitando que o arquivo de inicialização (`Program.cs`) centralize regras ou cresça desordenadamente.
4. **Testabilidade Isolada:** Facilidade em escrever testes unitários diretos para cada método de controller sem necessidade de subir o servidor web em memória em todas as suítes de teste.

---

## 7. Perguntas Abertas, Premissas e Próximos Passos

### 7.1 Perguntas Abertas
* **Uso Híbrido:** Seria benéfico utilizar Minimal APIs no futuro para rotas operacionais de altíssima frequência e sem regras de negócio, como checagem de saúde (`/health`) e métricas (`/metrics`)?
* **Adoção de FluentValidation:** Como a biblioteca `FluentValidation` pode ser padronizada entre os controladores para validar DTOs complexos de Tópicos e Ideias?

### 7.2 Premissas Assumidas
* A solução manterá o runtime em .NET 10 hospedado em containers Linux (Docker).
* O ganho de throughput de Minimal APIs não é o fator limitante para a carga prevista na CIS API, sendo a manutenibilidade, legibilidade e conformidade arquitetural as prioridades do projeto.

### 7.3 Próximos Passos
1. Manter a estrutura de controllers na camada `Cis.Presentation` iniciada no scaffolding (`TopicQueryController.cs`).
2. Implementar os próximos controladores para Tópicos (`TopicCommandController.cs`) e Ideias nas User Stories das Sprints subsequentes.
3. Atualizar o índice geral de Spikes da Wiki com as conclusões deste documento.

