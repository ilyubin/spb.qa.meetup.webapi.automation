using Destructurama;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using Serilog;

namespace AT.Github.Automation.Tests
{
    [SetUpFixture]
    public class FixtureSetup
    {
        public static IConfiguration Configuration { get; set; }

        [OneTimeSetUp]
        public void BeforeAll()
        {
            Configuration = SetupConfiguration();
            Log.Logger = SetupLogger();
        }

        [OneTimeTearDown]
        public void AfterAll()
        {
            Log.CloseAndFlush();
        }

        private static IConfigurationRoot SetupConfiguration()
        {
            const string baseConfigJson = "config.json";
            var configJson = baseConfigJson;

            #if RELEASE
                configJson = "config.release.json";
            #endif

            #if TEST
                configJson = "config.test.json";
            #endif

             return new ConfigurationBuilder()
                .AddJsonFile(baseConfigJson, optional: false)
                .AddJsonFile(configJson, optional: true)
                .AddEnvironmentVariables("AT_")
                .Build();
        }

        private static ILogger SetupLogger()
        {
            return new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithEnvironmentUserName()
                .Enrich.WithThreadId()
                .Destructure.JsonNetTypes()
                //.WriteTo.Seq(Configuration["seq:url"])
                .WriteTo.LiterateConsole(outputTemplate: "[{Timestamp:HH:mm:ss.fff}] {Message}{NewLine}{Exception}")
                .CreateLogger();
        }
    }
}
