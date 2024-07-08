using System.Text.Json.Serialization;
using BarBet.Common.Core.Enums;
using BarBet.Common.Core.Extensions;
using BarBet.Common.Core.Utils;

namespace BarBet.Common.Web.Models;

public class ResponseBaseModel<T>(
    StatusCode code,
    T? data = default,
    string? message = default,
    object? carrier = default
)
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public StatusCode Code { get; set; } = code;

    public T? Data { get; set; } = data;

    [JsonPropertyName("msg")]
    public string Message { get; set; } = message ?? code.GetMessage();

    public object? Carrier { get; set; } = carrier;

    public ResponseBaseModel(string code, T? data = default, string? message = default, object? carrier = default)
        : this(StatusCodeHelper.Parse(code), data, message, carrier)
    {
    }
}