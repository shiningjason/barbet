using System.Text.RegularExpressions;
using BarBet.Common.Web.Options;
using Microsoft.AspNetCore.Routing.Template;
using Microsoft.Extensions.Options;

namespace BarBet.Common.Web.Middlewares;

public partial class FirewallMiddleware(
    IOptions<FirewallMiddlewareOptions> options,
    RequestDelegate next,
    EndpointDataSource endpointDataSource
)
{
    [GeneratedRegex("/[^.]*$", RegexOptions.IgnoreCase)]
    private static partial Regex SpaRouteRegex();

    private readonly TemplateMatcher[] _templateMatchers = endpointDataSource.Endpoints
        .Select(endpoint => endpoint as RouteEndpoint)
        .Where(routeEndpoint => routeEndpoint != null)
        .OrderBy(routeEndpoint => routeEndpoint!.Order)
        .Select(routeEndpoint => routeEndpoint!.RoutePattern.RawText)
        .Where(routePattern => routePattern != null)
        .Select(routePattern => TemplateParser.Parse(routePattern!))
        .Select(routeTemplate => new TemplateMatcher(routeTemplate, new RouteValueDictionary()))
        .ToArray();
    private readonly Regex _staticFilePathRegex = new(options.Value.StaticFilePathPattern, RegexOptions.IgnoreCase);
    private readonly int _blockStatusCode = options.Value.BlockStatusCode;

    public Task Invoke(HttpContext context)
    {
        var requestPath = context.Request.Path;
        if (requestPath.Value?.StartsWith("/swagger", StringComparison.OrdinalIgnoreCase) ?? false)
            return next(context);

        var templateMatcher = Array.Find(_templateMatchers,
            matcher => matcher.TryMatch(requestPath, new RouteValueDictionary()));
        if (templateMatcher != null && !templateMatcher.Template.TemplateText!.Equals("{*path:nonfile}"))
            return next(context);

        var requestPathString = requestPath.ToString();
        if (SpaRouteRegex().IsMatch(requestPathString) || _staticFilePathRegex.IsMatch(requestPathString))
            return next(context);

        context.Response.StatusCode = _blockStatusCode;
        return Task.CompletedTask;
    }
}