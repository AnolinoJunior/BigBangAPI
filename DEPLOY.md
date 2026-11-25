# Deploy no Render e publicação do frontend na Somee

O backend ASP.NET Core .NET 8 roda no Render em Docker. O SQL Server permanece na Somee; o React/Vite continua sendo publicado como arquivos estáticos na Somee. A API não executa migrations durante a inicialização.

## Backend local

Instale o SDK .NET 8. Configure a conexão de desenvolvimento com User Secrets (na pasta `BigBangAPI`), sem gravar credenciais nos arquivos versionados:

```powershell
cd BigBangAPI
dotnet user-secrets set "SqlServer:Server" "SEU_SERVIDOR"
dotnet user-secrets set "SqlServer:Database" "SUA_BASE"
dotnet user-secrets set "SqlServer:TrustedConnection" "False"
dotnet user-secrets set "SqlServer:TrustServerCertificate" "True"
dotnet user-secrets set "JWT:SecretKey" "CHAVE_ALEATORIA_COM_PELO_MENOS_32_CARACTERES"
dotnet user-secrets set "JWT:ValidIssuer" "BigBangAPI"
dotnet user-secrets set "JWT:ValidAudience" "BigBangFrontend"
dotnet user-secrets set "JWT:TokenValidityInMinutes" "60"
dotnet user-secrets set "JWT:RefreshTokenValidityInMinutes" "10080"
dotnet run --launch-profile http
```

Alternativamente, forneça `ConnectionStrings__DefaultConnection` no ambiente local. A API abre em `http://localhost:5004`; Swagger em `/swagger`. A chave JWT segue a validação existente de mínimo 32 caracteres. Configure `AutoMapper:LicenseKey` por User Secrets se sua licença exigir uma chave. Nenhuma migration é aplicada automaticamente.

## Frontend local

```powershell
cd frontend
npm install
Copy-Item .env.example .env
npm run dev
```

O exemplo local aponta Axios para `http://localhost:5004/api`. `VITE_API_BASE_URL` deve terminar em `/api`. A aplicação estará em `http://localhost:5173`.

## Variáveis no Render

Crie um serviço Web a partir do repositório e escolha **Docker**. O Dockerfile está na raiz; não defina build/start command adicional. Configure estas variáveis em Environment:

| Nome | Valor |
| --- | --- |
| `ConnectionStrings__DefaultConnection` | Connection string SQL Server da Somee, guardada como segredo no Render |
| `JWT__SecretKey` | Segredo aleatório, com pelo menos 32 caracteres |
| `JWT__ValidIssuer` | Mesmo issuer usado para emitir/validar tokens, por exemplo `BigBangAPI` |
| `JWT__ValidAudience` | Mesmo audience esperado nos tokens, por exemplo `BigBangFrontend` |
| `JWT__TokenValidityInMinutes` | Validade numérica dos tokens de acesso |
| `JWT__RefreshTokenValidityInMinutes` | Validade numérica dos refresh tokens |
| `Cors__AllowedOrigins` | Origem pública do frontend, por exemplo `https://www.seudominio.com` |
| `AutoMapper__LicenseKey` | Chave da licença, se aplicável ao uso do AutoMapper |

ASP.NET Core converte `__` em `:`. Não coloque senha, cadeia de conexão, chave secreta JWT ou licença no repositório. O Render encaminha tráfego HTTPS ao container; ele escuta HTTP em `0.0.0.0:10000`, usando `ASPNETCORE_URLS`. A verificação inicial de disponibilidade pode usar `/swagger`.

Confirme no painel da Somee que o SQL Server permite conexões de saída originadas do Render e que as regras de firewall/allowlist e criptografia da connection string estão adequadas. Não habilite acesso amplo ao banco como atalho. Esse ajuste de rede depende da configuração da conta Somee.

## Publicação do backend

1. Envie os arquivos versionados ao GitHub.
2. No Render, crie **New + → Web Service** (serviço Web), conecte o repositório e selecione Docker.
3. Mantenha o Dockerfile na raiz e cadastre as variáveis acima.
4. Faça o deploy e copie a URL pública HTTPS do serviço.
5. Consulte `https://SUA-API.onrender.com/swagger` e valide uma rota que leia dados SQL.

O container publica apenas `BigBangAPI`; não inclui nem serve o frontend. Migrations existentes e banco Somee permanecem como estão.

## Geração e publicação dos arquivos React na Somee

Na pasta `frontend`, crie `.env.production` (arquivo local, não versionado) com a URL pública real da API:

```env
VITE_API_BASE_URL=https://SUA-API.onrender.com/api
```

Gere os arquivos estáticos:

```powershell
npm install
npm run build
```

Envie **o conteúdo** de `frontend/dist/` pelo Gerenciador de arquivos (File Manager) da Somee para a pasta pública do site (normalmente `wwwroot`), preservando a estrutura de diretórios e substituindo os arquivos antigos da versão do frontend. Envie `index.html`, `assets/` e demais arquivos gerados; não envie a pasta `dist` como subpasta aninhada. Configure o host para que rotas SPA do React direcionem para `index.html`, conforme o mecanismo disponível na hospedagem Somee.

Variáveis `VITE_*` são incorporadas na build e são públicas; nunca coloque segredos nelas. Para mudar URL ou domínio, gere e publique nova build.

## CORS e verificação ponta a ponta

No Render, `Cors__AllowedOrigins` recebe a origem exata do frontend, incluindo esquema e domínio, sem caminho ou barra final, por exemplo `https://www.seudominio.com`. Se houver outro domínio usado pelos visitantes, inclua-o separado por vírgula. `http://localhost:5173` permanece permitido para desenvolvimento.

1. Abra Swagger da API e confirme que carrega via HTTPS.
2. Confira se uma rota pública consegue ler registros do SQL Server da Somee; teste também autenticação/JWT quando aplicável.
3. Abra o frontend publicado, confira as abas Rede/Console do navegador para erros de CORS e respostas da API, e teste listagem, imagens e login conforme aplicável.
4. Confirme no SQL Server que os dados são os da base atual, sem alterar schema.

Se a API não conectar ao SQL Server, revise a connection string e as permissões de rede no painel Somee. Se o navegador mostrar CORS, confirme que `Cors__AllowedOrigins` corresponde exatamente à origem da barra de endereço e faça redeploy/reinicialização do serviço.

## Avisos AutoMapper

O projeto estava em AutoMapper 15.1.1. Os avisos [GHSA-6vhm-j9h7-f9gh](https://github.com/advisories/GHSA-6vhm-j9h7-f9gh) e [GHSA-gpwp-5797-mjff](https://github.com/advisories/GHSA-gpwp-5797-mjff) afetam versões anteriores à 15.1.4 na linha 15.x; as correções dessa linha estão em 15.1.4 (a linha 16 corrige em 16.3.0). Atualizei a dependência para 15.1.4, que mantém a linha principal já utilizada e limita o risco de incompatibilidade. A recomendação é permanecer em 15.1.4 ou posterior corrigida e revisar as notas da próxima atualização antes de mudar para 16.x.
