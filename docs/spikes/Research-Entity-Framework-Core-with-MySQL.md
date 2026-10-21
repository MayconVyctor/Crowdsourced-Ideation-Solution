# Entity Framework Core com MySQL

## 1. Introdução

O Entity Framework Core (EF Core) é uma ferramenta de mapeamento objeto-relacional (ORM) para aplicações desenvolvidas com .NET. Ele permite que desenvolvedores trabalhem com bancos de dados relacionais utilizando objetos e classes C#, reduzindo a necessidade de escrever SQL manualmente em operações comuns.

O objetivo desta pesquisa é apresentar os requisitos, as dependências, a configuração da conexão e os principais cuidados necessários para utilizar o EF Core com MySQL.

## 2. Requisitos para utilização

Para integrar o EF Core com MySQL, são necessários:

* **.NET SDK:** ambiente necessário para desenvolver e executar aplicações .NET.
* **Entity Framework Core:** biblioteca responsável pelo mapeamento entre os objetos da aplicação e as tabelas do banco.
* **Provedor MySQL:** biblioteca que permite ao EF Core comunicar-se com o MySQL.
* **MySQL Server:** servidor de banco de dados que armazenará as informações.
* **String de conexão:** configuração com o endereço do servidor, o banco de dados e as credenciais de acesso.

As versões dessas dependências devem ser compatíveis entre si para evitar problemas durante a compilação e a execução.

## 3. Provedor MySQL e dependências

Existem diferentes provedores que permitem utilizar o EF Core com MySQL. Entre as opções disponíveis estão:

| Provedor | Características |
|----------|-----------------|
| `Pomelo.EntityFrameworkCore.MySql` | Provedor de código aberto que utiliza o MySqlConnector e oferece suporte a MySQL e MariaDB. |
| `MySql.EntityFrameworkCore` | Provedor disponibilizado pela Oracle para integração do EF Core com MySQL. |

A escolha deve considerar a compatibilidade com a versão do EF Core, o suporte ao banco utilizado, a manutenção do projeto e as necessidades da aplicação.

Para os exemplos desta documentação, será utilizado o provedor **Pomelo.EntityFrameworkCore.MySql**.

### 3.1 Dependências principais

Em uma aplicação .NET, as dependências podem ser adicionadas pelo terminal com o comando `dotnet add package`.

Exemplo para um projeto que utilize .NET 8 e EF Core 8:

`dotnet add package Microsoft.EntityFrameworkCore --version 8.0.*
dotnet add package Pomelo.EntityFrameworkCore.MySql --version 8.0.3
dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.*`

As versões apresentadas são um exemplo de combinação compatível, não uma exigência para todos os projetos. Antes de instalar os pacotes, confira as versões disponíveis e a compatibilidade indicada na documentação oficial do provedor.

O pacote `Microsoft.EntityFrameworkCore.Design` é útil para operações de desenvolvimento, como o gerenciamento de migrations. Para utilizar os comandos do EF Core pelo terminal, também pode ser necessário instalar a ferramenta `dotnet-ef`.

## 4. Configuração da conexão com MySQL

A aplicação precisa de uma string de conexão para acessar o banco de dados.

Um exemplo de configuração é:

`{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3306;Database=appdb;User=appuser;Password=senha;"
  }
}`

Nesse exemplo:

* `Server`: endereço do servidor MySQL.
* `Port`: porta utilizada pelo servidor.
* `Database`: nome do banco de dados.
* `User`: usuário utilizado na conexão.
* `Password`: senha do usuário.

Os valores devem ser substituídos pelos dados do ambiente utilizado.

**Importante:** a senha do exemplo é fictícia. Credenciais reais não devem ser armazenadas diretamente em arquivos versionados. Em ambientes de desenvolvimento, podem ser utilizadas variáveis de ambiente ou ferramentas de configuração de segredos.

## 5. Configurando o EF Core na aplicação

Após instalar os pacotes, o provedor MySQL deve ser registrado na configuração da aplicação.

Exemplo em C#:

`using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString)));

var app = builder.Build();

app.Run();`

Nesse exemplo, `AppDbContext` representa a classe responsável por gerenciar o acesso às entidades do banco de dados.

O método `UseMySql` configura o provedor utilizado pelo EF Core. O método `ServerVersion.AutoDetect` tenta identificar a versão do servidor para ajustar o comportamento do provedor.

A identificação automática requer que a aplicação consiga acessar o servidor. Em ambientes nos quais essa identificação não seja adequada, a versão do MySQL pode ser configurada explicitamente.

## 6. DbContext e entidades

O `DbContext` representa a sessão de trabalho com o banco de dados. Ele permite consultar, adicionar, atualizar e remover entidades, além de persistir alterações.

Uma entidade é uma classe C# que representa dados armazenados no banco.

Exemplo de entidade:

`public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Login { get; set; } = string.Empty;
}`

Exemplo de contexto:

`using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
}`

O EF Core utiliza o contexto e as entidades para realizar operações no banco de dados. O mapeamento pode ser ajustado por convenções, atributos ou configuração explícita.

## 7. Migrations e gerenciamento de alterações

As migrations permitem registrar alterações realizadas no modelo da aplicação e gerar instruções para atualizar o esquema do banco de dados.

