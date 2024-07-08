using BarBet.Common.Logging.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BarBet.Common.Logging.Extensions;

public static class HostBuilderExtension
{
    public static IHostBuilder UseBarBetLogging(this IHostBuilder hostBuilder, string loggingSectionName)
    {
        return hostBuilder.ConfigureServices((context, services) =>
        {
            var configuration = context.Configuration;
            var rootSection = configuration.GetSection("Global") as IConfiguration;
            if (!rootSection.GetChildren().Any()) rootSection = configuration;

            if (!rootSection.GetChildren().Any(x => x.Key.Equals("Serilog")))
                throw new ArgumentException("The Serilog section doesn't exist or is incorrect.");

            var isDev = context.HostingEnvironment.IsDevelopment();
            var loggingSection = configuration.GetSection(isDev ? "Logging" : loggingSectionName);
            var loggerProvider = new BarBetLoggerProvider(rootSection, loggingSection, isDev);
            services.AddLogging(logging =>
            {
                logging.AddConfiguration(loggingSection);
                logging.AddProvider(loggerProvider);
            });
        });
    }
}