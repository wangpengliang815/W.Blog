namespace W.Blog.Api
{
    using System;
    using System.IO;
    using Autofac.Extensions.DependencyInjection;
    using AutoMapper;
    using DotnetCore.Common.Helper;
    using DotnetCore.Common.Options;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.DataProtection;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.OpenApi.Models;
    using W.Blog.Dal.DbContexts;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        /// <summary>
        /// 配置添加
        /// </summary>
        /// <param name="services"></param>
        private void AddConfigure(IServiceCollection services)
        {
            services.Configure<DbOptions>(Configuration.GetSection("DbOptions"));
        }

        /// <summary>
        /// 服务添加
        /// </summary>
        /// <param name="services"></param>
        private void AddService(IServiceCollection services)
        {
            IDataProtector protector = AddDataProtection(services);

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


            //services.AddAutoMapper(typeof(Startup));

            if (Configuration.GetSection("DbOptions:ConnectionString") != null)
            {
                string dbConnectionString = Configuration
                    .GetSection("DbOptions:ConnectionString").Value;
                if (!string.IsNullOrEmpty(dbConnectionString))
                {
                    string unprotectDbConnectionString = protector.Unprotect(dbConnectionString);
                    services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
                    unprotectDbConnectionString));
                }
                else
                    throw new Exception("未配置数据库连接");
            }
            services.AddAutoMapper(typeof(Startup));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        /// <summary>
        /// 数据保护
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Autofac替换DI
        /// </summary>
        /// <param name="services"></param>
        private void UseIoc(IServiceCollection services)
        {
            AutofacHelper.Begin();
            AutofacHelper.Builder.Populate(services);
            AutofacHelper.Registers("W.Blog.DAL", "W.Blog.BLL");
            AutofacHelper.End();
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            AddConfigure(services);

            AddService(services);

            UseIoc(services);

            return new AutofacServiceProvider(AutofacHelper.Container);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "version 1.0");
                c.RoutePrefix = "";
            });

            app.UseSwagger();

            app.UseCors("corsOptions");

            app.UseMvc();
        }
    }
}
