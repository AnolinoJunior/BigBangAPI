using System.Text.Json.Serialization;
using BigBangAPI.Context;
using BigBangAPI.Extensions;
using BigBangAPI.Filters;
using BigBangAPI.Logging;
using BigBangAPI.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

// Lê as configurações do appsettings.json
var server = configuration["SqlServer:Server"];
var database = configuration["SqlServer:Database"];
var trustedConnection = configuration["SqlServer:TrustedConnection"];
var trustServerCertificate = configuration["SqlServer:TrustServerCertificate"];

// Monta a Connection String
var connectionString =
    $"Server={server};Database={database};Trusted_Connection={trustedConnection};TrustServerCertificate={trustServerCertificate};";

// Controllers + Filtros + JSON
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ApiExceptionFilter>();
})
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler =
        ReferenceHandler.IgnoreCycles;
});

// Logging customizado
builder.Logging.AddProvider(
    new CustomLoggerProvider(
        new CustomLoggerProviderConfiguration
        {
            LogLevel = LogLevel.Warning,
            EventId = 0
        }));

// Dependency Injection
builder.Services.AddScoped<ApiLoggingFilter>();
builder.Services.AddScoped<ApiExceptionFilter>();
builder.Services.AddScoped<ICharacterRepository, CharacterRepository>();
builder.Services.AddScoped<IEpisodeRepository, EpisodeRepository>();
builder.Services.AddScoped<ILocationRepository, LocationRepository>();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Entity Framework
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

var app = builder.Build();

// Middleware global de exceções
app.ConfigureExceptionHandler();

// Swagger
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();