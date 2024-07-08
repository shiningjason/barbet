using System.Diagnostics;
using BarBet.Common.Logging.Details;
using BarBet.Common.Logging.Extensions;
using BarBet.Common.Telemetry.Utils;

namespace BarBet.Common.Web.Middlewares;

public class PerformanceMiddleware(ILogger<PerformanceMiddleware> logger, RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        using var scope = ActivityHelper.Start(nameof(PerformanceMiddleware));

        var sw = Stopwatch.StartNew();
        await next(context).ConfigureAwait(false);

        var request = context.Request;
        var arguments = $"url:{request.Path}{request.QueryString}[{request.Method}]|thread:{GetThreadPoolStats()}";
        logger.Performance(new PerformanceDetail("Performance", arguments), sw);
    }

    private static string GetThreadPoolStats()
    {
        ThreadPool.GetMaxThreads(out var maxWorkerThreads, out var maxIoThreads);
        ThreadPool.GetAvailableThreads(out var freeWorkerThreads, out var freeIoThreads);
        ThreadPool.GetMinThreads(out var minWorkerThreads, out var minIoThreads);
        var busyIoThreads = maxIoThreads - freeIoThreads;
        var busyWorkerThreads = maxWorkerThreads - freeWorkerThreads;
        var iocp =
            $"{{\"Busy\":{busyIoThreads},\"Free\":{freeIoThreads},\"Min\":{minIoThreads},\"Max\":{maxIoThreads},\"BusyBiggerThanMin\":{busyIoThreads > minIoThreads}}}";
        var worker =
            $"{{\"Busy\":{busyWorkerThreads},\"Free\":{freeWorkerThreads},\"Min\":{minWorkerThreads},\"Max\":{maxWorkerThreads},\"BusyBiggerThanMin\":{busyWorkerThreads > minWorkerThreads}}}";
        return $"{{\"IOCP\":{iocp},\"Worker\":{worker}}}";
    }
}