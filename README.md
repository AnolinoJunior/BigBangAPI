# 🚀 API de The Big Bang Theory

![.NET](https://img.shields.io/badge/.NET-8-512BD4?logo=.net)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-Web%20API-512BD4)
![React](https://img.shields.io/badge/React-19-61DAFB?logo=react)
![SQL Server](https://img.shields.io/badge/SQL%20Server-Database-CC2927?logo=microsoftsqlserver)
![Entity Framework Core](https://img.shields.io/badge/Entity%20Framework-Core-512BD4)
![Swagger](https://img.shields.io/badge/Swagger-OpenAPI-85EA2D?logo=swagger)
![Licença](https://img.shields.io/badge/Licen%C3%A7a-MIT-success)

Uma **API REST** desenvolvida com **ASP.NET Core (.NET 8)**, inspirada no universo da série **The Big Bang Theory** e acompanhada por uma aplicação web em React.

O projeto foi desenvolvido com foco em boas práticas de arquitetura, organização em camadas e conceitos utilizados no desenvolvimento de aplicações modernas.
## ⚠️ Aviso

Este projeto foi desenvolvido exclusivamente para fins de estudo e demonstração de conhecimentos em ASP.NET Core, Entity Framework Core e desenvolvimento de APIs REST.

A franquia **The Big Bang Theory**, seus personagens, nomes e imagens pertencem aos seus respectivos detentores de direitos autorais. Este projeto não possui qualquer vínculo oficial com a Warner Bros. ou seus proprietários e não tem finalidade comercial.
---

# 🌐 Demonstração

| Aplicação | Endereço |
|------------|------|
| API | https://bigbangapi.somee.com |
| Swagger | https://bigbangapi.somee.com/swagger |
| Documentação | https://bigbangapi.somee.com/docs |

---

# 📸 Prévia

## Interface web

<img width="745" height="638" alt="Captura de tela da interface web" src="https://github.com/user-attachments/assets/312eec1b-a270-4e66-a079-342955943fa5" />


## Swagger

<img width="911" height="652" alt="Captura de tela do Swagger" src="https://github.com/user-attachments/assets/239caea8-095f-4429-909d-fc17394b6174" />


---

# ✨ Funcionalidades

### 👥 Personagens

- CRUD completo
- Consulta por identificador
- Paginação
- Imagens dinâmicas
- Página de detalhes

### 📺 Episódios

- CRUD completo

### 📍 Localizações

- CRUD completo

### ⚙ Recursos

- DTOs (objetos de transferência de dados)
- AutoMapper
- Paginação
- JSON Patch
- Middleware
- Filtros de ação
- Filtros de exceção
- Registro personalizado de eventos
- Servir arquivos estáticos (`wwwroot`)
- Swagger / OpenAPI
- Carga inicial de dados

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
 Padrão Repository         Middleware
         │
         ▼
     Unidade de trabalho
         │
         ▼
 Entity Framework Core
         │
         ▼
      SQL Server
```

---

# 🛠 Tecnologias

## Servidor

- ASP.NET Core 8
- C#
- Entity Framework Core
- SQL Server
- AutoMapper
- Swagger / OpenAPI
- JSON Patch

## Aplicação web

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
- Hospedagem Somee

---

# 📚 Conceitos aplicados

### Arquitetura

- API REST
- Padrão de repositório
- Repositório genérico
- Unidade de trabalho
- Injeção de dependência

### Persistência

- Entity Framework Core
- LINQ
- SQL Server
- Relacionamentos um para muitos (1:N)
- Relacionamentos muitos para muitos (N:N)

### API

- Padrão DTO
- AutoMapper
- Paginação
- JSON Patch
- Swagger / OpenAPI

### Pipeline

- Middleware
- Action Filters
- Exception Filters
- Tratamento Global de Exceções

### Registro de eventos

- ILogger
- Provedor de registro personalizado
- Registros em arquivo

### Aplicação web

- React
- Axios
- React Router
- CSS Modules
- Consumo de APIs REST

---

# 📂 Estrutura do projeto

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

## Servidor

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

Para atualizar manualmente o banco de desenvolvimento, execute as migrations:

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

## Aplicação web

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

Aplicação web:

```text
http://localhost:5173
```

---

# 🎯 Objetivos

Este projeto foi desenvolvido para praticar:

- Desenvolvimento de APIs REST
- Entity Framework Core
- Arquitetura em Camadas
- Padrão de repositório
- Unidade de trabalho
- AutoMapper
- React consumindo APIs
- Organização de código
- Boas práticas de desenvolvimento
- Publicação em ambiente real

---

# 🚀 Próximas melhorias

- Autenticação JWT
- Token de atualização
- Versionamento da API
- FluentValidation
- Docker
- Redis Cache
- Testes unitários
- Testes de integração
- Verificações de disponibilidade
- CI/CD com GitHub Actions

---

# 👨‍💻 Autor

**Anolino Alves dos Santos Junior**

- GitHub: https://github.com/AnolinoJunior
- LinkedIn: https://www.linkedin.com/in/anolino-alves-3b1b95158

---

# 📄 Licença

Este projeto está licenciado sob a licença MIT.
