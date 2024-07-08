using System.Net;
using BarBet.Common.Logging.Details;
using BarBet.Common.Logging.Extensions;

namespace BarBet.Common.Web.Middlewares;

public class RealIpMiddleware(ILogger<RealIpMiddleware> logger, RequestDelegate next)
{
    private const string XForwardedFor = "X-Forwarded-For";

    public async Task Invoke(HttpContext context)
    {
        var headers = context.Request.Headers;
        var hasXForwardedFor = headers.TryGetValue(XForwardedFor, out var xForwardedForValue);
        try
        {
            if (hasXForwardedFor)
            {
                var remoteIpString = xForwardedForValue.ToString()
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(remoteIpString))
                    context.Connection.RemoteIpAddress = IPAddress.Parse(remoteIpString);
            }
            context.Connection.RemoteIpAddress = context.Connection.RemoteIpAddress?.MapToIPv4();
        }
        catch (Exception exception)
        {
            var args = hasXForwardedFor ? $"{XForwardedFor}:{xForwardedForValue}" : default;
            logger.Error(new ErrorDetail(exception, args));
        }
        await next(context).ConfigureAwait(false);
    }
}