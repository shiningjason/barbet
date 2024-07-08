using Serilog.Events;

namespace BarBet.Common.Logging.Details;

public class DebugDetail(
    string message,
    string? arguments = default,
    Exception? exception = default,
    bool isLarge = false,
    int? stackTraceLimit = default
) : LogDetail(
    LogEventLevel.Debug,
    message,
    arguments,
    exception,
    isLarge: isLarge,
    stackTraceLimit: stackTraceLimit
);