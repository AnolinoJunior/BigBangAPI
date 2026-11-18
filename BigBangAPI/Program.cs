using System.Text;
using System.Text.Json.Serialization;
using BigBangAPI.Context;
using BigBangAPI.Extensions;
using BigBangAPI.Filters;
using BigBangAPI.Logging;
using BigBangAPI.Models;
using BigBangAPI.Repositories;
using BigBangAPI.Services;
using BigBangAPI.Validations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

#region String de conexão

var server = configuration["SqlServer:Server"];
var database = configuration["SqlServer:Database"];
var trustedConnection = configuration["SqlServer:TrustedConnection"];
var trustServerCertificate = configuration["SqlServer:TrustServerCertificate"];

var connectionString = configuration.GetConnectionString("DefaultConnection")
    ?? ($"Server={server};" +
        $"Database={database};" +
        $"Trusted_Connection={trustedConnection};" +
        $"TrustServerCertificate={trustServerCertificate};");

#endregion

#region JWT

var secretKey = configuration["JWT:SecretKey"]
    ?? throw new ArgumentException("A chave JWT:SecretKey não foi encontrada.");

if (secretKey.Length < 32)
{
    throw new InvalidOperationException(
        "A chave JWT:SecretKey deve ter pelo menos 32 caracteres.");
}

#endregion

#region Controladores

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ApiExceptionFilter>();

    var messages = options.ModelBindingMessageProvider;
    messages.SetMissingBindRequiredValueAccessor(field => $"Informe o campo '{field}'.");
    messages.SetMissingKeyOrValueAccessor(() => "Informe um valor.");
    messages.SetMissingRequestBodyRequiredValueAccessor(() => "Envie um corpo para a requisição.");
    messages.SetValueMustNotBeNullAccessor(value => $"O valor '{value}' não é válido.");
    messages.SetAttemptedValueIsInvalidAccessor((value, field) => $"O valor '{value}' não é válido para o campo '{field}'.");
    messages.SetNonPropertyAttemptedValueIsInvalidAccessor(value => $"O valor '{value}' não é válido.");
    messages.SetUnknownValueIsInvalidAccessor(field => $"O valor informado para o campo '{field}' não é válido.");
    messages.SetNonPropertyUnknownValueIsInvalidAccessor(() => "O valor informado não é válido.");
    messages.SetValueIsInvalidAccessor(value => $"O valor '{value}' não é válido.");
    messages.SetValueMustBeANumberAccessor(field => $"O campo '{field}' deve conter um número.");
    messages.SetNonPropertyValueMustBeANumberAccessor(() => "Informe um número.");
})
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler =
        ReferenceHandler.IgnoreCycles;
})
.AddNewtonsoftJson(options => options.AllowInputFormatterExceptionMessages = false)
.ConfigureApiBehaviorOptions(options =>
{
    var defaultResponseFactory = options.InvalidModelStateResponseFactory;
    options.InvalidModelStateResponseFactory = context =>
    {
        var result = defaultResponseFactory(context);
        if (result is ObjectResult { Value: ValidationProblemDetails details })
        {
            details.Title = "Um ou mais campos contêm erros de validação.";

            foreach (var (field, errors) in details.Errors)
            {
                for (var index = 0; index < errors.Length; index++)
                {
                    errors[index] = errors[index] switch
                    {
                        "The input was not valid." => "O conteúdo enviado é inválido.",
                        var message when message == $"The {field} field is required." => $"Informe o campo '{field}'.",
                        _ => errors[index]
                    };
                }
            }
        }

        return result;
    };

    options.ClientErrorMapping[400].Title = "Requisição inválida";
    options.ClientErrorMapping[401].Title = "Autenticação necessária";
    options.ClientErrorMapping[403].Title = "Acesso negado";
    options.ClientErrorMapping[404].Title = "Recurso não encontrado";
    options.ClientErrorMapping[415].Title = "Tipo de mídia não compatível";
});

#endregion

#region Logs

builder.Logging.AddProvider(
    new CustomLoggerProvider(
        new CustomLoggerProviderConfiguration
        {
            LogLevel = LogLevel.Warning,
            EventId = 0
        }));

#endregion

#region Banco de dados

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

#endregion

#region Identity

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddErrorDescriber<PortugueseIdentityErrorDescriber>()
    .AddDefaultTokenProviders();

#endregion

#region Autenticação

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

#region Autorização

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
    {
        policy.RequireRole("Admin");
    });
});

#endregion

#region Injeção de dependência

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
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "BigBangAPI.xml"));

    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "BigBang API",
        Version = "v1",
        Description = "Consulte e gerencie personagens, episódios e localizações de The Big Bang Theory."
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Autenticação por JWT. No Swagger UI, informe apenas o token. O cabeçalho da requisição usa o formato Bearer {seu_token}.",
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
    options.AddPolicy("ReactPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .WithExposedHeaders("X-Pagination");
    });
});

#endregion

var app = builder.Build();

#region Pipeline HTTP

app.ConfigureExceptionHandler();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.DocumentTitle = "Documentação da BigBang API";
    options.SwaggerEndpoint("v1/swagger.json", "BigBang API v1");
});

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseCors("ReactPolicy");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("index.html");

#endregion

app.Run();  