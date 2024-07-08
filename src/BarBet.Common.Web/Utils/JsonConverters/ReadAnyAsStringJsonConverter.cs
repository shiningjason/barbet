using System.Text.Json;
using System.Text.Json.Serialization;

namespace BarBet.Common.Web.Utils.JsonConverters;

public class ReadAnyAsStringJsonConverter : JsonConverter<string>
{
    public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        switch (reader)
        {
            case { TokenType: JsonTokenType.Number }:
                return reader.TryGetInt64(out var longValue) ? longValue.ToString() : reader.GetDecimal().ToString();
            case { TokenType: JsonTokenType.String }:
                return reader.GetString() ?? string.Empty;
            default:
            {
                using var document = JsonDocument.ParseValue(ref reader);
                return document.RootElement.Clone().ToString();
            }
        }
    }

    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value);
    }
}