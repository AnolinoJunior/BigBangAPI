using System.IO;
using Microsoft.Extensions.Logging;

namespace BigBangAPI.Logging;

public class CustomLogger : ILogger
{
    readonly string loggerName;

    readonly CustomLoggerProviderConfiguration loggerConfig;

    public CustomLogger(string name, CustomLoggerProviderConfiguration config)
    {
        loggerName = name;
        loggerConfig = config;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel == loggerConfig.LogLevel;
    }

    public IDisposable? BeginScope<TState>(TState state)
        where TState : notnull
    {
        return null;
    }

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
            return;

        string mensagem =
            $"{logLevel}: {eventId.Id} - {formatter(state, exception)}";

        EscreverTextoNoArquivo(mensagem);
    }

    private void EscreverTextoNoArquivo(string mensagem)
    {
        string pastaLogs = Path.Combine(
            Directory.GetCurrentDirectory(),
            "Logs");

        if (!Directory.Exists(pastaLogs))
        {
            Directory.CreateDirectory(pastaLogs);
        }

        string caminhoArquivoLog = Path.Combine(
            pastaLogs,
            "BigBangAPI_Log.txt");

        try
        {
            using StreamWriter streamWriter =
                new StreamWriter(caminhoArquivoLog, true);

            streamWriter.WriteLine(mensagem);
        }
        catch (Exception)
        {
            throw;
        }
    }
}