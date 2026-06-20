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

- Listagem de personagens
- Consulta de personagem por ID
- Listagem de episódios
- Consulta de episódio por ID
- Listagem de locais
- Busca por nome
- Paginação
- Relacionamento Personagem ↔ Episódio
- Versionamento da API
- Swagger/OpenAPI
- Seed automático do banco

---

## 🛠️ Tecnologias

- ASP.NET Core Web API
- .NET 8
- Entity Framework Core
- MySQL
- Swagger / OpenAPI
- SOMEE Hosting
- Git & GitHub

---

## 📂 Estrutura do Projeto

```text
BigBangAPI
│
├── Controllers
│   ├── CharactersController
│   ├── EpisodesController
│   └── LocationsController
│
├── DTOs
│
├── Models
│   ├── Character
│   ├── Episode
│   ├── Location
│   └── CharacterEpisode
│
├── Data
│   ├── AppDbContext
│   └── SeedData
│
├── Repositories
│
├── Services
│
├── Migrations
│
├── Program.cs
└── appsettings.json
```

---

## 🗄️ Modelo de Dados

### Character

| Campo | Tipo |
|---------|---------|
| Id | int |
| Name | string |
| Status | string |
| Gender | string |
| Occupation | string |
| ImageUrl | string |

### Episode

| Campo | Tipo |
|---------|---------|
| Id | int |
| Title | string |
| SeasonNumber | int |
| EpisodeNumber | int |
| AirDate | DateTime |

### Location

| Campo | Tipo |
|---------|---------|
| Id | int |
| Name | string |
| Type | string |
| Description | string |

---

## 🔗 Relacionamentos

### Character ↔ Episode

Um personagem pode aparecer em vários episódios.

Um episódio pode conter vários personagens.

Relacionamento Many-to-Many através da tabela:

```text
CharacterEpisode
├── CharacterId
└── EpisodeId
```

---

## 📡 Endpoints

### Characters

#### Listar personagens

```http
GET /api/v1/characters
```

#### Buscar personagem por ID

```http
GET /api/v1/characters/1
```

#### Buscar por nome

```http
GET /api/v1/characters?name=sheldon
```

#### Paginação

```http
GET /api/v1/characters?page=1&pageSize=20
```

---

### Episodes

#### Listar episódios

```http
GET /api/v1/episodes
```

#### Buscar episódio por ID

```http
GET /api/v1/episodes/1
```

---

### Locations

#### Listar locais

```http
GET /api/v1/locations
```

#### Buscar local por ID

```http
GET /api/v1/locations/1
```

---

## 📦 Exemplo de Resposta

```json
{
  "id": 1,
  "name": "Sheldon Cooper",
  "occupation": "Theoretical Physicist",
  "episodes": [
    {
      "id": 1,
      "title": "Pilot"
    },
    {
      "id": 2,
      "title": "The Big Bran Hypothesis"
    }
  ]
}
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
