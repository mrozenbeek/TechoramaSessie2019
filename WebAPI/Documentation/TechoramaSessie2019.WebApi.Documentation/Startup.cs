using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace TechoramaSessie2019.WebApi.Documentation
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
