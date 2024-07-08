using System.Text.Json;
using System.Text.Json.Serialization;

namespace BarBet.Common.Web.Utils.JsonConverters;

public class DateTimeJsonConverterFactory(IHttpContextAccessor httpContextAccessor, string? dateTimeFormat = default)
    : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert == typeof(DateTime) ||
               typeToConvert == typeof(DateTime?) ||
               typeToConvert == typeof(DateTimeOffset) ||
               typeToConvert == typeof(DateTimeOffset?);
    }

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var converterType = typeof(DateTimeJsonConverter<>).MakeGenericType(typeToConvert);
        return (JsonConverter) Activator.CreateInstance(converterType, httpContextAccessor, dateTimeFormat)!;
    }
}