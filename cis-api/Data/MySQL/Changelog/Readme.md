# MySQL Database Migrations

## Objetivo

Este diretório contém os changelogs do Liquibase usados para gerenciar as alterações
de estrutura do banco de dados `sd3_cisdb`.

O objetivo é manter as mudanças no banco versionadas no Git, evitando a necessidade
de executar manualmente comandos `CREATE TABLE IF NOT EXISTS`.

## Estrutura

- `main.xml`: arquivo principal que inclui os changelogs da pasta `Changes`.
- `Changes/`: diretório para os arquivos XML de migração.
- `liquibase.properties`: configurações básicas do Liquibase.

## Como funciona

O serviço `migrations-mysql`, definido em `Data/compose.yml`, executa o comando
`liquibase status` para consultar as migrações pendentes no banco `sd3_cisdb`.

## Como adicionar uma migração

1. Crie um arquivo de changelog dentro de `Changes/`.
2. Defina nele as alterações de banco necessárias usando o formato do Liquibase.
3. Verifique o status das migrações antes de aplicar alterações.
4. A aplicação das migrações deve ser feita de forma explícita e validada.

## Configuração da conexão

A URL de conexão é fornecida pela variável de ambiente `sd3_cisdb_CONNECTION`.
Não coloque senhas ou credenciais reais neste arquivo.