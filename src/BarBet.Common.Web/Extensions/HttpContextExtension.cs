namespace BarBet.Common.Web.Extensions;

public static class HttpContextExtension
{
    private const string TimeZone = nameof(TimeZone);

    public static HttpContext SetTimeZone(this HttpContext httpContext, string timeZone)
    {
        httpContext.Items[TimeZone] = timeZone;
        return httpContext;
    }

    public static string? GetTimeZone(this HttpContext httpContext)
    {
        return httpContext.Items.TryGetValue(TimeZone, out var timeZone) ? timeZone as string : default;
    }
}