# 🚀 The Big Bang Theory API

![.NET](https://img.shields.io/badge/.NET-8-512BD4?logo=.net)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-Web%20API-512BD4)
![React](https://img.shields.io/badge/React-19-61DAFB?logo=react)
![SQL Server](https://img.shields.io/badge/SQL%20Server-Database-CC2927?logo=microsoftsqlserver)
![Entity Framework Core](https://img.shields.io/badge/Entity%20Framework-Core-512BD4)
![Swagger](https://img.shields.io/badge/Swagger-OpenAPI-85EA2D?logo=swagger)
![License](https://img.shields.io/badge/License-MIT-success)

Uma **REST API** desenvolvida com **ASP.NET Core (.NET 8)** inspirada no universo da série **The Big Bang Theory**, acompanhada por um **frontend em React** para consumo da API.

O projeto foi desenvolvido com foco em boas práticas de arquitetura, organização em camadas e conceitos utilizados no desenvolvimento de aplicações modernas.
## ⚠️ Aviso

Este projeto foi desenvolvido exclusivamente para fins de estudo e demonstração de conhecimentos em ASP.NET Core, Entity Framework Core e desenvolvimento de APIs REST.

A franquia **The Big Bang Theory**, seus personagens, nomes e imagens pertencem aos seus respectivos detentores de direitos autorais. Este projeto não possui qualquer vínculo oficial com a Warner Bros. ou seus proprietários e não tem finalidade comercial.
---

# 🌐 Demonstração

| Aplicação | Link |
|------------|------|
| API | https://bigbangapi.somee.com |
| Swagger | https://bigbangapi.somee.com/swagger |
| Documentação | https://bigbangapi.somee.com/docs |

---

# 📸 Preview

## Frontend

<img width="745" height="638" alt="image" src="https://github.com/user-attachments/assets/312eec1b-a270-4e66-a079-342955943fa5" />


## Swagger

<img width="911" height="652" alt="image" src="https://github.com/user-attachments/assets/239caea8-095f-4429-909d-fc17394b6174" />


---

# ✨ Funcionalidades

### 👥 Personagens

- CRUD completo
- Consulta por ID
- Paginação
- Imagens dinâmicas
- Página de detalhes

### 📺 Episódios

- CRUD completo

### 📍 Localizações

- CRUD completo

### ⚙ Recursos

- DTOs
- AutoMapper
- Paginação
- JSON Patch
- Middleware
- Action Filters
- Exception Filters
- Logging Personalizado
- Servir arquivos estáticos (wwwroot)
- Swagger / OpenAPI
- Seed de Dados

---

# 🏗 Arquitetura

```text
                React + Vite
                      │
                 Axios (HTTP)
                      │
                      ▼
            ASP.NET Core Web API
                      │
         ┌────────────┴────────────┐
         ▼                         ▼
 Repository Pattern         Middleware
         │
         ▼
     Unit of Work
         │
         ▼
 Entity Framework Core
         │
         ▼
      SQL Server
```

---

# 🛠 Tecnologias

## Backend

- ASP.NET Core 8
- C#
- Entity Framework Core
- SQL Server
- AutoMapper
- Swagger / OpenAPI
- JSON Patch

## Frontend

- React
- Vite
- React Router
- Axios
- CSS Modules

## Ferramentas

- Git
- GitHub
- Visual Studio
- VS Code
- Somee Hosting

---

# 📚 Conceitos Aplicados

### Arquitetura

- REST API
- Repository Pattern
- Generic Repository
- Unit of Work
- Dependency Injection

### Persistência

- Entity Framework Core
- LINQ
- SQL Server
- Relacionamentos One-to-Many (1:N)
- Relacionamentos Many-to-Many (N:N)

### API

- DTO Pattern
- AutoMapper
- Paginação
- JSON Patch
- Swagger / OpenAPI

### Pipeline

- Middleware
- Action Filters
- Exception Filters
- Tratamento Global de Exceções

### Logging

- ILogger
- Custom Logger Provider
- Logs em arquivo

### Frontend

- React
- Axios
- React Router
- CSS Modules
- Consumo de APIs REST

---

# 📂 Estrutura do Projeto

```text
Projeto-BigBangAPI
│
├── BigBangAPI
│   ├── Controllers
│   ├── Context
│   ├── DTOs
│   ├── Extensions
│   ├── Filters
│   ├── Logging
│   ├── Migrations
│   ├── Models
│   ├── Pagination
│   ├── Properties
│   ├── Repositories
│   ├── Validations
│   ├── wwwroot
│   │   └── Images
│   │       └── characters
│   ├── Program.cs
│   ├── appsettings.json
│   └── BigBangAPI.csproj
│
├── frontend
│   ├── public
│   ├── src
│   │   ├── assets
│   │   ├── components
│   │   ├── pages
│   │   ├── routes
│   │   ├── services
│   │   └── styles
│   ├── package.json
│   ├── vite.config.js
│   └── .env.example
│
└── README.md
```

---

# ⚙ Como executar

## Backend

Clone o repositório:

```bash
git clone https://github.com/AnolinoJunior/BigBangAPI.git
```

Entre na pasta:

```bash
cd BigBangAPI/BigBangAPI
```

Restaure as dependências:

```bash
dotnet restore
```

Execute as migrations:

```bash
dotnet ef database update
```

Inicie a API:

```bash
dotnet run
```

A API estará disponível em:

```text
http://localhost:5004
```

Swagger:

```text
http://localhost:5004/swagger
```

---

## Frontend

Entre na pasta:

```bash
cd frontend
```

Instale as dependências:

```bash
npm install
```

Crie um arquivo `.env` baseado no exemplo:

```env
VITE_API_BASE_URL=http://localhost:5004/api
```

Execute:

```bash
npm run dev
```

Frontend:

```text
http://localhost:5173
```

---

# 🎯 Objetivos

Este projeto foi desenvolvido para praticar:

- Desenvolvimento de APIs REST
- Entity Framework Core
- Arquitetura em Camadas
- Repository Pattern
- Unit of Work
- AutoMapper
- React consumindo APIs
- Organização de código
- Boas práticas de desenvolvimento
- Deploy em ambiente real

---

# 🚀 Próximas melhorias

- JWT Authentication
- Refresh Token
- Versionamento da API
- FluentValidation
- Docker
- Redis Cache
- Testes Unitários
- Testes de Integração
- Health Checks
- CI/CD com GitHub Actions

---

# 👨‍💻 Autor

**Anolino Alves dos Santos Junior**

- GitHub: https://github.com/AnolinoJunior
- LinkedIn: https://www.linkedin.com/in/anolino-alves-3b1b95158

---

# 📄 Licença

Este projeto está licenciado sob a licença MIT.
