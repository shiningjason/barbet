using System.IO.Compression;
using BarBet.Common.Web.Middlewares;
using BarBet.Common.Web.Options;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace BarBet.Common.Web.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection ConfigureHostShutdownTimeout
        (this IServiceCollection services, int shutdownTimeoutInSecond)
    {
        return services.Configure<HostOptions>(options =>
            options.ShutdownTimeout = TimeSpan.FromSeconds(shutdownTimeoutInSecond));
    }

    public static IServiceCollection ConfigureBarBetMiddlewares(this IServiceCollection services, string moduleName)
    {
        services
            .AddOptions<FirewallMiddlewareOptions>()
            .BindConfiguration($"{moduleName}:{nameof(FirewallMiddleware)}");
        services
            .AddOptions<PerformanceMiddlewareOptions>()
            .BindConfiguration($"{moduleName}:{nameof(PerformanceMiddleware)}");
        services
            .AddOptions<RequestLoggingMiddlewareOptions>()
            .BindConfiguration($"{moduleName}:{nameof(RequestLoggingMiddleware)}");
        return services;
    }

    public static IServiceCollection ConfigureResponseCompression(this IServiceCollection services, bool enableBrotli)
    {
        return services
            .AddResponseCompression(options =>
            {
                if (enableBrotli) options.Providers.Add<BrotliCompressionProvider>();
                options.Providers.Add<GzipCompressionProvider>();
            })
            .Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal);
    }

    public static IServiceCollection ConfigureSwagger(this IServiceCollection services, string moduleName)
    {
        return services
            .AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = $"{moduleName} API", Version = "v1" });
                options.CustomSchemaIds(type => type.FullName);
                options.CustomOperationIds(description =>
                {
                    var descriptor = (ControllerActionDescriptor) description.ActionDescriptor;
                    return $"{descriptor.ControllerName}.{descriptor.ActionName}";
                });
            })
            .Configure<SwaggerUIOptions>(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", $"{moduleName} API Spec V1");
                options.DocExpansion(DocExpansion.None);
                options.DocumentTitle = $"{moduleName} API Spec";
                options.DisplayOperationId();
            });
    }
}