Após configurar o EF Core e o projeto, um fluxo comum é:

`dotnet ef migrations add InitialCreate`

Esse comando cria uma migration com base nas diferenças identificadas no modelo.

Para aplicar as migrations ao banco configurado:

`dotnet ef database update`

Esses comandos pressupõem que a ferramenta `dotnet-ef`, o pacote de design e a configuração do projeto estejam preparados.

Antes de aplicar uma migration em um ambiente compartilhado ou de produção, revise o SQL gerado e avalie os riscos para os dados existentes. As migrations do EF Core não devem ser confundidas com changelogs do Liquibase: são mecanismos diferentes, e a equipe deve definir qual ferramenta será responsável pelas alterações do banco em cada projeto.

## 8. Compatibilidade e limitações

A integração entre EF Core e MySQL exige atenção aos seguintes pontos:

* **Compatibilidade de versões:** os provedores normalmente acompanham versões principais específicas do EF Core. Um provedor destinado ao EF Core 8 não deve ser presumido compatível com o EF Core 9.
* **Versão do .NET:** confirme os requisitos do .NET para a versão escolhida do EF Core e do provedor.
* **Versão do MySQL:** verifique se o servidor utilizado está entre as versões testadas ou oficialmente suportadas pelo provedor.
* **Tipos de dados:** alguns tipos e recursos do MySQL podem ter comportamentos diferentes dos de outros bancos relacionais.
* **Consultas e funções:** nem todas as expressões LINQ podem ser traduzidas da mesma forma para SQL pelo provedor.
* **Migrations:** alterações automáticas no esquema devem ser revisadas antes de serem aplicadas, principalmente quando envolverem dados existentes.
* **Desempenho:** consultas, índices, carregamento de relacionamentos e quantidade de dados devem ser avaliados conforme a necessidade da aplicação.
* **Dependência do provedor:** recursos específicos de um banco podem dificultar uma futura troca de provedor ou de sistema de banco de dados.

A equipe deve consultar a documentação do provedor escolhido antes de definir as versões definitivas.

## 9. Boas práticas de desenvolvimento

Para manter a integração organizada e segura, recomenda-se:

* Utilizar versões compatíveis e manter os pacotes atualizados de forma controlada.
* Armazenar as credenciais fora do código-fonte.
* Separar configurações de desenvolvimento, teste e produção.
* Testar consultas e migrations em um ambiente de desenvolvimento.
* Criar índices e restrições conforme as necessidades do modelo de dados.
* Evitar carregar grandes volumes de dados sem necessidade.
* Revisar as migrations antes de aplicá-las em ambientes importantes.
* Definir uma estratégia de backup e recuperação.
* Documentar a configuração e os procedimentos específicos do projeto.

## 10. Considerações para a equipe

Antes de adotar o EF Core com MySQL em um projeto, a equipe deve confirmar:

1. Qual versão do .NET será utilizada.
2. Qual versão do EF Core será adotada.
3. Qual provedor MySQL será utilizado.
4. Qual versão do MySQL será suportada.
5. Como a string de conexão será configurada em cada ambiente.
6. Como as migrations serão criadas, revisadas e aplicadas.
7. Como os testes de integração com o banco serão realizados.
8. Quem será responsável por manter e atualizar as dependências.

Essas decisões devem ser registradas no repositório ou na documentação do projeto para facilitar a manutenção.

## 11. Questões em aberto e próximos passos

Os seguintes pontos devem ser confirmados pela equipe antes de uma implementação:

* Definir o provedor MySQL e as versões compatíveis que serão utilizadas.
* Confirmar se o objetivo é utilizar EF Core em um novo projeto .NET ou apenas estudar a tecnologia para referência futura.
* Definir se o gerenciamento do esquema será feito pelo EF Core Migrations, pelo Liquibase ou por outro processo.
* Definir a estratégia de configuração e armazenamento de credenciais.
* Estabelecer como serão executados os testes de integração e as atualizações do banco.
* Solicitar a revisão da documentação pela equipe responsável.

Como o projeto Users API atual utiliza Java e Spring Boot, a adoção do EF Core exigiria uma aplicação ou serviço desenvolvido em .NET. Esta pesquisa não implica a substituição da implementação Java existente.

## 12. Conclusão

O Entity Framework Core pode ser utilizado com MySQL por meio de um provedor compatível, permitindo mapear entidades C# para tabelas e gerenciar operações de persistência com uma abordagem orientada a objetos.

Uma integração confiável depende da escolha correta das dependências, da configuração segura da conexão, da compatibilidade entre as versões e de um processo controlado para revisar e aplicar migrations.

Esta pesquisa oferece uma referência inicial para a equipe. A configuração definitiva deve ser validada conforme os requisitos do projeto e a documentação oficial das tecnologias escolhidas.

## 13. Referências

* Microsoft Learn — Provedores de banco de dados do EF Core: https://learn.microsoft.com/en-us/ef/core/providers/
* Pomelo Entity Framework Core MySQL — Repositório e compatibilidade: https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql
* Pomelo — Opções de configuração: https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql/wiki/Configuration-Options
* Microsoft Learn — Documentação do Entity Framework Core: https://learn.microsoft.com/en-us/ef/core/