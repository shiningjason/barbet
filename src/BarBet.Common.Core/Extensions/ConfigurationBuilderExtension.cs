using System.Text;
using System.Text.Json.Nodes;
using BarBet.Common.Core.Enums;
using BarBet.Common.Core.Exceptions;
using Microsoft.Extensions.Configuration;

namespace BarBet.Common.Core.Extensions;

internal static class ConfigurationBuilderExtension
{
    internal static IConfigurationBuilder AddLocalFile(this IConfigurationBuilder builder, string envName)
    {
        return builder
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false, false)
            .AddJsonFile($"appsettings.{envName}.json", true, false);
    }

    internal static IConfigurationBuilder AddConsul(this IConfigurationBuilder builder, string envName)
    {
        var configuration = builder
            .AddLocalFile(envName)
            .AddEnvironmentVariables()
            .Build();

        var ip = configuration["Consul:IP"];
        var port = configuration["Consul:Port"];
        var modules = configuration["Consul:Module"];
        if (string.IsNullOrWhiteSpace(ip) || string.IsNullOrWhiteSpace(port) || string.IsNullOrWhiteSpace(modules))
            throw new InvalidOperationException("Consul section doesn't exist or is incorrect in appsettings.json.");

        try
        {
            using var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri($"http://{ip}:{port}/v1/kv/");

            foreach (var module in modules.Split(','))
            {
                var response = httpClient.GetStringAsync(module).ConfigureAwait(false).GetAwaiter().GetResult();
                var base64Value = JsonNode.Parse(response)?.AsArray()[0]?["Value"]?.ToString() ??
                                  throw new FormatException($"Consul response for the key '{module}' is unexpected.");
                var json = $"{{\"{module}\":{Encoding.UTF8.GetString(Convert.FromBase64String(base64Value))}}}";
                builder.AddJsonStream(new MemoryStream(Encoding.UTF8.GetBytes(json)));
            }
        }
        catch (Exception ex)
        {
            throw new BarBetException(StatusCode.COMM0105, ex);
        }

        return builder;
    }
}