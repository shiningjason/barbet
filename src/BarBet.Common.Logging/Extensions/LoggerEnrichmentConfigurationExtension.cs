using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;

namespace BarBet.Common.Logging.Extensions;

public static class LoggerEnrichmentConfigurationExtension
{
    public static LoggerConfiguration WithThreadId(this LoggerEnrichmentConfiguration enrich)
    {
        return enrich.With<ThreadIdEnricher>();
    }

    internal class ThreadIdEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var threadId = Environment.CurrentManagedThreadId.ToString();
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("ThreadId", threadId));
        }
    }
}