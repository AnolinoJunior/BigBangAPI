# 🚀 BigBangAPI — The Big Bang Theory

![.NET](https://img.shields.io/badge/.NET-8-512BD4?logo=dotnet)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-Web%20API-512BD4)
![React](https://img.shields.io/badge/React-19-61DAFB?logo=react)
![SQL Server](https://img.shields.io/badge/SQL%20Server-Database-CC2927?logo=microsoftsqlserver)
![Entity Framework Core](https://img.shields.io/badge/Entity%20Framework-Core-512BD4)
![JWT](https://img.shields.io/badge/Authentication-JWT-black?logo=jsonwebtokens)
![Swagger](https://img.shields.io/badge/Swagger-OpenAPI-85EA2D?logo=swagger)
![Licença](https://img.shields.io/badge/Licen%C3%A7a-MIT-success)

Uma **API REST** desenvolvida com **ASP.NET Core (.NET 8)**, inspirada no universo da série *The Big Bang Theory*, acompanhada por uma aplicação web desenvolvida em React.

O projeto reúne uma API para gerenciamento de personagens, episódios e localizações, além de uma plataforma administrativa que permite gerenciar essas informações por meio de uma interface web.

O desenvolvimento tem como foco a aplicação prática de conceitos de desenvolvimento backend, frontend, persistência de dados, segurança e organização de aplicações.

## ⚠️ Aviso

Este projeto foi desenvolvido exclusivamente para fins de estudo e demonstração de conhecimentos em desenvolvimento de software.

A franquia **The Big Bang Theory**, seus personagens, nomes e imagens pertencem aos respectivos detentores de direitos autorais. Este projeto não possui vínculo oficial com a Warner Bros. ou seus proprietários e não tem finalidade comercial.

---

# 🌐 Demonstração

| Aplicação | Endereço |
|---|---|
| Aplicação web | https://bigbangapi-1.onrender.com/ |
| Área administrativa | https://bigbangapi-1.onrender.com/admin |
| Swagger / OpenAPI | https://bigbangapi-1.onrender.com/swagger/index.html |
| Repositório | https://github.com/AnolinoJunior/BigBangAPI |

---

# 📸 Prévia da aplicação

## Interface administrativa

A plataforma administrativa permite gerenciar os dados da aplicação sem precisar realizar operações diretamente pelo Swagger.

### Painel administrativo

<img src="https://github.com/user-attachments/assets/312eec1b-a270-4e66-a079-342955943fa5" alt="Interface da aplicação BigBangAPI" width="745" />

### Gerenciamento de personagens

O administrador pode cadastrar e editar personagens, preencher suas informações e enviar imagens diretamente pela interface.

<!-- Adicione aqui a captura de tela do formulário de cadastro de personagens. -->

### Swagger

<img src="https://github.com/user-attachments/assets/239caea8-095f-4429-909d-fc17394b6174" alt="Documentação Swagger da BigBangAPI" width="911" />

---

# ✨ Funcionalidades

## 👥 Gerenciamento de personagens

- Cadastro de personagens.
- Consulta por identificador.
- Edição e exclusão.
- Paginação.
- Envio de imagens.
- Exibição de imagens dos personagens.
- Página de detalhes.

## 📺 Gerenciamento de episódios

- Cadastro de episódios.
- Consulta e gerenciamento.
- Edição e exclusão.

## 📍 Gerenciamento de localizações

- Cadastro de localizações.
- Consulta e gerenciamento.
- Edição e exclusão.

## 🔐 Autenticação e autorização

- ASP.NET Core Identity.
- Autenticação baseada em JWT.
- Geração de Access Token e Refresh Token.
- Controle de acesso baseado em funções (Roles).
- Política de autorização para administradores (`AdminOnly`).
- Proteção de endpoints administrativos.
- Provisionamento explícito do primeiro administrador em produção.

## 🖥 Plataforma administrativa

A aplicação web possui uma área administrativa integrada ao backend, permitindo:

- Acesso autenticado ao painel.
- Gerenciamento de personagens, episódios e localizações.
- Cadastro e edição de informações.
- Envio de imagens durante o gerenciamento de personagens.
- Exclusão de registros.
- Integração com os endpoints protegidos da API.

---

# 🏗 Arquitetura

A aplicação utiliza React no frontend para consumir os recursos disponibilizados pela ASP.NET Core Web API.

```text
                  React + Vite
                       │
                  Axios (HTTP)
                       │
                       ▼
             ASP.NET Core Web API
                       │
          ┌────────────┼────────────┐
          ▼            ▼            ▼
      Controllers  Middleware   ASP.NET Identity
          │            │            │
          ▼            ▼            ▼
      Repositories  Filters     JWT / Roles
          │
          ▼
    Unidade de Trabalho
          │
          ▼
  Entity Framework Core
          │
          ▼
      SQL Server
```

---

# 🛠 Tecnologias utilizadas

## Backend

- C#
- ASP.NET Core 8
- Entity Framework Core
- SQL Server
- ASP.NET Core Identity
- JWT Bearer Authentication
- AutoMapper
- Swagger / OpenAPI
- JSON Patch

## Frontend

- React 19
- Vite
- React Router
- Axios
- CSS Modules

## Ferramentas e infraestrutura

- Git
- GitHub
- Visual Studio
- Visual Studio Code
- Render

---

# 📚 Conceitos aplicados

### Arquitetura e organização

- API REST.
- Arquitetura em camadas.
- Padrão Repository.
- Repositório genérico.
- Unidade de Trabalho (Unit of Work).
- Injeção de dependência.

### Persistência de dados

- Entity Framework Core.
- LINQ.
- SQL Server.
- Migrations.
- Relacionamentos um para muitos (1:N).
- Relacionamentos muitos para muitos (N:N).

### Desenvolvimento de APIs

- DTOs (Data Transfer Objects).
- AutoMapper.
- Paginação.
- JSON Patch.
- Swagger / OpenAPI.

### Segurança

- ASP.NET Core Identity.
- Autenticação JWT.
- Access Token e Refresh Token.
- Autorização baseada em Roles.
- Políticas de autorização.
- Proteção de endpoints administrativos.

### Pipeline e tratamento de requisições

- Middleware.
- Action Filters.
- Exception Filters.
- Tratamento global de exceções.

### Registro de eventos

- ILogger.
- Provedor de logging personalizado.
- Registros em arquivo.

### Desenvolvimento frontend

- React.
- Axios.
- React Router.
- CSS Modules.
- Consumo de APIs REST.
- Integração com endpoints autenticados.

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
│   ├── Services
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

# ⚙ Como executar localmente

## Backend

Clone o repositório:

```bash
git clone https://github.com/AnolinoJunior/BigBangAPI.git
```

Entre na pasta do backend:

```bash
cd BigBangAPI/BigBangAPI
```

Restaure as dependências:

```bash
dotnet restore
```

Configure a connection string do SQL Server e as demais configurações necessárias no ambiente local.

Para aplicar as migrations no banco de desenvolvimento:

```bash
dotnet ef database update
```

Inicie a API:

```bash
dotnet run
```

A API estará disponível na URL indicada pelo terminal.

Swagger:

```text
http://localhost:5004/swagger
```

## Frontend

Entre na pasta:

```bash
cd frontend
```

Instale as dependências:

```bash
npm install
```

Crie um arquivo `.env` baseado no `.env.example`:

```env
VITE_API_BASE_URL=http://localhost:5004/api
```

Execute a aplicação:

```bash
npm run dev
```

Frontend:

```text
http://localhost:5173
```

---

# 🚀 Próximas melhorias

- Versionamento da API.
- FluentValidation.
- Docker.
- Redis Cache.
- Testes unitários.
- Testes de integração.
- Monitoramento e observabilidade.
- Ampliação da automação CI/CD com GitHub Actions.

---

# 👨‍💻 Autor

**Anolino Alves dos Santos Junior**

- GitHub: https://github.com/AnolinoJunior
- LinkedIn: https://www.linkedin.com/in/anolino-alves-3b1b95158

---

# 📄 Licença

Este projeto está licenciado sob a licença MIT.
