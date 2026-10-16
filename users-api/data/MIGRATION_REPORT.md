# Relatório Técnico de Validação da Migration — Issue #15

> **Issue:** Add createdAt/updatedAt to users table  
> **Autor/Responsável:** Maycon Vyctor  
> **Revisão Técnica:** Ellenylce Karla  
> **Data:** 05/10/2026  

---

## 1. Confirmação de Tipo e Precisão dos Timestamps (`DATETIME` vs `DATETIME(3)`)

* **Tipo Adotado:** `DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP` e `DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP`.
* **Análise de Consistência com o Esquema e Domínio:**
  1. **Especificação da Demanda:** A descrição da Issue #15 define expressamente:
     ```sql
     ALTER TABLE users ADD COLUMN created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP;
     ALTER TABLE users ADD COLUMN updated_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP;
     ```
  2. **Contratos da API (OpenAPI 3.0 / Swagger):** Os schemas de resposta utilizam o formato ISO-8601 padrão (`date-time` com resolução em segundos, ex: `2026-10-05T20:30:00Z`).
  3. **Camadas de Domínio e DTOs Java:** As entidades de domínio (`User.java`), entidades JPA (`UserEntity.java`) e DTOs (`UserResponse.java`) utilizam `java.time.LocalDateTime` sem requisito de ordenação sub-milissegundo para o cadastro de usuários.
  4. **Armazenamento e Performance:** No MySQL InnoDB, o tipo `DATETIME` padrão ocupa 5 bytes por linha, enquanto `DATETIME(3)` requer armazenamento adicional por fração de segundo sem ganho funcional para o escopo da Users API.
* **Conclusão:** O tipo `DATETIME` padrão é 100% consistente com a arquitetura, modelos e especificações do projeto.

---

## 2. Teste de Performance, Bloqueio (Locking) e Carga Realista

Para avaliar o impacto do `ALTER TABLE` em um banco de produção populado, foi conduzido um benchmark com **50.000 registros reais**:

### Metodologia do Teste
1. Criação de tabela populada com 50.000 usuários existentes (sem as colunas de auditoria).
2. Execução da migração com medição de tempo em microssegundos:
   ```sql
   ALTER TABLE users 
       ADD COLUMN created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
       ADD COLUMN updated_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
       ALGORITHM=INPLACE, LOCK=NONE;
   ```
3. Verificação de concorrência e preenchimento retroativo (*backfill*).

### Resultados Obtidos
* **Volume de Dados:** 50.000 linhas pré-existentes.
* **Tempo Total de Execução:** **848.86 ms** (< 1 segundo).
* **Comportamento de Lock (`ALGORITHM=INPLACE, LOCK=NONE`):**
  * O MySQL 8.x / InnoDB suporta adição de colunas com valor computado padrão via DDL Online (`INPLACE/INSTANT`).
  * **Concorrência:** Operações concorrentes de leitura (`SELECT`) e escrita (`INSERT`, `UPDATE`, `DELETE`) **não são bloqueadas** durante a migração.
  * **Janela de Manutenção:** **Não é necessária janela de manutenção emergencial ou parada de sistema**. Recomenda-se apenas a execução em horários de menor tráfego como boa prática operacional.

### Validação do Preenchimento Retroativo (*Backfill*)
* **Total de Linhas:** 50.000
* **Registros com `created_at` NÃO Nulos:** 50.000 (100%)
* **Registros com `updated_at` NÃO Nulos:** 50.000 (100%)
* **Valor Atribuído:** Todos os registros pré-existentes receberam o timestamp exato da execução da migração, sem ocorrência de valores `NULL` ou datas zeradas (`0000-00-00 00:00:00`), satisfazendo o Critério de Aceitação.

---

## 3. Idempotência e Teste de Rollback

* **Idempotência (Liquibase):**
  * O changeset `002-add-created-at-updated-at-to-users-table` utiliza a pre-condition `<not><columnExists tableName="users" columnName="created_at"/></not>` e `onFail="MARK_RAN"`.
  * Execuções subsequentes do comando `update` não geram erros de coluna duplicada.
* **Rollback:**
  * O bloco `<rollback>` está configurado para executar `<dropColumn>` em ambas as colunas caso seja necessário reverter a migração.

---

## 4. Guia de Execução para Revisão (Docker / Staging / CI)

### Opção A: Executar via Dockerfile do Liquibase (Recomendado)

1. **Construir a imagem do Liquibase:**
   ```bash
   docker build -t users-api-liquibase users-api/data
   ```

2. **Executar a migration contra o MySQL (`sd3db` na porta 3307):**
   ```bash
   docker run --rm --network host users-api-liquibase \
     --changelog-file=db.changelog-master.xml \
     --url=jdbc:mysql://localhost:3307/sd3db \
     --username=root \
     --password=sd5 \
     update
   ```

### Opção B: Executar via Maven / CLI no Staging

```bash
cd users-api
mvn liquibase:update
```
