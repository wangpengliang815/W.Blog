namespace W.Blog.Api
{
    using System;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;

    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Run();
        }

        public static IWebHost CreateWebHostBuilder(string[] args)
        {
            const string ASPNETCORE_ENVIRONMENT = "ASPNETCORE_ENVIRONMENT";
            string envName = Environment.GetEnvironmentVariable(ASPNETCORE_ENVIRONMENT);
            string envAppSettings = $"appsettings.{envName}.json";
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile(envAppSettings, optional: true, reloadOnChange: true)
                .Build();

            return WebHost.CreateDefaultBuilder(args)
                 .UseConfiguration(configuration)
                 .UseStartup<Startup>()
                 .Build();
        }
    }
}
