using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using BarBet.Common.Core.Enums;

namespace BarBet.Common.Core.Utils;

public static partial class StatusCodeHelper
{
    private static readonly ConcurrentDictionary<string, StatusCode> _statusCodePool = new();

    private static readonly Dictionary<Type, StatusCode> _exceptionAndStatusCodeMapping = new()
    {
        { typeof(ValidationException), StatusCode.COMM0101 },
        { typeof(SocketException), StatusCode.COMM0105 },
        { typeof(HttpRequestException), StatusCode.COMM0105 },
        { typeof(OperationCanceledException), StatusCode.COMM0111 },
        { typeof(TaskCanceledException), StatusCode.COMM0111 }
    };

    private static readonly Dictionary<string, StatusCode> _messageAndStatusCodeMapping = new()
    {
        { "Incorrect string value: '.*' for column", StatusCode.COMM0101 },
        { "Unable to connect to any of the specified MySQL hosts", StatusCode.COMM0105 },
        { "Got timeout reading communication packets", StatusCode.COMM0105 },
        { "Couldn't connect to server", StatusCode.COMM0105 },
        { "Connect Timeout expired\\.$", StatusCode.COMM0105 },
        { "An exception occurred while receiving a message from the server", StatusCode.COMM0105 },
        { "The client reset the request stream", StatusCode.COMM0105 },
        { "\"grpc_status\":14", StatusCode.COMM0105 },
        { "\"grpc_status\":4", StatusCode.COMM0110 },
        { "\"grpc_status\":1", StatusCode.COMM0111 },
        { "StatusCode=\"Cancelled\"", StatusCode.COMM0111 }
    };

    [GeneratedRegex(@"^(?<StatusCode>[A-Z]{4}\d{4})")]
    private static partial Regex PrefixStatusCodeRegex();

    public static StatusCode Parse(string statusCode)
    {
        return _statusCodePool.GetOrAdd(statusCode, key =>
        {
            if (Enum.TryParse<StatusCode>(key, true, out var result))
                return result;
            throw new Exceptions.BarBetException(StatusCode.COMM0112) { Data = { { nameof(statusCode), statusCode } } };
        });
    }

    public static bool TryParse(string statusCode, out StatusCode statusCodeResult)
    {
        try
        {
            statusCodeResult = Parse(statusCode);
            return true;
        }
        catch
        {
            statusCodeResult = default;
            return false;
        }
    }

    public static void AddExceptionAndStatusCodeMapping(IDictionary<Type, StatusCode> exceptionAndStatusCodeMapping)
    {
        foreach (var (exception, statusCode) in exceptionAndStatusCodeMapping)
            _exceptionAndStatusCodeMapping[exception] = statusCode;
    }

    public static void AddMessageAndStatusCodeMapping(IDictionary<string, StatusCode> messageAndStatusCodeMapping)
    {
        foreach (var (message, statusCode) in messageAndStatusCodeMapping)
            _messageAndStatusCodeMapping[message] = statusCode;
    }

    internal static StatusCode MapFromException(Exception exception)
    {
        if (_exceptionAndStatusCodeMapping.TryGetValue(exception.GetType(), out var statusCode))
            return statusCode;

        var messageMapping = _messageAndStatusCodeMapping
            .FirstOrDefault(mapping => Regex.IsMatch(exception.Message, mapping.Key));
        if (messageMapping.Key != null)
            return messageMapping.Value;

        var matchStatusCodePrefix = PrefixStatusCodeRegex().Match(exception.Message);
        return matchStatusCodePrefix.Success &&
               TryParse(matchStatusCodePrefix.Groups["StatusCode"].Value, out statusCode)
            ? statusCode
            : StatusCode.COMM9999;
    }
}