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
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using GladosBank.Api.Config.Athentication;
using Microsoft.IdentityModel.Tokens;

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
            services.AddDbContext<ApplicationContext>(option => 
            {
                var cs = Configuration.GetConnectionString("MyConnectionString");
                option.UseSqlServer(cs);
            });
            services.AddControllers();
            services.AddDirectoryBrowser();

            services.AddSwaggerGen
                (
                sOpt =>
                {
                    sOpt.SwaggerDoc("v1", new OpenApiInfo() { Version = "v1", Title = "GladosBank.Api" });
                    var securityDefinitionId = "custom jwt auth";

                    var securityDefinition = new OpenApiSecurityScheme
                    {
                        Description = "Jwt Tokens using",
                        Name = securityDefinitionId,
                        In = ParameterLocation.Header,
                        Scheme = "bearer",
                        Type = SecuritySchemeType.Http,
                    };

                    OpenApiSecurityScheme securityScheme = new OpenApiSecurityScheme()
                    {
                        Reference = new OpenApiReference()
                        {
                            Id = securityDefinitionId,
                            Type = ReferenceType.SecurityScheme
                        }
                    };

                    var securityRequirements = new OpenApiSecurityRequirement()
                {
                    {securityScheme, new string[] { }},
                };

                    sOpt.AddSecurityDefinition(securityDefinitionId, securityDefinition);
                    sOpt.AddSecurityRequirement(securityRequirements);


                });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidAudience = JwtAuthenticationOptions.Audience,
                        ValidIssuer = JwtAuthenticationOptions.Issuer,
                        IssuerSigningKey = JwtAuthenticationOptions.GetSecurityKey()
                    };
                });

            services.AddSingleton<JwtGenerator>(options=>new JwtGenerator());


            services.AddCors();

            services.AddScoped<UserService>();
            services.AddAutoMapper(typeof(Startup));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();

                app.UseSwaggerUI(sOpt =>
                {
                    sOpt.SwaggerEndpoint("/swagger/v1/swagger.json", "GladosBank.Api v1");
                    sOpt.RoutePrefix = String.Empty;
                });
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            app.UseCors(pb=> 
            {
                pb.AllowAnyHeader()
                .AllowAnyOrigin()
                .AllowAnyMethod();
            });

            app.UseStaticFiles();
            app.UseDefaultFiles();

            app.UseHttpsRedirection();


            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
