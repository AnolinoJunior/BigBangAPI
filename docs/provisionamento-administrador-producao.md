# Provisionamento do primeiro administrador em produção

Este procedimento cria a role `Admin`, cria (ou localiza) uma conta administrativa e a associa à role usando ASP.NET Core Identity. A senha é processada pelo Identity e nunca é gravada diretamente pelo provisionador. IDs e hashes do banco local não são reutilizados.

O provisionamento só é iniciado pelo argumento explícito `--provision-admin` e exige `Production`. Não é executado no start normal da API, não executa migrations e não abre uma rota pública para cadastro.

## Pré-requisitos

- A versão da API que contém o comando já foi compilada ou implantada.
- As tabelas do ASP.NET Core Identity já existem no banco Somee e correspondem às migrations aplicadas à aplicação. O comando não cria tabelas nem atualiza o schema.
- A máquina que executa o comando consegue abrir uma conexão de saída com o SQL Server Somee. A disponibilidade de acesso remoto depende do plano Somee.
- A role `Admin` pode existir ou não. Se existir, será reutilizada.
- Execute uma única instância por vez. Não configure o argumento como Start Command ou Pre-deploy Command.

## Variáveis de ambiente

O .NET carrega variáveis de ambiente para configuração. Os dois sublinhados (`__`) representam `:`. São necessárias:

| Variável | Uso |
| --- | --- |
| `ASPNETCORE_ENVIRONMENT` | Deve ser `Production`; a execução local também deve defini-la explicitamente. |
| `ConnectionStrings__DefaultConnection` | Cadeia de conexão com o SQL Server Somee correto. |
| `JWT__SecretKey` | Chave JWT da API, com pelo menos 32 caracteres; o `Program.cs` exige-a também no modo de provisionamento. |
| `JWT__ValidIssuer` e `JWT__ValidAudience` | Valores da configuração de produção da API. |
| `AdminProvisioning__UserName` | Nome da conta administrativa desejada. |
| `AdminProvisioning__Email` | E-mail da conta administrativa desejada. |
| `AdminProvisioning__Password` | Senha nova e forte. |

Se a configuração de produção usa `AutoMapper__LicenseKey`, forneça também esse valor ao processo local. Não coloque cadeia de conexão, chaves, senha ou licença em código, comandos literais, histórico do PowerShell ou arquivos versionados. Use variáveis do processo e prompts protegidos como no exemplo abaixo. A senha e a cadeia de conexão ficam disponíveis em texto claro no ambiente do processo enquanto o comando roda; execute em uma máquina confiável e feche a sessão ao terminar.

## Execução local pelo PowerShell

Execute a partir da raiz do repositório. O script guarda os valores de ambiente do processo, pede os valores sensíveis sem ecoá-los, chama o comando explícito e restaura as variáveis no `finally`, inclusive se o comando falhar.

```powershell
$names = @(
    'ASPNETCORE_ENVIRONMENT',
    'ConnectionStrings__DefaultConnection',
    'JWT__SecretKey',
    'JWT__ValidIssuer',
    'JWT__ValidAudience',
    'AutoMapper__LicenseKey',
    'AdminProvisioning__UserName',
    'AdminProvisioning__Email',
    'AdminProvisioning__Password'
)

$previous = @{}
foreach ($name in $names) {
    $previous[$name] = [Environment]::GetEnvironmentVariable($name, 'Process')
}

$secureConnectionString = $null
$secureJwtKey = $null
$secureAutoMapperLicenseKey = $null
$secureAdminPassword = $null

try {
    $env:ASPNETCORE_ENVIRONMENT = 'Production'

    $secureConnectionString = Read-Host 'Cadeia de conexão do SQL Server Somee' -AsSecureString
    $env:ConnectionStrings__DefaultConnection =
        [System.Net.NetworkCredential]::new('', $secureConnectionString).Password

    $secureJwtKey = Read-Host 'JWT SecretKey de produção' -AsSecureString
    $env:JWT__SecretKey =
        [System.Net.NetworkCredential]::new('', $secureJwtKey).Password

    $env:JWT__ValidIssuer = Read-Host 'JWT ValidIssuer de produção'
    $env:JWT__ValidAudience = Read-Host 'JWT ValidAudience de produção'
    $env:AdminProvisioning__UserName = Read-Host 'Nome de usuário administrador'
    $env:AdminProvisioning__Email = Read-Host 'E-mail administrador'

    $secureAdminPassword = Read-Host 'Nova senha administrativa' -AsSecureString
    $env:AdminProvisioning__Password =
        [System.Net.NetworkCredential]::new('', $secureAdminPassword).Password

    # Se a API usa AutoMapper__LicenseKey em produção, informe-a aqui.
    # O valor também será removido ou restaurado no finally.
    $secureAutoMapperLicenseKey = Read-Host 'AutoMapper LicenseKey (Enter se não aplicável)' -AsSecureString
    $autoMapperLicenseKey =
        [System.Net.NetworkCredential]::new('', $secureAutoMapperLicenseKey).Password
    if (-not [string]::IsNullOrEmpty($autoMapperLicenseKey)) {
        $env:AutoMapper__LicenseKey = $autoMapperLicenseKey
    }

    dotnet run --project .\BigBangAPI\BigBangAPI.csproj --no-launch-profile -- --provision-admin
    if ($LASTEXITCODE -ne 0) {
        throw "O provisionamento terminou com código $LASTEXITCODE."
    }
}
finally {
    foreach ($name in $names) {
        $value = $previous[$name]
        if ($null -eq $value) {
            Remove-Item "Env:$name" -ErrorAction SilentlyContinue
        }
        else {
            [Environment]::SetEnvironmentVariable($name, $value, 'Process')
        }
    }

    foreach ($secureValue in @(
        $secureConnectionString,
        $secureJwtKey,
        $secureAutoMapperLicenseKey,
        $secureAdminPassword
    )) {
        if ($null -ne $secureValue) {
            $secureValue.Dispose()
        }
    }

    $secureConnectionString = $null
    $secureJwtKey = $null
    $secureAutoMapperLicenseKey = $null
    $secureAdminPassword = $null
    $autoMapperLicenseKey = $null
    $previous.Clear()
}
```

