using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DutchTreat
{
    public class Program
    {

        public static void Main(string[] args)
        {
            // a console app that listsenes on port 80.
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(GetConfigurationSources)
                .UseStartup<Startup>() // how to listen to web requests
                .Build();

        private static void GetConfigurationSources(WebHostBuilderContext ctx, IConfigurationBuilder builder)
        {
            //removing the deafult configurtion options
            builder.Sources.Clear();

            builder.AddJsonFile("config.json", false, true)
                .AddEnvironmentVariables();

        }
    }
}
