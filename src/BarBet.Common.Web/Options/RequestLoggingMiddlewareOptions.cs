namespace BarBet.Common.Web.Options;

public class RequestLoggingMiddlewareOptions
{
    public string DisableLoggingPathPattern { get; set; } =
        @"\.(css|js|jp(e|)g|png|svg|ico|eot|ttf|woff2?|json|htm(l|)|map)$";
}