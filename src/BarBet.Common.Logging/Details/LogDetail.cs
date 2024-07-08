using BarBet.Common.Core.Enums;
using BarBet.Common.Core.Exceptions;
using Serilog.Events;

namespace BarBet.Common.Logging.Details;

public class LogDetail
{
    internal LogDetail(
        LogEventLevel level,
        string? message = default,
        string? arguments = default,
        Exception? exception = default,
        double? duration = default,
        bool isLarge = false,
        int? stackTraceLimit = default
    )
    {
        var barBetException = exception != null ? BarBetException.Wrap(exception) : default;

        Level = level;

        Message = message ?? barBetException?.Message ??
            throw new BarBetException(StatusCode.COMM0101);

        Arguments = arguments;
        if (barBetException != null)
        {
            var exceptionArguments = $"statusCode:{barBetException.StatusCode}";
            if (string.IsNullOrWhiteSpace(arguments)) Arguments = exceptionArguments;
            else Arguments += "|" + exceptionArguments;
        }

        Duration = duration;

        IsLarge = isLarge;

        StackTrace = barBetException?.ToString();
        if (StackTrace != null && stackTraceLimit != null)
            StackTrace = StackTrace[..Math.Min(stackTraceLimit.Value, StackTrace.Length)];
    }

    public LogEventLevel Level { get; }

    public string Message { get; }

    public string? Arguments { get; }

    public double? Duration { get; internal set; }

    public bool IsLarge { get; }

    public string? StackTrace { get; }

    public override string ToString()
    {
        return Message;
    }
}