using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace BarBet.Common.Core.Extensions;

public static class HostBuilderExtension
{
    public static IHostBuilder UseBarBetConfiguration(this IHostBuilder hostBuilder, string[]? args = default)
    {
        return hostBuilder.ConfigureAppConfiguration((context, configuration) =>
        {
            var envName = context.HostingEnvironment.EnvironmentName;

            configuration
                .AddConsul(envName)
                .AddLocalFile(envName)
                .AddEnvironmentVariables();

            if (args?.Length > 0)
                configuration.AddCommandLine(args);
        });
    }
}