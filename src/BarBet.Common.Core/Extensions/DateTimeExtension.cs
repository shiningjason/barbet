using System.Globalization;
using System.Text.RegularExpressions;

namespace BarBet.Common.Core.Extensions;

public static partial class DateTimeExtension
{
    [GeneratedRegex(@"^[+-]\d{2}:\d{2}$")]
    private static partial Regex UtcOffsetRegex();

    public static DateTimeOffset ToOffset(this DateTime dateTime, TimeSpan offset)
    {
        if (dateTime == DateTime.MinValue) return DateTimeOffset.MinValue;
        return dateTime.Kind == DateTimeKind.Unspecified
            ? new DateTimeOffset(dateTime, offset)
            : new DateTimeOffset(dateTime).ToOffset(offset);
    }

    public static DateTimeOffset ToOffset(this DateTime dateTime, string? utcOffset)
    {
        return dateTime.ToOffset(ParseUtcOffset(utcOffset));
    }

    public static DateTime ToUtc(this DateTime dateTime, string? utcOffset)
    {
        return dateTime.ToOffset(utcOffset).UtcDateTime;
    }

    public static DateTimeOffset ToOffset(this DateTimeOffset dateTimeOffset, string? utcOffset)
    {
        return dateTimeOffset.ToOffset(ParseUtcOffset(utcOffset));
    }

    private static TimeSpan ParseUtcOffset(string? utcOffset)
    {
        if (string.IsNullOrEmpty(utcOffset))
            return TimeSpan.Zero;
        if (!UtcOffsetRegex().IsMatch(utcOffset))
            throw new ArgumentException($"Invalid UTC offset: '{utcOffset}'.");
        return TimeSpan.Parse(utcOffset.Replace("+", string.Empty), CultureInfo.InvariantCulture);
    }
}