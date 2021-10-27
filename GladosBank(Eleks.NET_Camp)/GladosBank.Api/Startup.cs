using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GladosBank.Domain;
using Microsoft.EntityFrameworkCore;
using GladosBank.Services;
using System.Reflection;
using System.IO;
using Microsoft.OpenApi.Models;
using AutoMapper;

namespace GladosBank.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddRazorPages();
            services.AddDbContext<ApplicationContext>(option => 
            {
                var cs = Configuration.GetConnectionString("MyConnectionString");
                option.UseSqlServer(cs);
            });
            services.AddControllers();
            services.AddSwaggerGen
                (
                sOpt => sOpt.SwaggerDoc("v1",new OpenApiInfo() {Version="v1",Title= "GladosBank.Api" })
                ); 
            services.AddScoped<UserService>();
            //services.AddAutoMapper(typeof(Startup));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger
                    (
                    sOpt => sOpt.SerializeAsV2=true
                    );
                app.UseSwaggerUI(sOpt =>
                {
                    sOpt.SwaggerEndpoint("/swagger/v1/swagger.json", "GladosBank.Api v1");
                    sOpt.RoutePrefix = String.Empty;
                });
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
