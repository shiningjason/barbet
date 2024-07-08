using BarBet.Application.Web.Constants;
using BarBet.Application.Web.Services.Auth;
using BarBet.Common.Telemetry.Extensions;
using BarBet.Common.Web.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;

namespace BarBet.Application.Web;

public class Startup
{
    public void Configure(IApplicationBuilder app, IHostEnvironment env)
    {
        if (env.IsDevelopment()) app.UseDeveloperExceptionPage();
        if (env.IsProduction()) app.UseHsts();

        app.UseRouting();
        app.UseCors();
        app.UseResponseCaching();
        app.UseResponseCompression();

        if (env.IsProduction())
        {
            var fileProvider = new PhysicalFileProvider(Path.Join(env.ContentRootPath, "ClientApp/build"));
            app.UseStaticFiles(new StaticFileOptions { FileProvider = fileProvider });
        }

        app.UseBarBetMiddlewares();

        if (Config.Web.EnableSwagger)
            app.UseSwagger().UseSwaggerUI();

        app.UseAuthentication();
        app.UseEndpoints(builder =>
        {
            builder.MapHealthChecks("/health");
            builder.MapControllers();
        });
    }

    public void ConfigureServices(IServiceCollection services)
    {
        ConfigureMvcCore(services);
        ConfigureResponseCaching(services);
        ConfigureAuthentication(services);

        services
            .ConfigureHostShutdownTimeout(Config.Global.AppSettings.ShutdownTimeoutInSecond)
            .ConfigureBarBetMiddlewares(Config.WebSectionKey)
            .ConfigureResponseCompression(Config.Web.EnableBrotli)
            .ConfigureSwagger(Config.WebSectionKey)
            .AddJaegerTracing()
            .AddHttpContextAccessor()
            .AddMemoryCache()
            .AddHealthChecks();

        // Services
        // services.AddSingleton<ICacheService, ConcurrentCacheService>();

        // Grpc Service Clients
    }

    private static void ConfigureMvcCore(IServiceCollection services)
    {
        services
            .AddMvcCore()
            .AddApiExplorer()
            .AddDataAnnotations()
            .AddCors(options =>
                options.AddDefaultPolicy(policy => policy
                    .WithOrigins(Config.Web.CorsPolicy.Origins)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                ))
            .ConfigureJsonOptions()
            .ConfigureInvalidModelResponseHandler();
    }

    private static void ConfigureResponseCaching(IServiceCollection services)
    {
        services
            .AddResponseCaching(options =>
            {
                options.MaximumBodySize = Config.Web.ResponseCacheMaximumBodySize;
                options.SizeLimit = Config.Web.ResponseCacheSizeLimit;
            })
            .AddControllers(options =>
            {
                options.CacheProfiles.Add(CacheProfileName.SsrIndex, new CacheProfile
                {
                    Duration = Config.Web.SsrIndexResponseCacheDurationInSecond,
                    Location = ResponseCacheLocation.Any
                });
                options.CacheProfiles.Add(CacheProfileName.SsrAsset, new CacheProfile
                {
                    Duration = Config.Web.SsrAssetResponseCacheDurationInSecond,
                    Location = ResponseCacheLocation.Any
                });
            });
    }

    private static void ConfigureAuthentication(IServiceCollection services)
    {
        services
            .AddAuthentication(BarBetAuthenticationHandler.AuthenticationScheme)
            .AddBarBet(options => { });
    }
}