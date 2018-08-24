using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace TMWork
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                   .CaptureStartupErrors(true) // the default 
                   .UseSetting("detailedErrors", "true") //Remove when issue resolved
                   .UseStartup<Startup>()
                   .Build();
    }
}
