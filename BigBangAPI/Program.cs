    using BigBangAPI.Context;
    using BigBangAPI.Extensions;
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

    builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler =
                System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        });

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(connectionString));

    var app = builder.Build();

    app.ConfigureExceptionHandler();

    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();