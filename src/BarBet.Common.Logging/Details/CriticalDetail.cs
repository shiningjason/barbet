using Serilog.Events;

namespace BarBet.Common.Logging.Details;

public class CriticalDetail(
    string? message,
    Exception exception,
    string? arguments = default,
    bool isLarge = false,
    int? stackTraceLimit = default
) : LogDetail(
    LogEventLevel.Fatal,
    message,
    arguments,
    exception,
    isLarge: isLarge,
    stackTraceLimit: stackTraceLimit
)
{
    public CriticalDetail(
        Exception exception,
        string? arguments = default,
        bool isLarge = false,
        int? stackTraceLimit = default
    ) : this(default, exception, arguments, isLarge, stackTraceLimit)
    {
    }
}