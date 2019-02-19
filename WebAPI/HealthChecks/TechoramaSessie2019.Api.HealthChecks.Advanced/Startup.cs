using TechoramaSessie2019.Api.HealthChecks.Advanced.Data;


namespace TechoramaSessie2019.Api.HealthChecks.Advanced
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

            //AspNetCore.HealthChecks.SqlServer
            //services.AddHealthChecks()
            //    .AddSqlServer("");

            //Microsoft.Extensions.Diagnostics.HealthChecks.EntityFrameworkCore
            services.AddHealthChecks()
                .AddDbContextCheck<ExampleDbContext>( tags: new string[] {"db"})
                .AddCheck("Example working", () =>
               HealthCheckResult.Healthy("Example is OK!"), tags: new string[] { "notsodb" });

            services.AddDbContext<ExampleDbContext>(options =>
            {
                options.UseSqlServer(
                    Configuration["ConnectionStrings:DefaultConnection"]);
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

            app.UseHealthChecks("/healthy", new HealthCheckOptions()
            {
                ResponseWriter = WriteResponse,
                //Predicate = EL=> EL.Tags.Contains("|db")
            });
            app.Map("/createdatabase", b => b.Run(async (context) =>
            {
                await context.Response.WriteAsync("Creating the database...");
                await context.Response.WriteAsync(Environment.NewLine);
                await context.Response.Body.FlushAsync();

                var appDbContext =
                    context.RequestServices.GetRequiredService<ExampleDbContext>();
                 appDbContext.Database.EnsureCreated();

                await context.Response.WriteAsync("Done!");
                await context.Response.WriteAsync(Environment.NewLine);
                await context.Response.WriteAsync(
                    "Navigate to /health to see the health status.");
                await context.Response.WriteAsync(Environment.NewLine);
            }));

            app.UseHttpsRedirection();

            app.UseRouting(routes =>
            {
                routes.MapApplication();
            });

            app.UseAuthorization();
        }

        private static Task WriteResponse(HttpContext httpContext,
                HealthReport result)
        {
            httpContext.Response.ContentType = "application/json";

            var json = new JObject(
                new JProperty("status", result.Status.ToString()),
                new JProperty("results", new JObject(result.Entries.Select(pair =>
                    new JProperty(pair.Key, new JObject(
                        new JProperty("status", pair.Value.Status.ToString()),
                        new JProperty("description", pair.Value.Description),
                        new JProperty("data", new JObject(pair.Value.Data.Select(
                            p => new JProperty(p.Key, p.Value))))))))));
            return httpContext.Response.WriteAsync(
                json.ToString(Formatting.Indented));
        }
    }
}
