# 🍔 GoodHamburger API

API REST para gerenciamento de pedidos de uma hamburgueria, desenvolvida em C# com .NET 9 e ASP.NET Core.

---

## 📋 Índice

- [Tecnologias](#tecnologias)
- [Pré-requisitos](#pré-requisitos)
- [Como executar](#como-executar)
- [Estrutura do projeto](#estrutura-do-projeto)
- [Decisões de arquitetura](#decisões-de-arquitetura)
- [Cardápio e regras de desconto](#cardápio-e-regras-de-desconto)
- [Endpoints](#endpoints)
- [Testes](#testes)
- [O que ficou fora](#o-que-ficou-fora)

---

## 🛠️ Tecnologias

- **.NET 9** com ASP.NET Core
- **Entity Framework Core** com SQL Server
- **MediatR** para CQRS
- **FluentValidation** para validação
- **Swashbuckle** para documentação (Swagger)
- **Blazor WebAssembly** para o frontend
- **xUnit** + **FluentAssertions** + **NSubstitute** + **Bogus** para testes

---

## ✅ Pré-requisitos

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [SQL Server](https://www.microsoft.com/pt-br/sql-server/sql-server-downloads) (ou SQL Server Express)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) ou [VS Code](https://code.visualstudio.com/)

---

## 🚀 Como executar

### 1. Clone o repositório

```bash
git clone https://github.com/seu-usuario/GoodHamburger.git
cd GoodHamburger
```

### 2. Configure a connection string

No arquivo `GoodHamburger.API/appsettings.json`, ajuste a connection string para o seu SQL Server:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=GoodHamburger;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

### 3. Execute as migrations

```bash
dotnet ef database update --project GoodHamburger.Infrastructure --startup-project GoodHamburger.API
```

### 4. Execute a API

```bash
cd GoodHamburger.API
dotnet run
```

A API estará disponível em `https://localhost:7048`. O Swagger estará em `https://localhost:7048/swagger`.

### 5. Execute o frontend Blazor (opcional)

```bash
cd GoodHamburger.Blazor
dotnet run
```

O frontend estará disponível em `https://localhost:7108`.

---

## 📁 Estrutura do projeto

```
GoodHamburger/
├── GoodHamburger.API/            → Controllers, Middlewares, Program.cs
├── GoodHamburger.Application/    → Handlers, Commands, Queries, Validators, DTOs
├── GoodHamburger.Domain/         → Entidades, Enums, Exceções, Repositórios (interfaces)
├── GoodHamburger.Infrastructure/ → EF Core, Repositórios (implementações), Migrations
├── GoodHamburger.Blazor/         → Frontend WebAssembly
└── GoodHamburger.Tests/          → Testes unitários
```

---

## 🏗️ Decisões de arquitetura

### Arquitetura em camadas
O projeto foi dividido em quatro camadas com dependências unidirecionais — as camadas internas não conhecem as externas. Isso permite trocar o banco de dados ou o framework HTTP sem tocar nas regras de negócio.

### CQRS com MediatR
Commands e Queries foram separados para deixar claro o que modifica e o que apenas lê dados. O MediatR desacopla os controllers dos handlers e permite adicionar comportamentos transversais via Pipeline Behaviors, como a validação automática com FluentValidation.

### Repository Pattern + Unit of Work
O repositório abstrai o acesso ao banco, permitindo trocar a implementação sem afetar os handlers. O Unit of Work garante que todas as operações de um handler sejam salvas juntas ou não sejam salvas de jeito nenhum.

### Domínio rico
As entidades do domínio têm setters privados e métodos que encapsulam as regras de negócio. O estado só pode ser modificado através de métodos como `AdicionarItem` e `LimparItens`, garantindo que as validações sempre sejam executadas.

### Exceções customizadas
Foram criadas `NegocioException` (400) e `NaoEncontradoException` (404) para diferenciar erros de negócio de erros inesperados. O `ExceptionMiddleware` centraliza o tratamento e retorna respostas HTTP adequadas sem necessidade de try/catch em cada controller.

### Validação do cardápio
Os validators verificam se o item enviado existe no cardápio e se o preço bate. O cardápio é centralizado em `CardapioItems` no domínio, evitando duplicação entre o validator e o controller.

---

## 🍔 Cardápio e regras de desconto

### Itens disponíveis

| Item | Tipo | Preço |
|------|------|-------|
| X-Burger | Sanduíche | R$ 5,00 |
| X-Egg | Sanduíche | R$ 4,50 |
| X-Bacon | Sanduíche | R$ 7,00 |
| Batata frita | Acompanhamento | R$ 2,00 |
| Refrigerante | Bebida | R$ 2,50 |

### Regras de desconto

| Combinação | Desconto |
|------------|----------|
| Sanduíche + Batata + Refrigerante | 20% |
| Sanduíche + Refrigerante | 15% |
| Sanduíche + Batata | 10% |

> Cada pedido pode conter apenas um sanduíche, uma batata e um refrigerante.

---

## 📡 Endpoints

### Cardápio
| Método | Rota | Descrição |
|--------|------|-----------|
| GET | `/api/Cardapio` | Retorna o cardápio completo com regras de desconto |

### Pedidos
| Método | Rota | Descrição |
|--------|------|-----------|
| POST | `/api/Pedido` | Cria um novo pedido |
| GET | `/api/Pedido` | Lista todos os pedidos |
| GET | `/api/Pedido/{id}` | Busca um pedido por ID |
| PUT | `/api/Pedido/{id}` | Atualiza um pedido existente |
| DELETE | `/api/Pedido/{id}` | Remove um pedido |

### Exemplo de request — Criar pedido

```json
POST /api/Pedido
{
  "itens": [
    { "tipo": 1, "nome": "X-Burger", "preco": 5.00 },
    { "tipo": 2, "nome": "Batata frita", "preco": 2.00 },
    { "tipo": 3, "nome": "Refrigerante", "preco": 2.50 }
  ]
}
```

### Exemplo de response — 201 Created

```json
{
  "id": 1,
  "subtotal": 9.50,
  "desconto": 1.90,
  "total": 7.60,
  "itens": [
    { "tipo": "Sanduiche", "nome": "X-Burger", "preco": 5.00 },
    { "tipo": "Batata", "nome": "Batata frita", "preco": 2.00 },
    { "tipo": "Refrigerante", "nome": "Refrigerante", "preco": 2.50 }
  ]
}
```

### Tipos de item
| Valor | Tipo |
|-------|------|
| 1 | Sanduíche |
| 2 | Acompanhamento (Batata) |
| 3 | Bebida (Refrigerante) |

---

## 🧪 Testes

Os testes unitários cobrem:

- Lógica de desconto e validações do domínio (`PedidoTests`)
- Handlers de criação, atualização e deleção (`CriarPedidoHandlerTests`, `AtualizarPedidoHandlerTests`, `DeletarPedidoHandlerTests`)
- Validators de criação e atualização (`CriarPedidoValidatorTests`, `AtualizarPedidoValidatorTests`)

Para executar os testes:

```bash
dotnet test
```

---

## 🚧 O que ficou fora

As funcionalidades abaixo foram identificadas como melhorias mas não foram implementadas nesta versão:

**Paginação** — O endpoint `GET /api/Pedido` retorna todos os pedidos sem paginação. Em produção seria necessário adicionar parâmetros `page` e `pageSize`.

**Logs** — Não foi implementado um sistema de logs estruturados. O ideal seria usar `ILogger` nos handlers para registrar operações e erros.

**Versionamento da API** — As rotas não estão versionadas. O padrão `/api/v1/` facilitaria evoluções futuras sem quebrar clientes existentes.

**Autenticação e autorização** — Os endpoints não têm proteção. Em produção seria necessário implementar autenticação via JWT.

**Testes de integração** — Foram implementados apenas testes unitários. Testes de integração que sobem a API em memória e batem no banco real aumentariam a cobertura.

**Cardápio dinâmico** — O cardápio é estático, definido em código. Em produção seria ideal gerenciar os itens via banco de dados com endpoints de CRUD.

**Docker** — A aplicação não está containerizada. Um `Dockerfile` e `docker-compose` facilitariam o deploy e a execução em diferentes ambientes.