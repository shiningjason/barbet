using System.Text;
using System.Text.Json;
using BarBet.Common.Core.Exceptions;
using BarBet.Common.Logging.Details;
using BarBet.Common.Logging.Extensions;
using BarBet.Common.Web.Attributes;
using BarBet.Common.Web.Models;

namespace BarBet.Common.Web.Middlewares;

public class ExceptionMiddleware(
    ILogger<ExceptionMiddleware> logger,
    IHostEnvironment env,
    RequestDelegate next
)
{
    private readonly bool _isDev = env.IsDevelopment();

    public async Task Invoke(HttpContext context)
    {
        try
        {
            context.Request.EnableBuffering();
            await next(context).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            var exception = BarBetException.Wrap(ex);

            if (!context.RequestAborted.IsCancellationRequested)
            {
                var disableRequestBodyLogging =
                    context.GetEndpoint()?.Metadata.GetMetadata<DisableRequestBodyLoggingAttribute>() != null;

                var request = context.Request;
                var requestBody = disableRequestBodyLogging
                    ? "-"
                    : await ReadHttpRequestBody(request).ConfigureAwait(false);
                logger.Error(new ErrorDetail(exception,
                    $"url:{request.Path}{request.QueryString}[{request.Method}]|request.content:{requestBody}"));
            }

            var responseData = new ResponseBaseModel<object>(
                exception.StatusCode,
                exception.Payload,
                _isDev ? exception.ToString() : exception.Message
            );
            var responseJson = JsonSerializer.Serialize(responseData);
            var response = context.Response;
            response.ContentType = "application/json";
            await response.WriteAsync(responseJson, Encoding.UTF8).ConfigureAwait(false);
        }
    }

    private static async Task<string> ReadHttpRequestBody(HttpRequest request)
    {
        try
        {
            SeekToBegin();
            using var reader = new StreamReader(request.Body);
            var body = await reader.ReadToEndAsync().ConfigureAwait(false);
            SeekToBegin();
            return string.IsNullOrWhiteSpace(body) ? string.Empty : body;
        }
        catch
        {
            return string.Empty;
        }

        void SeekToBegin()
        {
            if (request.Body.CanSeek) request.Body.Seek(0, SeekOrigin.Begin);
        }
    }
}