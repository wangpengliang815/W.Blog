namespace W.Blog.Api
{
    using System;
    using System.IO;
    using System.Reflection;
    using Autofac;
    using DotnetCore.Common.Options;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.DataProtection;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Models;
    using W.Blog.Dal.DbContexts;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private void AddConfigure(IServiceCollection services)
        {
            services.Configure<DbOptions>(Configuration.GetSection("DbOptions"));
        }

        private void AddService(IServiceCollection services)
        {
            IDataProtector protector = AddDataProtection(services);

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "W.Blog.Api",
                    Version = "v1",
                    Contact = new OpenApiContact { Name = "", Email = "" },
                });
                var path = Path.Combine(Path.GetDirectoryName(typeof(Program).Assembly.Location),
                    "W.Blog.Api.xml");
                c.IncludeXmlComments(path, true);
            });

            services.AddCors(options =>
            {
                options.AddPolicy("corsOptions", policy =>
                {
                    policy
                    .WithOrigins("*")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });

            if (Configuration.GetSection("DbOptions:ConnectionString") != null)
            {
                string dbConnectionString = Configuration
                    .GetSection("DbOptions:ConnectionString").Value;
                if (!string.IsNullOrEmpty(dbConnectionString))
                {
                    string unprotectDbConnectionString = protector.Unprotect(dbConnectionString);
                    services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
                    unprotectDbConnectionString), contextLifetime: ServiceLifetime.Transient);
                }
                else
                    throw new Exception("未配置数据库连接");
            }
        }

        private IDataProtector AddDataProtection(IServiceCollection services)
        {
            // 启用数据保护器
            services.AddDataProtection();
            ServiceProvider serviceProvider = services.BuildServiceProvider();
            IDataProtectionProvider protectionProvider =
                serviceProvider.GetService<IDataProtectionProvider>();
            IDataProtector protector = protectionProvider.CreateProtector(@"key_protector");
            return protector;

        }
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.Load("W.Blog.DAL"))
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(Assembly.Load("W.Blog.BLL"))
                .AsImplementedInterfaces();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            AddConfigure(services);

            AddService(services);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "version 1.0");
                c.RoutePrefix = "";
            });

            app.UseRouting();

            app.UseCors("corsOptions");
        }
    }
}
