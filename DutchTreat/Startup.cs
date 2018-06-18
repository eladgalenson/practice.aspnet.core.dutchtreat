using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DutchTreat.Data;
using DutchTreat.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DutchTreat
{
    public class Startup
    {
        private IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IDutchRepository, DutchRepository>();
            services.AddScoped<SeedGenerator>();
            services.AddDbContext<DutchContext>(cfg =>
            {
                cfg.UseSqlServer(_configuration.GetConnectionString("DutchConnectionString"));
            });
            services.AddTransient<IMailService, NullMailService>();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if(env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
            }
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}

            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World!");
            //});
            //app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseMvc(cfg =>
            {
                cfg.MapRoute("Default",
                    "{controller}/{action}/{id?}",
                    new { controller = "App", Action = "Index" });
            });


            if (env.IsDevelopment())
            {
                //using (var scope = new env.)
                using (var scope = app.ApplicationServices.CreateScope())
                {
                    var seed = scope.ServiceProvider.GetService<SeedGenerator>();
                    seed.Seed();
                }
            }

            // Add support for node_modules but only during development **temporary**
            //https://wildermuth.com/2017/11/19/ASP-NET-Core-2-0-and-the-End-of-Bower
            //if (env.IsDevelopment())
            //{
            //    app.UseStaticFiles(new StaticFileOptions()
            //    {
            //        FileProvider = new PhysicalFileProvider(
            //          Path.Combine(Directory.GetCurrentDirectory(), @"node_modules")),
            //        RequestPath = new PathString("/vendor")
            //    });
            //}
            //this did not work for me.. the script refernce did not resolve jQuery

        }
    }
}
