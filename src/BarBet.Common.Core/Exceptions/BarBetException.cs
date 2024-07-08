using BarBet.Common.Core.Enums;
using BarBet.Common.Core.Extensions;
using BarBet.Common.Core.Utils;

namespace BarBet.Common.Core.Exceptions;

public class BarBetException(StatusCode statusCode, Exception? innerException = default)
    : Exception(statusCode.GetMessage(), innerException)
{
    public StatusCode StatusCode { get; set; } = statusCode;

    public BarBetException(string statusCode, Exception? innerException = default)
        : this(StatusCodeHelper.Parse(statusCode), innerException)
    {
    }

    public object? Payload
    {
        get => Data[nameof(Payload)];
        set => Data[nameof(Payload)] = value;
    }

    public static void TryThrow(StatusCode statusCode)
    {
        if (statusCode.IsSuccess()) return;
        throw new BarBetException(statusCode);
    }

    public static void TryThrow(string statusCode)
    {
        TryThrow(StatusCodeHelper.Parse(statusCode));
    }

    public static T TryThrow<T>(Func<T> func)
    {
        try
        {
            return func();
        }
        catch (Exception exception) when (exception is not BarBetException)
        {
            throw Wrap(exception);
        }
    }

    public static void TryThrow(Action action)
    {
        TryThrow(() =>
        {
            action();
            return true;
        });
    }

    public static async Task<T> TryThrowAsync<T>(Func<Task<T>> func)
    {
        try
        {
            return await func().ConfigureAwait(false);
        }
        catch (Exception exception) when (exception is not BarBetException)
        {
            throw Wrap(exception);
        }
    }

    public static Task TryThrowAsync(Func<Task> func)
    {
        return TryThrowAsync(async () =>
        {
            await func().ConfigureAwait(false);
            return true;
        });
    }

    public static BarBetException Wrap(Exception exception)
    {
        if (exception is AggregateException { InnerExceptions.Count: 1 } aggregateException)
            exception = aggregateException.InnerExceptions.First();

        if (exception is BarBetException barBetException)
            return barBetException;

        var statusCode = StatusCodeHelper.MapFromException(exception);
        return new BarBetException(statusCode, exception);
    }
}