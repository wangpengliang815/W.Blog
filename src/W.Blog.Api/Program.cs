namespace W.Blog.Api
{
    using System;
    using Autofac.Extensions.DependencyInjection;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using W.Blog.Dal.DbContexts;

    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            try
            {
                using var serviceScope = host.Services.CreateScope();
                var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                context.Database.EnsureCreated();
            }
            catch (Exception ex)
            {
                Console.WriteLine("throw Exception:{0}", ex.Message);
            }
            finally
            {
                host.Run();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            const string ASPNETCORE_ENVIRONMENT = "ASPNETCORE_ENVIRONMENT";
            string envName = Environment.GetEnvironmentVariable(ASPNETCORE_ENVIRONMENT);
            string envAppSettings = $"appsettings.{envName}.json";
            IConfigurationRoot configuration = new ConfigurationBuilder()
               .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
               .AddJsonFile(envAppSettings, optional: true, reloadOnChange: true)
               .Build();
            return Host.CreateDefaultBuilder(args)
                   .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                   .ConfigureWebHostDefaults(webBuilder =>
                   {
                       webBuilder
                       .UseConfiguration(configuration)
                       .UseStartup<Startup>();
                   });
        }
    }
}
