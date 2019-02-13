using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc.Versioning.Conventions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TechoramaSessie.API.Versioning.Controllers.V1;

namespace TechoramaSessie.API.Versioning
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

            services.AddApiVersioning(o =>
            {
                //Header in the HTTP request specifing which version it wants.
                o.ApiVersionReader = new HeaderApiVersionReader("api-version");

                //Querstring based aka ?api-version=2
                //o.ApiVersionReader = new QueryStringApiVersionReader("api-version");

                //UrlSegment based requires route changes on each controller.
                //[Route("api/{v:apiVersion}/values")]
                //o.ApiVersionReader = new UrlSegmentApiVersionReader();

                // Versioning using the accep header: accept: application/json;api-version=1.0
                //o.ApiVersionReader = new MediaTypeApiVersionReader("api-version");

                //When the version is not specified using the ApiVersionReader then give the default.
                o.AssumeDefaultVersionWhenUnspecified = true;
                
                //Default if no version is specified.
                o.DefaultApiVersion = new ApiVersion(1,0);

                //Outputs in headers which versions there. Also seperates deprecated and supported versions.
                o.ReportApiVersions = true;
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

            app.UseRouting(routes =>
            {
                routes.MapApplication();
            });

            app.UseAuthorization();
        }
    }
}
