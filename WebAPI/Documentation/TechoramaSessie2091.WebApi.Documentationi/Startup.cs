using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;

namespace TechoramaSessie2091.WebApi.Documentationi
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
            services.AddMvc()
                   .AddJsonOptions(options =>
                   {
                       options.SerializerSettings.Formatting = Formatting.Indented;
                   })
                .AddNewtonsoftJson();
          
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "My API",
                    Description = "My First ASP.NET Core Web API",
                    TermsOfService = "None",
                    Contact = new Contact() { Name = "Michael Rozenbeek", Email = "m.rozenbee@webintegration.nl", Url = "www.tweakers.net" }
                });

                c.SwaggerDoc("v2", new Info
                {
                    Version = "v2",
                    Title = "My API 2",
                    Description = "My First ASP.NET Core Web API 2",
                    TermsOfService = "None",
                    Contact = new Contact() { Name = "Michael Rozenbeek", Email = "m.rozenbee@webintegration.nl", Url = "www.tweakers.net" }
                });
                
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

            //Just a json document with specifications
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "apireference/{documentName}/swagger.json";
            });

            //Complete fancy UI enzo
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/apireference/v1/swagger.json", "v1 docs");
                c.SwaggerEndpoint("/apireference/v2/swagger.json", "v2 docs");
            });

            app.UseHttpsRedirection();

            app.UseRouting(routes =>
            {
                routes.MapApplication();
            });

            app.UseAuthorization();
        }
    }
}
