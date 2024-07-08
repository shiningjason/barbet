using System.Diagnostics;
using BarBet.Common.Logging.Details;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using Serilog.Events;

namespace BarBet.Common.Logging.Utils;

internal class BarBetLogger(string sender, LogLevel enableLogLevel) : ILogger
{
    private static int _logArgumentsCharLimit = 1000;
    private static int _logMessageCharLimit = 500;

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter
    )
    {
        ArgumentNullException.ThrowIfNull(formatter);

        if (!IsEnabled(logLevel)) return;

        var message = formatter(state, exception);
        var formattedLogValues = state as IEnumerable<KeyValuePair<string, object>>;
        var detail = formattedLogValues?.FirstOrDefault().Value as LogDetail ??
                     new LogDetail((LogEventLevel) logLevel, message, exception: exception);

        using (LogContext.PushProperty("Sender", sender))
            Log(detail);
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel >= enableLogLevel;
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return default;
    }

    private static void Log(LogDetail detail)
    {
        var level = detail.Level;
        var (template, values) = StandardizeMessage(detail);
        if (!IsLargeLog(detail))
        {
            Serilog.Log.Logger.Write(level, template, values);
            return;
        }

        var guid = Guid.NewGuid().ToString();
        Serilog.Log.Logger.Write(level, $"[Large]{template}\nlargeLogId:{guid}", values);

        values[0] = detail.Message[..Math.Min(detail.Message.Length, _logMessageCharLimit)];
        if (!string.IsNullOrWhiteSpace(detail.Arguments))
            values[1] = $"msgLength:{detail.Message.Length},argsLength:{detail.Arguments.Length}";
        Serilog.Log.Logger.Write(level, $"[SaveToLargeLog]{template}\nlargeLogId:{guid}", values);
    }

    private static bool IsLargeLog(LogDetail detail)
    {
        return detail.IsLarge ||
               detail.Message.Length > _logMessageCharLimit ||
               detail.Arguments?.Length > _logArgumentsCharLimit;
    }

    private static (string template, object[] values) StandardizeMessage(LogDetail detail)
    {
        var values = new List<object> { detail.Message };
        var prefix = detail is PerformanceDetail ? "[Perf]" : string.Empty;
        var template = $"{prefix}{{@Message}}";

        if (!string.IsNullOrWhiteSpace(detail.Arguments))
        {
            values.Add(detail.Arguments);
            template += "\narguments:{@Arguments}";
        }

        if (!string.IsNullOrWhiteSpace(detail.StackTrace))
        {
            values.Add(detail.StackTrace);
            template += "\nstackTrace:{@StackTrace}";
        }

        if (detail.Duration != null)
            template += $"\nduration:{detail.Duration.Value:0.0000}";

        var activity = Activity.Current;
        if (activity != null)
            template += $"\ntraceId:{activity.TraceId}\nspanId:{activity.SpanId}";

        return (template, values.ToArray());
    }

    internal static void SetLogCharLimit(int logMessageCharLimit, int logArgumentsCharLimit)
    {
        if (logMessageCharLimit > 0) _logMessageCharLimit = logMessageCharLimit;
        if (logArgumentsCharLimit > 0) _logArgumentsCharLimit = logArgumentsCharLimit;
    }
}