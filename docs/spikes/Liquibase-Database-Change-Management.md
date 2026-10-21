# Gerenciamento de Alterações no Banco de Dados com Liquibase

## 1. Introdução

O Liquibase é uma ferramenta utilizada para gerenciar alterações na estrutura e nos dados de um banco de dados. Ele permite registrar, organizar e aplicar mudanças de forma controlada, facilitando a manutenção e a evolução do banco durante o desenvolvimento do software.

O objetivo desta documentação é apresentar os principais conceitos do Liquibase, explicar como trabalhar com alterações no banco de dados e estabelecer orientações para que a equipe possa criar, revisar, aplicar e manter essas alterações de maneira organizada.

## 2. O que é o Liquibase?

O Liquibase é uma ferramenta de gerenciamento de mudanças em bancos de dados. Ele utiliza arquivos chamados _changelogs_ para descrever as alterações que precisam ser executadas.

Essas alterações podem incluir:

* Criação e remoção de tabelas.
* Adição, alteração e remoção de colunas.
* Criação de índices e restrições.
* Alterações em dados existentes.
* Execução controlada de scripts SQL.

O Liquibase mantém um registro das alterações aplicadas, permitindo identificar quais mudanças já foram executadas em determinado banco de dados.

## 3. Principais conceitos

### 3.1 Changelog

O _changelog_ é o arquivo que organiza as alterações do banco de dados. Ele pode ser escrito em XML, YAML, JSON ou SQL formatado.

Nesse arquivo, são definidos os conjuntos de alterações que o Liquibase deve executar.

### 3.2 Changeset

O _changeset_ representa uma unidade de alteração dentro do changelog. Cada changeset possui um identificador, um autor e as instruções que serão executadas.

A combinação do identificador, do autor e do arquivo de origem permite que o Liquibase reconheça cada changeset e controle sua execução.

**Orientação:** depois que um changeset for aplicado em um ambiente compartilhado, evite modificar seu conteúdo. Para realizar uma nova alteração, crie outro changeset.

### 3.3 DATABASECHANGELOG

A tabela `DATABASECHANGELOG` registra os changesets que já foram executados. Ela permite que o Liquibase verifique quais alterações ainda precisam ser aplicadas.

### 3.4 DATABASECHANGELOGLOCK

A tabela `DATABASECHANGELOGLOCK` controla o bloqueio durante a execução das atualizações, ajudando a evitar que múltiplas execuções do Liquibase apliquem mudanças simultaneamente no mesmo banco.

## 4. Organização dos arquivos

Uma organização possível para um projeto é manter os changelogs em uma pasta específica, separados por finalidade ou versão.

Exemplo de estrutura:

`src/
└── main/
    └── resources/
        └── db/
            └── changelog/
                ├── db.changelog-master.yaml
                └── changes/
                    ├── 001-create-users-table.yaml
                    ├── 002-add-user-columns.yaml
                    └── 003-create-user-index.yaml`

O arquivo principal, `db.changelog-master.yaml`, referencia os arquivos que contêm as alterações.

Essa estrutura é apenas uma sugestão. A equipe deve seguir a organização já adotada pelo projeto para evitar duplicidade e manter a consistência.

## 5. Fluxo de trabalho com Liquibase

O fluxo recomendado para gerenciar alterações no banco de dados é:

1. **Identificar a necessidade:** determinar qual mudança precisa ser feita no banco.
2. **Criar um changeset:** registrar a alteração em um novo changeset, seguindo as convenções do projeto.
3. **Atualizar o changelog principal:** incluir a referência ao novo arquivo, quando a organização utilizada exigir isso.
4. **Revisar a alteração:** conferir o SQL ou as instruções geradas, as dependências e os possíveis impactos nos dados existentes.
5. **Testar a mudança:** aplicar a alteração em um banco de desenvolvimento ou teste.
6. **Validar o resultado:** verificar se a estrutura e os dados ficaram conforme o esperado.
7. **Revisar e integrar o código:** enviar as alterações para revisão pela equipe antes da integração à branch principal.
8. **Aplicar no ambiente correspondente:** executar a atualização por meio do processo definido para aquele ambiente.
9. **Registrar e acompanhar:** verificar os logs e o histórico de changesets aplicados.

## 6. Comandos úteis

Os comandos disponíveis dependem da forma como o Liquibase foi configurado no projeto. Quando integrado ao Maven, alguns exemplos são:

`mvn liquibase:status`

Verifica quais alterações estão pendentes de execução.

`mvn liquibase:updateSQL`

Gera o SQL que seria executado pelo comando de atualização, permitindo revisar as instruções antes de aplicá-las.

`mvn liquibase:update`

Aplica as alterações pendentes no banco configurado.

