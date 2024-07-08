// ReSharper disable All
using System;
using Microsoft.Extensions.Configuration;

namespace BarBet.Application.Web;

public static class Config
{
    private static IConfiguration? _instance;
    private static IConfiguration Instance =>
        _instance ?? throw new InvalidOperationException("Configuration has not been initialized.");

    public static void Initialize(IConfiguration config)
    {
        _instance = config;
    }

    private static T GetValue<T>(string key, T defaultValue = default(T)) =>
        Instance.GetValue<T>(key) ?? defaultValue;

    private static T GetSection<T>(string key, T defaultValue = default(T)) =>
        Instance.GetSection(key).Get<T>() ?? defaultValue;

    public const string GlobalSectionKey = "Global";
    public static class Global<T>
    {
        private static T? _value;
        public static T Get() => _value ??= GetSection<T>(GlobalSectionKey);
    }

    public static class Global
    {
        public const string DomainServiceSectionKey = "Global:DomainService";
        public static class DomainService<T>
        {
            private static T? _value;
            public static T Get() => _value ??= GetSection<T>(DomainServiceSectionKey);
        }

        public static class DomainService
        {
        }

        public const string ModuleSectionKey = "Global:Module";
        public static class Module<T>
        {
            private static T? _value;
            public static T Get() => _value ??= GetSection<T>(ModuleSectionKey);
        }

        public static class Module
        {
        }

        public const string GrpcNetworkSettingSectionKey = "Global:GrpcNetworkSetting";
        public static class GrpcNetworkSetting<T>
        {
            private static T? _value;
            public static T Get() => _value ??= GetSection<T>(GrpcNetworkSettingSectionKey);
        }

        public static class GrpcNetworkSetting
        {
            public const string ClientSectionKey = "Global:GrpcNetworkSetting:Client";
            public static class Client<T>
            {
                private static T? _value;
                public static T Get() => _value ??= GetSection<T>(ClientSectionKey);
            }

            public static class Client
            {
                private static bool? _enableCustomerSetting;
                public static bool EnableCustomerSetting => _enableCustomerSetting ??= GetValue<bool>("Global:GrpcNetworkSetting:Client:EnableCustomerSetting");

                private static int? _grpc_keepalive_permit_without_calls;
                public static int grpc_keepalive_permit_without_calls => _grpc_keepalive_permit_without_calls ??= GetValue<int>("Global:GrpcNetworkSetting:Client:grpc.keepalive_permit_without_calls");

                private static int? _grpc_http2_max_pings_without_data;
                public static int grpc_http2_max_pings_without_data => _grpc_http2_max_pings_without_data ??= GetValue<int>("Global:GrpcNetworkSetting:Client:grpc.http2.max_pings_without_data");

                private static int? _grpc_keepalive_time_ms;
                public static int grpc_keepalive_time_ms => _grpc_keepalive_time_ms ??= GetValue<int>("Global:GrpcNetworkSetting:Client:grpc.keepalive_time_ms");
            }
        }

        public const string AppSettingsSectionKey = "Global:AppSettings";
        public static class AppSettings<T>
        {
            private static T? _value;
            public static T Get() => _value ??= GetSection<T>(AppSettingsSectionKey);
        }

        public static class AppSettings
        {
            private static int? _shutdownTimeoutInSecond;
            public static int ShutdownTimeoutInSecond => _shutdownTimeoutInSecond ??= GetValue<int>("Global:AppSettings:ShutdownTimeoutInSecond");

            private static int? _httpClientTimeoutInSecond;
            public static int HttpClientTimeoutInSecond => _httpClientTimeoutInSecond ??= GetValue<int>("Global:AppSettings:HttpClientTimeoutInSecond");
        }
    }

    public const string WebSectionKey = "Web";
    public static class Web<T>
    {
        private static T? _value;
        public static T Get() => _value ??= GetSection<T>(WebSectionKey);
    }

    public static class Web
    {
        public const string LoggingSectionKey = "Web:Logging";
        public static class Logging<T>
        {
            private static T? _value;
            public static T Get() => _value ??= GetSection<T>(LoggingSectionKey);
        }

