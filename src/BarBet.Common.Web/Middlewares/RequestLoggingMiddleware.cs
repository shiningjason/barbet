using System.Diagnostics;
using System.Text.RegularExpressions;
using BarBet.Common.Logging.Details;
using BarBet.Common.Logging.Extensions;
using BarBet.Common.Web.Options;
using Microsoft.Extensions.Options;

namespace BarBet.Common.Web.Middlewares;

public class RequestLoggingMiddleware(
    ILogger<RequestLoggingMiddleware> logger,
    IOptions<RequestLoggingMiddlewareOptions> options,
    RequestDelegate next
)
{
    private readonly Regex _disableLoggingPathRegex =
        new(options.Value.DisableLoggingPathPattern, RegexOptions.IgnoreCase);

    public async Task Invoke(HttpContext context)
    {
        if (_disableLoggingPathRegex.IsMatch(context.Request.Path))
        {
            await next(context).ConfigureAwait(false);
            return;
        }

        var ip = context.Connection.RemoteIpAddress?.ToString();
        var ipArgs = string.IsNullOrWhiteSpace(ip) ? string.Empty : $"ip:{ip}|";
        var request = context.Request;
        var requestArgs = $"url:{request.Path}{request.QueryString}[{request.Method}]{ipArgs}";
        logger.Info("Request", requestArgs);

        var sw = Stopwatch.StartNew();
        await next(context).ConfigureAwait(false);

        var responseArgs = $"{requestArgs}|response.status:{context.Response.StatusCode}";
        logger.Performance(new PerformanceDetail("Response", responseArgs, logLevel: LogLevel.Information), sw);
    }
}