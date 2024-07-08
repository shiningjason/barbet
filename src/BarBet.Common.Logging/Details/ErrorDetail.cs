using Serilog.Events;

namespace BarBet.Common.Logging.Details;

public class ErrorDetail(
    string? message,
    Exception exception,
    string? arguments = default,
    bool isLarge = false,
    int? stackTraceLimit = default
) : LogDetail(
    LogEventLevel.Error,
    message,
    arguments,
    exception,
    isLarge: isLarge,
    stackTraceLimit: stackTraceLimit
)
{
    public ErrorDetail(
        Exception exception,
        string? arguments = default,
        bool isLarge = false,
        int? stackTraceLimit = default
    ) : this(default, exception, arguments, isLarge, stackTraceLimit)
    {
    }
}