using BarBet.Application.Web;
using BarBet.Common.Core.Extensions;
using BarBet.Common.Logging.Extensions;
using dotenv.net;

DotEnv.Load();

var host = Host
    .CreateDefaultBuilder(args)
    .ConfigureHostConfiguration(config => config.AddEnvironmentVariables("ASPNETCORE_"))
    .UseBarBetConfiguration()
    .UseBarBetLogging(Config.Web.LoggingSectionKey)
    .ConfigureServices((context, _) => Config.Initialize(context.Configuration))
    .ConfigureWebHostDefaults(builder => builder.UseStartup<Startup>())
    .Build();

var logger = host.Services.GetRequiredService<ILogger<Program>>();
try
{
    AppDomain.CurrentDomain.UnhandledException += (_, args) => logger.Error((args.ExceptionObject as Exception)!);

    ThreadPool.GetMinThreads(out var minWorkerThreads, out var minIoThreads);
    logger.Info("Program - Start", $"minWorkerThreads:{minWorkerThreads}|minIoThreads:{minIoThreads}");

    var lifetime = host.Services.GetRequiredService<IHostApplicationLifetime>();
    lifetime.ApplicationStarted.Register(() => logger.Info("Application - Started"));
    lifetime.ApplicationStopping.Register(() => logger.Info("Application - Stopping"));
    lifetime.ApplicationStopped.Register(() => logger.Info("Application - Stopped"));
    host.Run();

    logger.Info("Program - Stopped");
}
catch (Exception ex)
{
    logger.Critical("Program - Crash", ex);
    throw;
}