using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace ProjectRenamer.Api
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
            .UseKestrel(o => { o.Limits.KeepAliveTimeout = TimeSpan.FromSeconds(90); })
            .Build();
    }
}
