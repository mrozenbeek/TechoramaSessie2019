using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc.Versioning.Conventions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TechoramaSessie.API.Versioning.Conventions.Controllers.V1;

namespace TechoramaSessie.API.Versioning.Conventions
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
                o.DefaultApiVersion = new ApiVersion(1, 0);

                //Outputs in headers which versions there. Also seperates deprecated and supported versions.
                o.ReportApiVersions = true;

                o.Conventions.Controller<Version1Controller>()
                                        .HasDeprecatedApiVersion(1, 0);

                o.Conventions.Controller<Version2Controller>()
                                        .HasApiVersion(2, 0);

                o.Conventions.Controller<Version3Controller>()
                                        .HasApiVersion(3, 0);
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
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
