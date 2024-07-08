using System.Collections.Concurrent;
using System.ComponentModel;
using System.Reflection;
using BarBet.Common.Core.Enums;

namespace BarBet.Common.Core.Extensions;

public static class StatusCodeExtension
{
    private static readonly ConcurrentDictionary<StatusCode, string> _messagePool = new();

    public static bool IsSuccess(this StatusCode statusCode)
    {
        return statusCode.Equals(StatusCode.COMM0000);
    }

    public static string GetMessage(this StatusCode statusCode)
    {
        return _messagePool.GetOrAdd(statusCode, key =>
        {
            var statusCodeName = key.ToString();
            var statusCodeField = typeof(StatusCode).GetField(statusCodeName);
            return $"{statusCodeName} : {statusCodeField?.GetCustomAttribute<DescriptionAttribute>()?.Description}";
        });
    }
}