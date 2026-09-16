# Gerenciador de Finanças

Aplicação de gerenciamento de receitas, despesas e limites. O backend foi
migrado para Java 17 com Spring Boot, Spring Web, Spring Data JPA e SQL Server.
O frontend permanece em React/Vite.

## Pré-requisitos

- Java 17 ou superior
- Maven 3.9 ou superior
- Node.js 18 ou superior (para o frontend)
- SQL Server acessível pela aplicação

## Backend

Na raiz do repositório, configure as variáveis de ambiente abaixo quando os
valores padrão não forem adequados:

| Variável | Padrão | Descrição |
| --- | --- | --- |
| `SERVER_PORT` | `8080` | Porta HTTP da API |
| `DB_URL` | `jdbc:sqlserver://localhost:1433;databaseName=FinancasDB;encrypt=true;trustServerCertificate=true` | URL JDBC do SQL Server |
| `DB_USERNAME` | `sa` | Usuário do banco |
| `DB_PASSWORD` | `YourStrong!Passw0rd` | Senha do banco |
| `JPA_DDL_AUTO` | `update` | Estratégia de criação/atualização do schema |

Execute:

```bash
mvn spring-boot:run
```

A API estará disponível em `http://localhost:8080`. A documentação OpenAPI
fica em `http://localhost:8080/swagger-ui.html`.

Endpoints disponíveis:

- `GET`, `POST`, `PUT`, `DELETE /api/Despesas`
- `GET`, `POST`, `PUT`, `DELETE /api/Receitas`
- `GET`, `POST`, `PUT`, `DELETE /api/Limites`

Os campos da API são `id`, `descricao`, `valor`, `categoria`, `data`, `pago` e
`valorLimite`. Valores monetários usam precisão decimal e datas usam o formato
ISO-8601 (`yyyy-MM-dd`).

## Frontend

```bash
cd Frontend
npm install
npm run dev
```

O frontend usa `http://localhost:8080/api` por padrão. Para apontar para outro
servidor, defina `VITE_API_URL` (incluindo o sufixo `/api`), por exemplo:

```bash
VITE_API_URL=http://localhost:8080/api npm run dev
```

Para gerar a versão de produção:

```bash
npm run build
```
