using System.Diagnostics;
using BarBet.Common.Logging.Details;
using Microsoft.Extensions.Logging;

namespace BarBet.Common.Logging.Extensions;

public static class LoggerExtension
{
    public static void Critical(this ILogger logger, CriticalDetail detail)
    {
        logger.LogCritical("{@Detail}", detail);
    }

    public static void Critical(this ILogger logger, Exception exception)
    {
        logger.Critical(new CriticalDetail(exception));
    }

    public static void Critical(this ILogger logger, string message, Exception exception)
    {
        logger.Critical(new CriticalDetail(message, exception));
    }

    public static void Debug(this ILogger logger, DebugDetail detail)
    {
        logger.LogDebug("{@Detail}", detail);
    }

    public static void Debug(this ILogger logger, string message, string? arguments = default)
    {
        logger.Debug(new DebugDetail(message, arguments));
    }

    public static void Error(this ILogger logger, ErrorDetail detail)
    {
        logger.LogError("{@Detail}", detail);
    }

    public static void Error(this ILogger logger, Exception exception)
    {
        logger.Error(new ErrorDetail(exception));
    }

    public static void Error(this ILogger logger, string message, Exception exception)
    {
        logger.Error(new ErrorDetail(message, exception));
    }

    public static void Info(this ILogger logger, InfoDetail detail)
    {
        logger.LogInformation("{@Detail}", detail);
    }

    public static void Info(this ILogger logger, string message, string? arguments = default)
    {
        logger.Info(new InfoDetail(message, arguments));
    }

    public static void Trace(this ILogger logger, TraceDetail detail)
    {
        logger.LogTrace("{@Detail}", detail);
    }

    public static void Trace(this ILogger logger, string message, string? arguments = default)
    {
        logger.Trace(new TraceDetail(message, arguments));
    }

    public static void Warning(this ILogger logger, WarningDetail detail)
    {
        logger.LogWarning("{@Detail}", detail);
    }

    public static void Warning(this ILogger logger, Exception exception)
    {
        logger.Warning(new WarningDetail(exception));
    }

    public static void Warning(this ILogger logger, string message, Exception exception)
    {
        logger.Warning(new WarningDetail(message, exception: exception));
    }

    public static void Performance(this ILogger logger, PerformanceDetail detail, TimeSpan elapsed)
    {
        detail.Duration = elapsed.TotalSeconds;
        logger.Log((LogLevel) detail.Level, "{@Detail}", detail);
    }

    public static void Performance(this ILogger logger, PerformanceDetail detail, Stopwatch stopwatch)
    {
        if (stopwatch.IsRunning) stopwatch.Stop();
        logger.Performance(detail, stopwatch.Elapsed);
    }

    public static void Performance(this ILogger logger, PerformanceDetail detail, Action action)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        action();
        logger.Performance(detail, stopwatch);
    }

    public static void Performance(this ILogger logger, string message, TimeSpan elapsed)
    {
        logger.Performance(new PerformanceDetail(message), elapsed);
    }

    public static void Performance(this ILogger logger, string message, Stopwatch stopwatch)
    {
        logger.Performance(new PerformanceDetail(message), stopwatch);
    }

    public static void Performance(this ILogger logger, string message, Action action)
    {
        logger.Performance(new PerformanceDetail(message), action);
    }
}