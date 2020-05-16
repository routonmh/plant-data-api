using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace PlantDataAPI
{
    public class Program
    {
        public static string ConnectionString { get; set; }
        public static string JwtSigningKey { get; set; }

        public static void Main(string[] args)
        {
            ConnectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
            JwtSigningKey = Environment.GetEnvironmentVariable("JWT_SIGNING_KEY");
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(config => config.AddConsole())
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}