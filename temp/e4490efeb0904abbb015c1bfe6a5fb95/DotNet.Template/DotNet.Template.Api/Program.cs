using System;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace DotNet.Template.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                   .UseStartup<Startup>()
                   .UseKestrel()
                   .ConfigureAppConfiguration((hostingContext, config) =>
                                                {
                                                    IHostingEnvironment env = hostingContext.HostingEnvironment;

                                                    string environmentName = env.EnvironmentName;
                                                    string basePath = Directory.GetCurrentDirectory();

                                                    Console.WriteLine("EnvironmentName : " + environmentName);
                                                    Console.WriteLine("Env Base Path : " + basePath);

                                                    config.SetBasePath(basePath)
                                                          .AddJsonFile("appsettings.json")
                                                          .AddJsonFile("sharedsettings.json");

                                                    if (!string.IsNullOrEmpty(environmentName))
                                                    {
                                                        config.AddJsonFile($"appsettings.{environmentName}.json");
                                                        config.AddJsonFile($"sharedsettings.{environmentName}.json");
                                                    }

                                                    config.AddEnvironmentVariables();
                                                })
                   .Build();
    }
}