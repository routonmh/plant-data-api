using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using PlantDataAPI.Middlewares;

namespace PlantDataAPI
{
    /// <summary>
    ///
    /// </summary>
    public class Startup
    {
        public IConfiguration Configuration { get; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options => { options.EnableEndpointRouting = false; })
                .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

            services.AddOptions();
            services.AddHttpClient();

            // Swagger Documentation setup
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Plant Data API",
                    Version = "v1",
                    Description = "Description",
                    TermsOfService = new Uri("https://github.com/routonmh"),
                    Contact = new OpenApiContact
                    {
                        Name = "Matthew Routon",
                        Email = "routonmh@gmail.com",
                        Url = new Uri("https://github.com/routonmh"),
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddSwaggerGenNewtonsoftSupport();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="logger"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            // Setup API Documentation
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Data API");
                options.RoutePrefix = "docs";
            });

            // app.UseMiddleware<RequireLocalAuthentication>();

            app.Use(async (context, next) =>
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                logger.LogInformation("Request from: {0} on {1}",
                    context.Request.Host, context.Request.Path);

                await next();

                sw.Stop();
                logger.LogInformation("Finished: {0} - {1} {2}ms",
                    context.Request.Path, context.Response.StatusCode,
                    sw.ElapsedMilliseconds);
            });

            app.UseMvc();
        }
    }
}