using System.Collections.Concurrent;
using Destructurama;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace BarBet.Common.Logging.Utils;

internal class BarBetLoggerProvider : ILoggerProvider
{
    private readonly ConcurrentDictionary<string, BarBetLogger> _loggers = new();
    private readonly Dictionary<string, string> _logLevels;
    private bool _disposed;

    public BarBetLoggerProvider(IConfiguration configuration, IConfiguration loggingSection, bool consoleOutput = false)
    {
        BarBetLogger.SetLogCharLimit(
            configuration.GetValue<int>("LogMessageCharLimit"),
            configuration.GetValue<int>("LogArgumentsCharLimit"));

        var loggerConfiguration = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.FromLogContext()
            .Destructure.JsonNetTypes();
        if (consoleOutput) loggerConfiguration.WriteTo.Console();
        Log.Logger = loggerConfiguration.CreateLogger();

        _logLevels = loggingSection.GetSection("LogLevel").GetChildren()
            .Where(x => !string.IsNullOrWhiteSpace(x.Value))
            .OrderByDescending(x => x.Key.Length)
            .ToDictionary(x => x.Key, x => x.Value!);
    }

    ~BarBetLoggerProvider()
    {
        Dispose(false);
    }

    public ILogger CreateLogger(string categoryName)
    {
        return _loggers.GetOrAdd(categoryName, _ =>
        {
            var logLevel = _logLevels.FirstOrDefault(x => categoryName.StartsWith(x.Key)).Value ??
                           _logLevels.FirstOrDefault(x => x.Key.Equals("Default", StringComparison.OrdinalIgnoreCase))
                               .Value ??
                           "Trace";
            return new BarBetLogger(categoryName, Enum.Parse<LogLevel>(logLevel, true));
        });
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (_disposed) return;
        if (disposing) (Log.Logger as IDisposable)?.Dispose();
        _disposed = true;
    }
}