**Atenção:** execute os comandos somente após confirmar as configurações de conexão, o banco de destino e o perfil de ambiente utilizado. Os comandos acima dependem de o plugin Maven do Liquibase estar configurado no projeto.

## 7. Boas práticas

Para facilitar a manutenção do banco de dados, recomenda-se:

* Criar changesets pequenos, com objetivos claros.
* Utilizar identificadores únicos e nomes descritivos.
* Não modificar changesets já aplicados em ambientes compartilhados.
* Revisar as alterações antes de executá-las.
* Testar mudanças em ambiente de desenvolvimento antes de aplicá-las em outros ambientes.
* Evitar credenciais e informações sensíveis diretamente nos arquivos versionados.
* Considerar o impacto das mudanças sobre os dados existentes e sobre versões anteriores da aplicação.
* Definir procedimentos para backup e recuperação antes de alterações de alto risco.
* Documentar mudanças que exigem migração ou transformação de dados.
* Manter os changelogs versionados junto ao código da aplicação.

## 8. Rollback e recuperação

O Liquibase oferece recursos para reverter determinadas alterações, dependendo do tipo de changeset e de como a reversão foi definida.

Nem toda mudança pode ser revertida automaticamente com segurança. Por exemplo, a remoção de uma coluna pode causar perda de dados que não podem ser recuperados apenas com a execução de um rollback.

Por isso, antes de executar mudanças em ambientes importantes, a equipe deve avaliar se existe uma estratégia de reversão adequada, se é necessário criar instruções específicas de rollback e se há um backup disponível.

O rollback não substitui o backup nem elimina a necessidade de testar os procedimentos de recuperação.

## 9. Revisão e manutenção

Antes de aprovar um changeset, a equipe deve verificar:

* Se a alteração atende ao requisito que a originou.
* Se o identificador do changeset é único.
* Se o arquivo está referenciado no changelog principal, quando necessário.
* Se a mudança funciona no banco de dados utilizado pelo projeto.
* Se há riscos de perda ou inconsistência de dados.
* Se a alteração afeta a aplicação ou outras migrations.
* Se a execução pode ser repetida com segurança no fluxo do Liquibase.
* Se existe uma estratégia adequada de recuperação, quando aplicável.

A revisão deve considerar tanto a alteração isolada quanto seu impacto no restante do sistema.

## 10. Considerações para o projeto

A adoção do Liquibase deve seguir a configuração existente no repositório. Antes de criar novos arquivos ou executar comandos, os desenvolvedores devem identificar:

* Onde estão os changelogs atuais.
* Qual arquivo é o changelog principal.
* Qual banco de dados e qual ambiente serão utilizados.
* Como as credenciais de conexão são fornecidas.
* Como o Liquibase é executado: pelo Maven, pela aplicação ou por outro processo.
* Como as alterações são testadas e aplicadas nos diferentes ambientes.

Essas informações devem ser confirmadas no código e na configuração do projeto. Os exemplos desta página servem como referência geral e não substituem as instruções específicas do repositório.

## 11. Questões em aberto e próximos passos

Durante a pesquisa e a adoção da ferramenta, a equipe deve confirmar e documentar os seguintes pontos:

* Qual formato de changelog será adotado como padrão.
* Qual convenção de nomes e identificadores será utilizada.
* Qual processo será responsável por aplicar as alterações em cada ambiente.
* Como as migrations serão testadas automaticamente.
* Como serão tratados erros de execução e alterações parcialmente aplicadas.
* Qual procedimento de backup e recuperação será utilizado.
* Como serão realizadas alterações incompatíveis com versões anteriores da aplicação.
* Se o processo de implantação exige aprovação antes da execução das migrations.

Essas decisões devem ser registradas pela equipe para que todos os desenvolvedores sigam o mesmo fluxo.

## 12. Conclusão

O Liquibase ajuda a controlar e documentar a evolução do banco de dados por meio de changelogs e changesets versionados. Quando utilizado com convenções claras, revisão de código e testes, ele facilita a manutenção do banco e reduz o risco de alterações inconsistentes entre ambientes.

Esta documentação serve como referência inicial para a equipe. As convenções definitivas, os comandos de execução e os procedimentos de recuperação devem ser confirmados de acordo com a configuração real do projeto.

## 13. Referências

* Documentação oficial do Liquibase: https://docs.liquibase.com/
* Conceitos de changelog: https://docs.liquibase.com/concepts/changelogs/home.html
* Conceitos de changeset: https://docs.liquibase.com/concepts/changelogs/changeset.html
* Comando `update`: https://docs.liquibase.com/commands/update/update.html
* Comando `rollback`: https://docs.liquibase.com/commands/rollback/rollback.html