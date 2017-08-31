using System;
using Microsoft.AspNetCore.Hosting;

namespace hiload
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            new WebHostBuilder().UseKestrel(c=>{
#if DEBUG
                    c.Listen(System.Net.IPAddress.Any, 5000);
#endif
                    c.Limits.KeepAliveTimeout = TimeSpan.FromHours(4);
                })
                .UseStartup<Startup>()
                .Build();
    }
}
