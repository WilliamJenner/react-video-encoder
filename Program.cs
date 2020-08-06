using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Json;

namespace VideoEncoderReact
{
    public class Program
    {
        public static readonly LoggingLevelSwitch LoggingLevelSwitch = new LoggingLevelSwitch { MinimumLevel = LogEventLevel.Verbose };
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.ControlledBy(LoggingLevelSwitch)
            .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
            .Enrich.FromLogContext()
            .WriteTo.File(
                new JsonFormatter(renderMessage: true),
                Path.Combine(AppContext.BaseDirectory, "logs//Serilog.json"),
                shared: true,
                fileSizeLimitBytes: 20_971_520,
                rollOnFileSizeLimit: true,
                retainedFileCountLimit: 10)
            .CreateLogger();
            try
            {
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception exception)
            {
                Log.Error(exception, "Oh snap we broke on startup");
                throw;
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
              .ConfigureWebHostDefaults(webBuilder =>
              {
                  webBuilder.UseStartup<Startup>();
              });
    }
}
