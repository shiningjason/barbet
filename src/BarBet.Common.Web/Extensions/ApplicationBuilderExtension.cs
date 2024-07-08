using BarBet.Common.Web.Middlewares;
using BarBet.Common.Web.Options;
using Microsoft.Extensions.Options;

namespace BarBet.Common.Web.Extensions;

public static class ApplicationBuilderExtension
{
    public static IApplicationBuilder UseBarBetMiddlewares(this IApplicationBuilder app)
    {
        var firewallMiddlewareOptions =
            app.ApplicationServices.GetRequiredService<IOptions<FirewallMiddlewareOptions>>();
        if (firewallMiddlewareOptions.Value.Enable)
            app.UseMiddleware<FirewallMiddleware>();

        var performanceMiddlewareOptions =
            app.ApplicationServices.GetRequiredService<IOptions<PerformanceMiddlewareOptions>>();
        if (performanceMiddlewareOptions.Value.Enable)
            app.UseMiddleware<PerformanceMiddleware>();

        app.UseMiddleware<RealIpMiddleware>();
        app.UseMiddleware<RequestLoggingMiddleware>();
        app.UseMiddleware<ExceptionMiddleware>();

        return app;
    }
}