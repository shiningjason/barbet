using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using BarBet.Common.Core.Extensions;
using BarBet.Common.Web.Extensions;

namespace BarBet.Common.Web.Utils.JsonConverters;

public class DateTimeJsonConverter<T>(
    IHttpContextAccessor httpContextAccessor,
    string dateTimeFormat = DateTimeJsonConverter<T>.DefaultDateTimeFormat
) : JsonConverter<T>
{
    public const string DefaultDateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ssK";
    public const string DateTimeWithFractionalSecondsFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK";

    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String) return default;
        if (!reader.TryGetDateTime(out var dateTimeValue)) return default;

        var utcOffset = httpContextAccessor.HttpContext?.GetTimeZone();

        if (typeToConvert == typeof(DateTime) || typeToConvert == typeof(DateTime?))
            return dateTimeValue.ToUtc(utcOffset) is T dateTime ? dateTime : default;

        return dateTimeValue.ToOffset(utcOffset) is T dateTimeOffset ? dateTimeOffset : default;
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        var utcOffset = httpContextAccessor.HttpContext?.GetTimeZone();
        DateTimeOffset? dateTimeOffset = value switch
        {
            DateTime dateTimeValue => dateTimeValue.ToOffset(utcOffset),
            DateTimeOffset dateTimeOffsetValue => dateTimeOffsetValue.ToOffset(utcOffset),
            _ => default
        };
        if (!dateTimeOffset.HasValue)
        {
            writer.WriteNullValue();
            return;
        }

        var dateTimeString = dateTimeOffset.Value.ToString(dateTimeFormat);
        writer.WriteStringValue(JsonEncodedText.Encode(dateTimeString, JavaScriptEncoder.UnsafeRelaxedJsonEscaping));
    }
}