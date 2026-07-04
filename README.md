# 🚀 The Big Bang Theory API

Uma API REST pública inspirada na Rick and Morty API, criada para fornecer informações estruturadas sobre o universo da série The Big Bang Theory.

O projeto disponibiliza dados sobre personagens, episódios, locais e relacionamentos da série através de endpoints simples e bem documentados.

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
- Dependency Injection
- Middleware
- Action Filters
- Logging (ILogger)
- Custom Logger Provider
- LINQ
- DTO Pattern
- REST API
- Git
- GitHub
- Somee Hosting
---
## 🧩 Recursos Implementados

- Injeção de Dependência (Dependency Injection)
- Middleware personalizado para tratamento global de exceções
- Action Filter personalizado
- Logger personalizado utilizando ILogger
- Logger Provider personalizado
- Paginação utilizando Skip() e Take()
- Carregamento de relacionamentos com Include()
- Projeções utilizando DTO
- Serialização JSON configurada para evitar ciclos
- Tratamento de erros HTTP
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
├── Logs
├── Migrations
├── Models
├── Validations
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

- Cache
- Docker
- Rate Limiting
- Testes Unitários
- GraphQL
- Autenticação JWT

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
