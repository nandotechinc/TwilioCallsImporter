using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using TwilioCallsImporter.Services;

namespace TwilioCallsImporter
{
    public class Program
    {
        public static IConfiguration Configuration { get; }
        public static IServiceProvider ServiceProvider { get; }
        static Program()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();

            var services = new ServiceCollection();

            services.AddSingleton(Configuration);
            services.AddScoped<ICallData, CallData>();
            services.AddSingleton<Application>();
            ServiceProvider = services.BuildServiceProvider();
        }
        public static void Main(string[] args)
        {
            try
            {
                var app = ServiceProvider.GetService<Application>();

                app.Run().Wait();
            }
            finally
            {
                ((IDisposable)ServiceProvider).Dispose();
            }
        }
    }
}
