using BarBet.Common.Telemetry.Utils;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace BarBet.Common.Telemetry.Extensions;

public static class ServiceCollectionExtension
{
    private const string JaegerServiceNameEnv = "JAEGER_SERVICE_NAME";
    private const string JaegerAgentHostEnv = "JAEGER_AGENT_HOST";
    private const string JaegerAgentPortEnv = "JAEGER_AGENT_PORT";

    public static IServiceCollection AddJaegerTracing(this IServiceCollection services)
    {
        services
            .AddOpenTelemetry()
            .WithTracing(builder =>
            {
                var serviceName = Environment.GetEnvironmentVariable(JaegerServiceNameEnv);
                if (string.IsNullOrWhiteSpace(serviceName))
                    throw new ArgumentException($"{JaegerServiceNameEnv} environment variable is empty.");

                var jaegerAgentHost = Environment.GetEnvironmentVariable(JaegerAgentHostEnv);
                if (string.IsNullOrWhiteSpace(jaegerAgentHost))
                    throw new ArgumentException($"{JaegerAgentHostEnv} environment variable is empty.");

                var jaegerAgentPortString = Environment.GetEnvironmentVariable(JaegerAgentPortEnv);
                if (string.IsNullOrWhiteSpace(jaegerAgentPortString) ||
                    !int.TryParse(jaegerAgentPortString, out var jaegerAgentPort) ||
                    jaegerAgentPort == 0)
                    throw new ArgumentException($"{JaegerAgentPortEnv} environment variable is invalid.");

                builder
                    .ConfigureResource(resourceBuilder => resourceBuilder.AddService(serviceName))
                    .AddAspNetCoreInstrumentation()
                    .AddGrpcClientInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddSource(ActivityHelper.Source.Name)
                    .AddJaegerExporter(options =>
                    {
                        options.AgentHost = jaegerAgentHost;
                        options.AgentPort = jaegerAgentPort;
                    });
            });
        return services;
    }
}