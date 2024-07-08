using Serilog.Events;

namespace BarBet.Common.Logging.Details;

public class WarningDetail(
    string? message,
    Exception exception,
    string? arguments = default,
    bool isLarge = false,
    int? stackTraceLimit = default
) : LogDetail(
    LogEventLevel.Warning,
    message,
    arguments,
    exception,
    isLarge: isLarge,
    stackTraceLimit: stackTraceLimit
)
{
    public WarningDetail(
        Exception exception,
        string? arguments = default,
        bool isLarge = false,
        int? stackTraceLimit = default
    ) : this(default, exception, arguments, isLarge, stackTraceLimit)
    {
    }
}