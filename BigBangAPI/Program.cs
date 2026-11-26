using System.Text;
using System.Text.Json.Serialization;
using BigBangAPI.Context;
using BigBangAPI.Extensions;
using BigBangAPI.Filters;
using BigBangAPI.Logging;
using BigBangAPI.Models;
using BigBangAPI.Repositories;
using BigBangAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

const string provisionAdminArgument = "--provision-admin";
var shouldProvisionAdmin = args.Contains(provisionAdminArgument, StringComparer.Ordinal);
var builderArguments = args
    .Where(argument => !string.Equals(argument, provisionAdminArgument, StringComparison.Ordinal))
    .ToArray();

var builder = WebApplication.CreateBuilder(builderArguments);

var configuration = builder.Configuration;

#region Connection String

var server = configuration["SqlServer:Server"];
var database = configuration["SqlServer:Database"];
var trustedConnection = configuration["SqlServer:TrustedConnection"];
var trustServerCertificate = configuration["SqlServer:TrustServerCertificate"];

var connectionString = configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrWhiteSpace(connectionString))
{
    if (!builder.Environment.IsDevelopment() ||
        string.IsNullOrWhiteSpace(server) ||
        string.IsNullOrWhiteSpace(database))
    {
        throw new InvalidOperationException(
            "A cadeia de conexão DefaultConnection não foi configurada.");
    }

    connectionString = $"Server={server};" +
        $"Database={database};" +
        $"Trusted_Connection={trustedConnection};" +
        $"TrustServerCertificate={trustServerCertificate};";
}

#endregion

#region JWT

var secretKey = configuration["JWT:SecretKey"]
    ?? throw new ArgumentException("A chave JWT:SecretKey não foi configurada.");

if (secretKey.Length < 32)
{
    throw new InvalidOperationException(
        "A chave JWT:SecretKey deve conter pelo menos 32 caracteres.");
}

#endregion

#region Controllers

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ApiExceptionFilter>();
})
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler =
        ReferenceHandler.IgnoreCycles;
})
.AddNewtonsoftJson();

#endregion

#region Logging

builder.Logging.AddProvider(
    new CustomLoggerProvider(
        new CustomLoggerProviderConfiguration
        {
            LogLevel = LogLevel.Warning,
            EventId = 0
        }));

#endregion

#region Database

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

#endregion

#region Identity

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddErrorDescriber<BigBangAPI.Localization.DescritorDeErrosIdentityPtBr>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

#endregion

#region Authentication

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme =
            JwtBearerDefaults.AuthenticationScheme;

        options.DefaultChallengeScheme =
            JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;

        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ClockSkew = TimeSpan.Zero,

                ValidIssuer = configuration["JWT:ValidIssuer"],
                ValidAudience = configuration["JWT:ValidAudience"],

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(secretKey))
            };
    });

#endregion

#region Authorization

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
    {
        policy.RequireRole("Admin");
    });
});

#endregion

#region Dependency Injection

builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddScoped<ApiLoggingFilter>();
builder.Services.AddScoped<ApiExceptionFilter>();

builder.Services.AddScoped<ICharacterRepository, CharacterRepository>();
builder.Services.AddScoped<IEpisodeRepository, EpisodeRepository>();
builder.Services.AddScoped<ILocationRepository, LocationRepository>();

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddScoped<IUnityOfWork, UnityOfWork>();

builder.Services.AddAutoMapper(
    options =>
    {
        options.LicenseKey = configuration["AutoMapper:LicenseKey"];
    },
    AppDomain.CurrentDomain.GetAssemblies());

#endregion

#region Swagger

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Big Bang API",
        Version = "v1",
        Description = "API para gerenciamento de personagens, episódios e localizações do universo The Big Bang Theory."
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Informe: Bearer {seu_token}",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    options.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
});

#endregion

#region CORS

builder.Services.AddCors(options =>
{
    var allowedOrigins = (configuration["Cors:AllowedOrigins"] ?? string.Empty)
        .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
        .Prepend("http://localhost:5173")
        .Distinct(StringComparer.OrdinalIgnoreCase)
        .ToArray();

    options.AddPolicy("ReactPolicy", policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .WithExposedHeaders("X-Pagination");
    });
});

#endregion

var app = builder.Build();

if (shouldProvisionAdmin)
{
    try
    {
        if (!app.Environment.IsProduction())
        {
            throw new InvalidOperationException(
                "O provisionamento do administrador só pode ser executado no ambiente Production.");
        }

        var adminUserName = configuration["AdminProvisioning:UserName"];
        var adminEmail = configuration["AdminProvisioning:Email"];
        var adminPassword = configuration["AdminProvisioning:Password"];

        if (string.IsNullOrWhiteSpace(adminUserName) ||
            string.IsNullOrWhiteSpace(adminEmail) ||
            string.IsNullOrEmpty(adminPassword))
        {
            throw new InvalidOperationException(
                "Configure AdminProvisioning:UserName, AdminProvisioning:Email e AdminProvisioning:Password para executar o provisionamento.");
        }

        using var scope = app.Services.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<ApplicationUser>>();

        var result = await AdminAccountProvisioner.ProvisionAsync(
            userManager,
            roleManager,
            passwordHasher,
            adminUserName,
            adminEmail,
            adminPassword);

        Console.WriteLine(
            $"Provisionamento concluído: role Admin {(result.RoleCreated ? "criada" : "já existente")}; " +
            $"usuário {(result.UserCreated ? "criado" : "já existente")}; " +
            $"associação {(result.RoleAssignmentCreated ? "criada" : "já existente")}. ");
    }
    finally
    {
        await app.DisposeAsync();
    }

    return;
}

#region Pipeline

app.ConfigureExceptionHandler();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.DocumentTitle = "Documentação da API Big Bang";
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Big Bang API v1");
});

app.UseStaticFiles();

app.UseCors("ReactPolicy");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

#endregion

app.Run();
