namespace BarBet.Common.Web.Options;

public class FirewallMiddlewareOptions
{
    public bool Enable { get; set; }

    public string StaticFilePathPattern { get; set; } =
        @"^/(assets/.+\.(css|js|jp(e|)g|png|svg|ico|eot|ttf|woff2?|json)|[^/]+\.json)$";

    public int BlockStatusCode { get; set; } = 418;
}