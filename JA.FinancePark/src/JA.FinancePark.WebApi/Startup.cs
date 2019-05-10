using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;

namespace JA.FinancePark.WebApi
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
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "JA.FinancePark.WebApi", Version = "v1" });

                // Set the comments path for the Swagger JSON and UI.
                string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";

                string[] allXmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml");
                string xmlDocumentFile = allXmlFiles.FirstOrDefault(f => f.ToUpperInvariant().Contains(xmlFile.ToUpperInvariant()));
                if (xmlDocumentFile != null)
                {
                    c.IncludeXmlComments(xmlDocumentFile);
                }
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal);
            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
            });

            // Add memory cache services
            services.AddMemoryCache(setup =>
            {
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseResponseCompression();

            app.Use(
                       next =>
                       {
                           return async context =>
                           {
                               var stopWatch = new Stopwatch();
                               stopWatch.Start();
                               context.Response.OnStarting(
                                   () =>
                                   {
                                       stopWatch.Stop();
                                       context.Response.Headers.Add("X-ResponseTime-Ms", stopWatch.Elapsed.TotalMilliseconds.ToString());

                                       return Task.CompletedTask;
                                   });

                               await next(context);
                           };
                       });

            app.UseMvc();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "JA.FinancePark.WebApi V1");
            });
        }
    }
}
