namespace W.Blog.Api.Tests
{
    using System.IO;
    using System.Reflection;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using W.Blog.Api;

    public class TestBase
    {
        protected static volatile TestServer testServer = null;

        private static readonly object singleton_Lock = new object();

        public TestBase()
        {
            CreateSingleTestServer();
        }

        protected TestServer CreateSingleTestServer()
        {
            if (testServer == null)
            {
                lock (singleton_Lock)
                {
                    if (testServer == null)
                    {
                        string path = Assembly.GetAssembly(typeof(TestBase)).Location;

                        IWebHostBuilder hostBuilder =
                            new WebHostBuilder()
                                .UseContentRoot(Path.GetDirectoryName(path))
                                .ConfigureAppConfiguration(cb =>
                                {
                                    cb.AddJsonFile("appsettings.test.json", false, false);
                                })
                                .UseStartup<TestStartup>();

                        testServer = new TestServer(hostBuilder);
                    }
                }
            }
            return testServer;
        }
    }

    public class TestStartup : Startup
    {
        public TestStartup(IConfiguration configuration) : base(configuration)
        {

        }
    }
}
