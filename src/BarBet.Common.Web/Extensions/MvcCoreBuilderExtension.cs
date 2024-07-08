using System.Text.Json;
using System.Text.Json.Serialization;
using BarBet.Common.Core.Enums;
using BarBet.Common.Web.Models;
using BarBet.Common.Web.Utils.JsonConverters;
using Microsoft.AspNetCore.Mvc;

namespace BarBet.Common.Web.Extensions;

public static class MvcCoreBuilderExtension
{
    public static IMvcCoreBuilder ConfigureJsonOptions(this IMvcCoreBuilder builder)
    {
        builder.Services
            .AddOptions<JsonOptions>()
            .Configure<IHttpContextAccessor>((options, httpContextAccessor) =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.NumberHandling = JsonNumberHandling.AllowReadingFromString;
                options.JsonSerializerOptions.Converters.Add(new ReadAnyAsStringJsonConverter());
                options.JsonSerializerOptions.Converters.Add(new DateTimeJsonConverterFactory(httpContextAccessor));
            });
        return builder;
    }

    public static IMvcCoreBuilder ConfigureInvalidModelResponseHandler(this IMvcCoreBuilder builder)
    {
        return builder.ConfigureApiBehaviorOptions(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var message = default(string?);
                var hostEnvironment = context.HttpContext.RequestServices.GetRequiredService<IHostEnvironment>();
                if (hostEnvironment.IsDevelopment())
                {
                    var errors = context.ModelState.Values.SelectMany(x => x.Errors).Select(e => e.ErrorMessage);
                    message = string.Join(",", errors);
                }
                var response = new ResponseBaseModel<string>(StatusCode.COMM0101, message);
                return new JsonResult(response) { StatusCode = 200 };
            };
        });
    }
}