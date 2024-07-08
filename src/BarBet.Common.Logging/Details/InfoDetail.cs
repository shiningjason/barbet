using Serilog.Events;

namespace BarBet.Common.Logging.Details;

public class InfoDetail(
    string message,
    string? arguments = default,
    Exception? exception = default,
    bool isLarge = false,
    int? stackTraceLimit = default
) : LogDetail(
    LogEventLevel.Information,
    message,
    arguments,
    exception,
    isLarge: isLarge,
    stackTraceLimit: stackTraceLimit
);