`--no-launch-profile` impede que um perfil local sobrescreva o ambiente `Production`. `dotnet run` compila e executa a API com o argumento depois de `--`; o `Program.cs` remove esse argumento da configuração do host, executa o provisionamento e retorna sem iniciar o pipeline HTTP. O processo usa a connection string Somee informada acima, não o banco local.

## Execução no Render

Também é possível executar em um Shell efêmero ou job isolado do Render, configurando as mesmas variáveis no ambiente do serviço e executando:

```sh
dotnet BigBangAPI.dll --provision-admin
```

Não coloque `--provision-admin` no Start Command nem no Pre-deploy Command. A disponibilidade do Shell efêmero depende do tipo/plano do serviço; consulte a [documentação de Shell e SSH do Render](https://render.com/docs/ssh). Como alternativa, veja a [documentação de jobs avulsos](https://render.com/docs/one-off-jobs).

## Verificação do resultado

1. O processo deve terminar com código de saída zero e imprimir um resumo indicando se a role, o usuário e a associação foram criados ou já existiam. A saída não deve conter senha ou e-mail.
2. Faça uma consulta somente leitura às tabelas `AspNetRoles`, `AspNetUsers` e `AspNetUserRoles`, ou valide pelo login da aplicação e por uma operação administrativa protegida. Não insira, atualize ou exclua esses dados manualmente.
3. Depois do sucesso, remova as variáveis `AdminProvisioning__UserName`, `AdminProvisioning__Email` e `AdminProvisioning__Password` do ambiente usado e feche a sessão PowerShell. O script restaura as variáveis do processo ao final; isso não altera variáveis salvas no perfil do usuário ou no painel do Render.

## Cuidados com o banco de produção

- Confirme que `ConnectionStrings__DefaultConnection` aponta para a base Somee de produção correta e que o plano permite acesso remoto. Faça backup conforme o procedimento operacional da equipe antes da execução.
- Confirme por leitura que a estrutura do Identity já está aplicada; este comando não executa migrations nem garante a existência das tabelas.
- Execute uma instância por vez. Não rode como parte de cada inicialização da API.
- A criação da role, a criação do usuário e a associação são operações Identity distintas e não estão envolvidas por uma única transação. Uma falha pode deixar a role criada sem usuário ou o usuário criado sem associação. Corrija a causa e execute novamente com o mesmo nome, e-mail e senha para completar as etapas pendentes.
- Não copie o ID nem o hash da conta local. O Identity gera novos IDs e cria o hash da senha informada.
- A senha existente não é redefinida. A verificação usa `IPasswordHasher.VerifyHashedPassword`, que não persiste rehash quando retorna `SuccessRehashNeeded`.

## Idempotência e conflitos

- Se `Admin` já existir, ela será reutilizada.
- Se o usuário com o mesmo nome e e-mail já existir e a senha conferir, o provisionador só cria a associação caso ela ainda não exista.
- Se nome e e-mail apontarem para contas diferentes, se apenas um deles pertencer a outra conta, ou se a senha não conferir, o processo aborta sem criar role, usuário ou associação.
- Se uma etapa de gravação falhar, as gravações anteriores não são revertidas automaticamente. Uma execução posterior pode completar o estado, desde que os dados e a senha correspondam à conta criada.

## Origem da conta de desenvolvimento

O código de `AuthController.Register` cria usuários com `UserManager.CreateAsync`, mas não cria a role nem atribui o usuário a ela. `CreateRole` e `AddUserToRole` são protegidos por `AdminOnly`, então o repositório não permite determinar como a conta e a associação de desenvolvimento foram originalmente provisionadas. Elas podem ter sido criadas por ação externa ao código versionado. Este procedimento usa IDs gerados pelo Identity no banco de produção.
