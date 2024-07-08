using Microsoft.Extensions.Logging;
using Serilog.Events;

namespace BarBet.Common.Logging.Details;

public class PerformanceDetail(
    string message,
    string? arguments = default,
    bool isLarge = false,
    LogLevel logLevel = LogLevel.Debug
) : LogDetail((LogEventLevel) logLevel, message, arguments, isLarge: isLarge);