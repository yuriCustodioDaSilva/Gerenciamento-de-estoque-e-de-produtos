# Gerenciamento de Estoque e de Produtos

Sistema web para gerenciamento de estoque e produtos. Permite controlar entradas, saídas, reservas e ajustes de estoque, com autenticação por login, controle de acesso por perfil de usuário e histórico completo de movimentações.

## Tecnologias utilizadas

**Backend**
- C# / .NET 10
- Entity Framework Core
- SQLite
- ASP.NET Identity + JWT (autenticação e autorização)
- xUnit + FluentAssertions (testes unitários)
- Swagger (documentação interativa da API)

**Frontend**
- Angular 21

## Arquitetura

O backend segue uma separação em camadas:

```
backend/
├── src/
│   ├── Domain/          # Entidades, enums e interfaces (regras de negócio)
│   ├── Application/     # Camada de orquestração (reservada para casos de uso)
│   ├── Infrastructure/  # DbContext, migrations e repositórios (EF Core)
│   └── Api/             # Controllers, autenticação JWT e configuração
└── tests/
    └── UnitTests/       # Testes unitários (xUnit + FluentAssertions)
```

O padrão utilizado é o **Repository Pattern**, as interfaces ficam no Domain e a implementação fica no Infrastructure, deixando as camadas desacopladas.

O frontend é organizado por funcionalidade:

```
frontend/
└── src/app/
    ├── pages/           # Telas: login, dashboard, produtos, estoque
    ├── services/        # Comunicação com a API
    ├── guards/          # Proteção de rotas autenticadas
    └── interceptors/    # Injeção automática do token JWT
```

## Funcionalidades

**Produtos**
- Cadastrar, editar, deletar e consultar produtos

**Estoque**
- Entrada, saída, reserva, cancelamento de reserva e ajuste manual
- Histórico completo de movimentações

**Segurança**
- Login com JWT
- Controle de acesso por perfil:
  - **Administrador** — acesso total ao sistema
  - **Operador** — gerencia estoque e consulta produtos
  - **Consultor** — só visualização de produtos e histórico
- Auditoria: toda movimentação registra quem realizou a operação

## Como rodar o projeto

São necessários dois terminais abertos ao mesmo tempo — um para o backend e um para o frontend.

### Pré-requisitos
- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Node.js LTS](https://nodejs.org)
- Angular CLI: `npm install -g @angular/cli`
- dotnet-ef: `dotnet tool install --global dotnet-ef`

### 1. Clonar o repositório

```bash
git clone <url-do-repositorio>
cd <nome-da-pasta>
```

### 2. Sobre o appsettings.json

O arquivo `backend/src/Api/appsettings.json` foi incluído no repositório para facilitar os testes. Em um projeto real ele estaria no `.gitignore`, com a chave JWT fora do controle de versão.

### 3. Rodar o backend

```bash
cd backend

# Cria o banco SQLite local
dotnet ef database update --project src/Infrastructure/StockManager.Infrastructure.csproj --startup-project src/Api/StockManager.Api.csproj

# Sobe a API
dotnet run --project src/Api
```

A API vai estar disponível na porta exibida no terminal (geralmente `http://localhost:5000`). O Swagger fica em `http://localhost:5000/swagger`.

### 4. Rodar o frontend

Em outro terminal:

```bash
cd frontend
npm install --legacy-peer-deps
ng serve
```

Acesse `http://localhost:4200` no navegador.

> Se a API subir em uma porta diferente de 5000, ajuste o `apiUrl` em `frontend/src/environments/environment.ts`.

### 5. Criar o primeiro usuário

Não existe usuário pré-cadastrado. Crie um Administrador pelo Swagger ou via Postman:

```
POST http://localhost:5000/api/auth/register
```

```json
{
  "email": "admin@teste.com",
  "password": "Admin@123",
  "role": "Administrador"
}
```

Depois é só fazer login pela tela em `http://localhost:4200/login`.

## Rodando os testes

```bash
cd backend
dotnet test tests/UnitTests/StockManager.UnitTests.csproj
```

## Observações

- O banco SQLite é criado automaticamente na primeira execução das migrations, não precisa instalar nada além do .NET.
- A camada `Application` existe na estrutura mas está vazia, porem foi mantida para deixar a arquitetura preparada para crescer, separando futuros serviços e casos de uso dos controllers.
- Por ser um desafio técnico, algumas práticas de produção como rate limiting, logging estruturado e rotação de chaves não foram implementadas.