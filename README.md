![.NET](https://img.shields.io/badge/.NET-8-purple)

![C#](https://img.shields.io/badge/C%23-ASP.NET%20Core-blue)

![SQL Server](https://img.shields.io/badge/SQL%20Server-Database-red)

![Swagger](https://img.shields.io/badge/Swagger-OpenAPI-green)

# 🚀 The Big Bang Theory API

Uma API REST desenvolvida em ASP.NET Core (.NET 8), inspirada na Rick and Morty API, com o objetivo de aplicar conceitos modernos de desenvolvimento backend.

Além do CRUD de personagens, episódios e locais, o projeto implementa recursos como Middleware, Action Filters, Logging Personalizado, Entity Framework Core, DTOs, Paginação e Tratamento Global de Exceções.
---

## 🌎 Demonstração

### API
https://bigbangapi.somee.com

### Swagger
https://bigbangapi.somee.com/swagger

### Site de Documentação
https://bigbangapi.somee.com/docs

---

## 📸 Preview

Em breve.

---

## ✨ Funcionalidades

- CRUD completo de Personagens
- CRUD completo de Episódios
- CRUD completo de Locais
- Consulta por ID
- Paginação
- DTOs para projeção de dados
- Relacionamento One-to-Many
- Relacionamento Many-to-Many
- Restrições de rotas
- Middleware global para tratamento de exceções
- Filtros personalizados (Action Filters)
- Sistema de Logging personalizado
- Logging utilizando ILogger
- Repository Pattern
- Provider de Logging customizado
- Logs gravados automaticamente em arquivo
- Swagger / OpenAPI
- Entity Framework Core
- SQL Server
## 🛠️ Tecnologias

- ASP.NET Core Web API (.NET 8)
- C#
- Entity Framework Core
- SQL Server
- Swagger / OpenAPI
- Git
- GitHub
- Repository Pattern
- Dependency Injection
- Somee Hosting
---
## 🧩 Conceitos Aplicados
- REST API
- Repository Pattern
- Dependency Injection
- Middleware
- Action Filters
- Exception Filters
- Logging (ILogger)
- Custom Logger Provider
- DTO Pattern
- LINQ
- Paginação
- Relacionamentos (1:N e N:N)
- Tratamento Global de Exceçõe
## 📂 Estrutura do Projeto

```text
BigBangAPI
│
├── Controllers
├── Context
├── DTO
├── Extensions
├── Filters
├── Logging
├── Models
├── Repositories
│   ├── ICharacterRepository
│   └── CharacterRepository
├── Validations
├── Logs
├── Migrations
├── Program.cs
└── appsettings.json
```

---

## ⚙️ Executando Localmente

### Clonar repositório

```bash
git clone https://github.com/AnolinoJunior/BigBangAPI.git
```

### Entrar na pasta

```bash
cd BigBangAPI
```

### Restaurar dependências

```bash
dotnet restore
```

### Aplicar migrations

```bash
dotnet ef database update
```

### Executar projeto

```bash
dotnet run
```

---

## 📖 Swagger

Após iniciar a aplicação:

```text
https://localhost:5001/swagger
```

---

## 🎯 Objetivos do Projeto

Este projeto foi criado para:

- Estudo de APIs REST
- Prática com Entity Framework Core
- Modelagem de banco de dados relacional
- Deploy em ambiente real
- Portfólio de desenvolvimento backend
- Disponibilização de uma API gratuita para estudantes

---

## 🚀 Próximas Melhorias
- JWT Authentication
- Refresh Token
- Versionamento da API
- FluentValidation
- Docker
- Cache
- Testes Unitários
- Health Checks
- CI/CD com GitHub Actions
---
## 👨‍💻 Autor

Anolino Alves dos Santos Junior

GitHub:
https://github.com/AnolinoJunior

LinkedIn:
(Adicionar LinkedIn)

---

## ⭐ Contribuindo

Contribuições são bem-vindas.

Se encontrar algum problema ou tiver sugestões:

1. Faça um Fork
2. Crie uma Branch
3. Faça suas alterações
4. Abra um Pull Request

---

## 📜 Licença

Este projeto está licenciado sob a licença MIT.
