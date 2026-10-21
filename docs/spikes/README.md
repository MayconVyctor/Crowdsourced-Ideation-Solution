---
title: Spikes
---
# **Fase 2: Spikes e Investigações Tecnológicas (CIS API)**

**Esta página documenta os resultados das investigações técnicas (_Spikes_) realizadas pela equipe no início da Fase 2 (Sprint 5). O objetivo destas pesquisas é mitigar riscos técnicos, avaliar o comportamento de ferramentas e fundamentar as decisões arquiteturais para o desenvolvimento da CIS API.**

## **1. Visão Geral dos Spikes Realizados**

**A tabela abaixo resume os temas investigados e o seu estado atual no projeto:**

<table>
<tr>
<td>

**ID**
</td>
<td>

**Tema do Spike**
</td>
<td>

**Foco Principal**
</td>
<td>

**Estado**
</td>
</tr>
<tr>
<td>

**SPK-01**
</td>
<td>

**Entity Framework Core com MySQL**
</td>
<td>

**Avaliação de compatibilidade e desempenho do ORM com o SGBD.**
</td>
<td>

**Documentado**

**([Acesse aqui](https://gitlab.com/groups/jala-university1/cohort-6/PT.CO.CSSD-232.GA.T2.26.M2/SA/group-2/-/wikis/home/capstone/Spikes/Research-Entity-Framework-Core-with-MySQL))**
</td>
</tr>
<tr>
<td>

**SPK-02**
</td>
<td>

**Code First vs Database First**
</td>
<td>

**Definição da melhor estratégia de mapeamento e ciclo de vida do banco.**
</td>
<td>

**Documentado**

**([Acesse aqui](https://gitlab.com/groups/jala-university1/cohort-6/PT.CO.CSSD-232.GA.T2.26.M2/SA/group-2/-/wikis/home/capstone/Spikes/Research-Entity-Framework-Core-Code-First-and-Database-First))**
</td>
</tr>
<tr>
<td>

**SPK-03**
</td>
<td>

**Liquibase Database Change Management**
</td>
<td>

**Estratégia de versionamento e migração automatizada de esquemas.**
</td>
<td>

**Documentado**

**([Acesse aqui)](https://gitlab.com/groups/jala-university1/cohort-6/PT.CO.CSSD-232.GA.T2.26.M2/SA/group-2/-/wikis/home/capstone/Spikes/Liquibase-Database-Change-Management)**
</td>
</tr>
<tr>
<td>

**SPK-04**
</td>
<td>

**Controller-based APIs vs Minimal APIs**
</td>
<td>

**Análise de arquitetura e manutenção no ASP.NET Core.**
</td>
<td>

**Documentado**

**([Acesse aqui](https://gitlab.com/groups/jala-university1/cohort-6/PT.CO.CSSD-232.GA.T2.26.M2/SA/group-2/-/wikis/home/capstone/Spikes/Research-Controller-based-APIs-and-Minimal-APIs))**
</td>
</tr>
</table>

## **2. Detalhamento dos Spikes**

### **2.1. Spike: Entity Framework Core com MySQL**

* **Objetivo:** Investigar os requisitos e a configuração do Entity Framework Core para integração com o MySQL em aplicações .NET.
* **Resultados e Descobertas:**
  * O EF Core permite mapear entidades e realizar operações no banco de dados utilizando C#.
  * Requer um provedor compatível com MySQL, como o Pomelo, além de versões compatíveis das dependências.
  * Oferece recursos de consulta e persistência de dados, além de suporte a migrações.
* **Decisão da Equipe:** Utilizar o Entity Framework Core para acesso e persistência de dados em aplicações .NET e o MySQL como banco de dados, alinhando sua utilização ao gerenciamento de alterações definido para o projeto com Liquibase.

### **2.2. Spike: Code First vs Database First**

* **Objetivo:** Analisar as abordagens Code First e Database First no EF Core, avaliando o ciclo de vida do banco e a convivência com o versionamento DDL via Liquibase.
* **Resultados e Descobertas:**
  * **Code First:** O código C# é a única fonte da verdade e as migrações são geradas pelo EF Core. Embora ofereça desenvolvimento ágil e aderência a DDD, o uso concomitante com ferramentas externas de migração cria conflito duplo de versionamento (__EFMigrationsHistory vs DATABASECHANGELOG).
  * **Database First:** O banco físico ou changelogs DDL são a fonte da verdade. Permite controle absoluto do esquema e tipos do MySQL. A engenharia reversa automatizada (scaffold) pode sobrescrever customizações, mas o mapeamento manual garante integridade.
* **Decisão da Equipe:** Adotar a abordagem Database-Driven com Liquibase como autoridade DDL única e mapeamento manual via Fluent API no CisDbContext, garantindo aderência estrita à Clean Architecture sem gerar migrações concorrentes pelo EF Core.

### **2.3. Spike: Liquibase Database Change Management**

* **Objetivo:** Investigar como utilizar o Liquibase para gerenciar e versionar alterações no banco de dados.
* **Resultados e Descobertas:**
  * Utiliza arquivos _changelog_ e _changesets_ para registrar e aplicar alterações no banco de dados.
  * Mantém o histórico das alterações executadas e oferece suporte a operações de rollback.
  * Permite padronizar e automatizar as atualizações do esquema do banco de dados.
* **Decisão da Equipe:** Utilizar o Liquibase para gerenciar e versionar as alterações do banco de dados, mantendo um histórico das mudanças realizadas.

### **2.4. Spike: Controller-based APIs and Minimal APIs**

* **Objetivo:** Comparar os conceitos, características, vantagens e desvantagens de Controller-based APIs e Minimal APIs no ASP.NET Core, fornecendo embasamento técnico para o desenvolvimento da CIS API.
* **Resultados e Descobertas:**
  * **Controller-based APIs:** Abordagem consolidada e orientada a classes que isola perfeitamente os limites de apresentação (`Cis.Presentation`) na Clean Architecture multi-projeto. Facilita a divisão de trabalho e a realização de testes unitários isolados, além de manter paridade conceitual com o `users-api` (Spring Boot).
  * **Minimal APIs:** Modelo orientado a funções com menor cerimônia sintática, excelente throughput, inicialização acelerada e alta compatibilidade com Native AOT. No entanto, para soluções de grande porte com Clean Architecture rígida, requer esforço extra de modularização para evitar acoplamento no `Program.cs`.
* **Decisão da Equipe:** Adotar Controller-based APIs como o padrão oficial para a CIS API, assegurando separação estrita de responsabilidades, previsibilidade na manutenção e facilidade de extensão nas próximas User Stories.

## **3. Conclusão e Próximos Passos**