        public static class Logging
        {
            public const string LogLevelSectionKey = "Web:Logging:LogLevel";
            public static class LogLevel<T>
            {
                private static T? _value;
                public static T Get() => _value ??= GetSection<T>(LogLevelSectionKey);
            }

            public static class LogLevel
            {
                private static string? _default;
                public static string Default => _default ??= GetValue<string>("Web:Logging:LogLevel:Default");

                private static string? _barBet;
                public static string BarBet => _barBet ??= GetValue<string>("Web:Logging:LogLevel:BarBet");

                private static string? _system;
                public static string System => _system ??= GetValue<string>("Web:Logging:LogLevel:System");

                private static string? _microsoft;
                public static string Microsoft => _microsoft ??= GetValue<string>("Web:Logging:LogLevel:Microsoft");
            }
        }

        public const string CorsPolicySectionKey = "Web:CorsPolicy";
        public static class CorsPolicy<T>
        {
            private static T? _value;
            public static T Get() => _value ??= GetSection<T>(CorsPolicySectionKey);
        }

        public static class CorsPolicy
        {
            private static string[]? _origins;
            public static string[] Origins => _origins ??= GetSection<string[]>("Web:CorsPolicy:Origins");
        }

        private static int? _defaultCacheDurationInSecond;
        public static int DefaultCacheDurationInSecond => _defaultCacheDurationInSecond ??= GetValue<int>("Web:DefaultCacheDurationInSecond");

        private static bool? _enableBrotli;
        public static bool EnableBrotli => _enableBrotli ??= GetValue<bool>("Web:EnableBrotli");

        private static bool? _enableSwagger;
        public static bool EnableSwagger => _enableSwagger ??= GetValue<bool>("Web:EnableSwagger");

        private static int? _responseCacheMaximumBodySize;
        public static int ResponseCacheMaximumBodySize => _responseCacheMaximumBodySize ??= GetValue<int>("Web:ResponseCacheMaximumBodySize");

        private static int? _responseCacheSizeLimit;
        public static int ResponseCacheSizeLimit => _responseCacheSizeLimit ??= GetValue<int>("Web:ResponseCacheSizeLimit");

        private static int? _ssrIndexResponseCacheDurationInSecond;
        public static int SsrIndexResponseCacheDurationInSecond => _ssrIndexResponseCacheDurationInSecond ??= GetValue<int>("Web:SsrIndexResponseCacheDurationInSecond");

        private static int? _ssrAssetResponseCacheDurationInSecond;
        public static int SsrAssetResponseCacheDurationInSecond => _ssrAssetResponseCacheDurationInSecond ??= GetValue<int>("Web:SsrAssetResponseCacheDurationInSecond");

        public const string GoogleAuthenticationSectionKey = "Web:GoogleAuthentication";
        public static class GoogleAuthentication<T>
        {
            private static T? _value;
            public static T Get() => _value ??= GetSection<T>(GoogleAuthenticationSectionKey);
        }

        public static class GoogleAuthentication
        {
            private static string? _clientId;
            public static string ClientId => _clientId ??= GetValue<string>("Web:GoogleAuthentication:ClientId");

            private static string? _clientSecret;
            public static string ClientSecret => _clientSecret ??= GetValue<string>("Web:GoogleAuthentication:ClientSecret");
        }

        public const string FirewallMiddlewareSectionKey = "Web:FirewallMiddleware";
        public static class FirewallMiddleware<T>
        {
            private static T? _value;
            public static T Get() => _value ??= GetSection<T>(FirewallMiddlewareSectionKey);
        }

        public static class FirewallMiddleware
        {
            private static bool? _enable;
            public static bool Enable => _enable ??= GetValue<bool>("Web:FirewallMiddleware:Enable");
        }

        public const string PerformanceMiddlewareSectionKey = "Web:PerformanceMiddleware";
        public static class PerformanceMiddleware<T>
        {
            private static T? _value;
            public static T Get() => _value ??= GetSection<T>(PerformanceMiddlewareSectionKey);
        }

        public static class PerformanceMiddleware
        {
            private static bool? _enable;
            public static bool Enable => _enable ??= GetValue<bool>("Web:PerformanceMiddleware:Enable");
        }
    }
}

